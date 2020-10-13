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
        public static Thread HeartPcgThread = null;
        public static bool IsMatching = false;
        private static IPEndPoint ipe;
        

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
                if(ReceiveData(ref data))
                {
                    PBConverter.ResData(data);
                }         
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
        public void ReqEndTurn(string uid, int round)
        {
            byte[] data = new byte[1024];
            PBConverter.ReqTurnEnd(uid, round, ref data);
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
        public bool ReceiveData(ref byte[] recvBytes)
        {

            //接受从服务器返回的信息
            int bytes;

            try
            {
                if (socket == null)
                {
                    Socket_Create_Connect();
                }
                else if (!socket.Connected)
                {
                    //if (!IsSocketConnected())
                    //{
                        Reconnect();
                    //}
                    //else
                    //{
                    ReqLogin(UserData.instance.UID, UserData.instance.Password);
                    //}
                }

                bytes = socket.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
                byte[] resRecvBytes = new byte[bytes];

                for (int i = 0; i < bytes; i++)
                {
                    resRecvBytes[i] = recvBytes[i];
                }
                recvBytes = resRecvBytes;
                return true;

            }
            catch
            {
               
            }

            return false;

            //try
            //{
            //    bytes = socket.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
            //    byte[] resRecvBytes = new byte[bytes];

            //    for (int i = 0; i < bytes; i++)
            //    {
            //        resRecvBytes[i] = recvBytes[i];
            //    }
            //    recvBytes = resRecvBytes;
            //    return true;
            //}
            //catch
            //{
            //    if (!IsSocketConnected())
            //    {
            //        Reconnect();       
            //    }

            //    try
            //    {
            //        int port = 8000;
            //        string host = "106.52.156.98";
            //        ///创建终结点EndPoint
            //        IPAddress ip = IPAddress.Parse(host);
            //        ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例
            //        ///创建socket并连接到服务器
            //        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
            //        socket.Connect(ipe);//连接到服务器
            //        Thread.Sleep(1000);
            //    }
            //    catch
            //    {

            //    }

            //    if (socket.Connected)
            //    {
            //        ReqLogin(UserData.instance.UID, UserData.instance.Password);
            //    }
            //    else
            //    {

            //    }



            //    return false;
            //}
        }

        public void ReLogin()
        {
            Reconnect();
            ReqLogin(UserData.instance.UID, UserData.instance.Password);
        }

        public static bool Reconnect()
        {
            //socket.Shutdown(SocketShutdown.Both);
            //socket.Disconnect(true);
            //socket.Close();

            return Socket_Create_Connect();
        }

        public static bool Socket_Create_Connect()
        {
            int port = 8000;
            string host = "106.52.156.98";
            ///创建终结点EndPoint
            IPAddress ip = IPAddress.Parse(host);
            ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndpoint实例
            ///创建socket并连接到服务器
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
            socket.Connect(ipe);//连接到服务器

            return true;
        }

        private bool IsSocketConnected()
        {
            #region remarks
            /********************************************************************************************
             * 当Socket.Conneted为false时， 如果您需要确定连接的当前状态，请进行非阻塞、零字节的 Send 调用。
             * 如果该调用成功返回或引发 WAEWOULDBLOCK 错误代码 (10035)，则该套接字仍然处于连接状态； 
             * 否则，该套接字不再处于连接状态。
             * Depending on http://msdn.microsoft.com/zh-cn/library/system.net.sockets.socket.connected.aspx?cs-save-lang=1&cs-lang=csharp#code-snippet-2
            ********************************************************************************************/
            #endregion

            #region 过程
            // This is how you can determine whether a socket is still connected.
            bool connectState = true;
            bool blockingState = socket.Blocking;
            try
            {
                byte[] tmp = new byte[1];

                socket.Blocking = false;
                socket.Send(tmp, 0, 0);
                //Console.WriteLine("Connected!");
                connectState = true; //若Send错误会跳去执行catch体，而不会执行其try体里其之后的代码
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    //Console.WriteLine("Still Connected, but the Send would block");
                    connectState = true;
                }

                else
                {
                    //Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                    connectState = false;
                }
            }
            finally
            {
                socket.Blocking = blockingState;
            }

            //Console.WriteLine("Connected: {0}", client.Connected);
            return connectState;
            #endregion
        }

    }
}
