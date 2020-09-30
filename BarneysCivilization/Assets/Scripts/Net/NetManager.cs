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
        public static NetManager instance;
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
            byte[] data = new byte[1024];
            PBConverter.ReqRegister(uid, password, ref data);
            SendData(data);
        }

        // 请求匹配
        public void ReqMatching(String uid, int raceIndex)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqMatching(uid, raceIndex, ref data);
            SendData(data);
        }

        public void ReqStopMatching(String uid)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqStopMatching(uid, ref data);
            SendData(data);
        }

        // 登录时向服务器发送请求
        public void ReqLogin(String uid, String password)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqLogin(uid, password, ref data);
            SendData(data);
        }

        // 请求退出登录
        public void ReqLoginOut(string uid)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqLoginOut(uid, ref data);
            SendData(data);
        }

        // 发送移动指令
        public void ReqSetDestiny(string uid,int index)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqSetDestiny(uid, index, ref data);
            SendData(data);
        }

        public void ReqUseCard(string uid, int cardID, int hexID)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqUseCard(uid, cardID, hexID, ref data);
            SendData(data);
        }

        // 发送回合结束指令
        public void ReqEndTurn(string uid)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqTurnEnd(uid, ref data);
            SendData(data);
        }
        // 发送玩家结束指令   
        public void ReqGameEnd(string uid)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqGameEnd(uid, ref data);
            SendData(data);
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
            for(int i = 0; i < bytes; i++)
            {
                resRecvBytes[i] = recvBytes[i];
            }
            recvBytes = resRecvBytes;
        }
    }
}
