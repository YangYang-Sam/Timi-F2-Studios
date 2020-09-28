using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

namespace NetTest
{
    class NetManager
    {
        public static String USERID;
        public static Socket socket;
        private IPEndPoint ipe;

        public void InitState()
        {
            int port = 8000;
            string host = "106.52.156.98";
            ///创建终结点EndPoint
            IPAddress ip = IPAddress.Parse(host);
            ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例
            ///创建socket并连接到服务器
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
            socket.Connect(ipe);//连接到服务器

            //开一个子线程，用于接收数据
            Thread ReceiveThread = new Thread(new ThreadStart(ReceiveThreadFunc));
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();

        }



        // 子线程中等待接收的方法
        public void ReceiveThreadFunc()
        {
            while (true)
            {
                byte[] data = new byte[1024];
                ReceiveData(ref data);
                PBConverter.ResData(data);
            }
        }

        // 注册时向服务器发送请求
        public void ReqRegister(String uid, String password)
        {
            byte[] data = PBConverter.ReqRegister(uid, password);
            SendData(data);
        }

        // 请求匹配
        public void ReqMatching(String uid)
        {
            byte[] data = PBConverter.ReqMatching(uid);
            SendData(data);
        }

        // 注册请求后服务器的返回
        public int ResRegister()
        {
            byte[] data = new byte[1024];
            ReceiveData(ref data);
            int res = PBConverter.ResRegister(data);
            return res;
        }

        // 登录时向服务器发送请求
        public void ReqLogin(String uid, String password)
        {
            USERID = uid;
            byte[] data = PBConverter.ReqLogin(uid, password);
            SendData(data);
        }

        // 登录请求后服务器的返回
        public int ResLogin()
        {
            byte[] data = new byte[1024];
            ReceiveData(ref data);
            int res = PBConverter.ResLogin(data);
            if (res == 0)
            {
                //NTY
                AsyncReceiveNty();
            }
            return res;
        }

        private async Task AsyncReceiveNty()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    byte[] data = new byte[1024];
                    ReceiveData(ref data);
                    NtyLoginOut nlo = new NtyLoginOut();
                    bool isNty = PBConverter.ResNtyLoginOut(data, ref nlo);
                    if (isNty == true)
                    {
                        //打断主进程
                        Console.WriteLine("this is nty");
                        break;
                    }
                    Console.WriteLine("no Nty");
                }
            });
        }

        // 请求退出登录
        public void ReqLoginOut(string uid)
        {
            byte[] data = PBConverter.ReqLoginOut(uid);
            SendData(data);
        }

        // 返回退出登录
        public int ResLoginOut()
        {
            byte[] data = new byte[1024];
            ReceiveData(ref data);
            int res = PBConverter.ResLoginOut(data);
            if (res == 0)
            {
                socket.Close();
            }
            return res;
        }

        // socket发送数据
        public void SendData(byte[] data)
        {
            //向服务器发送信息
            socket.Send(data, data.Length, 0);//发送信息
        }

        // socket接收数据
        public void ReceiveData(ref byte[] recvBytes)
        {
            //接受从服务器返回的信息
            int bytes;
            bytes = socket.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
            byte[] resRecvBytes = new byte[bytes];
            for (int i = 0; i < bytes; i++)
            {
                resRecvBytes[i] = recvBytes[i];
            }
            recvBytes = resRecvBytes;
        }
    }
}
