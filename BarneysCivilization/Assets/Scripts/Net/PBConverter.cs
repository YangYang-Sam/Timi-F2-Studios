using System;
using System.Collections.Generic;
using System.Text;
using Csmsg;            //protobuf中的包名
using Google.Protobuf; //谷歌对C#提供的语言接口
using System.Threading;


public struct NtyLoginOut
{
    public String uid;
    public int reason;
}

namespace NetTest
{
    class PBConverter
    {
        // 序列化
        public static byte[] Serialize<T>(T obj) where T : IMessage
        {
            byte[] data = obj.ToByteArray();
            return data;
        }

        // 反序列化
        public static T Deserialize<T>(byte[] data) where T : class, IMessage, new()
        {
            T obj = new T();
            IMessage message = obj.Descriptor.Parser.ParseFrom(data);
            return message as T;
        }

        //接收
        public static void ResData(byte[] data)
        {
            CS_RES_MSG msg = PBConverter.Deserialize<CS_RES_MSG>(data);
            switch (msg.Msgid)
            {
                case MSGID.CsResHelloId:
                    CS_RES_HELLO resMsg0 = msg.CsResHello;
                    int res0 = (int)resMsg0.Timenow;
                    CsResManager.ResHello(res0);
                    break;
                case MSGID.CsResRegistId:
                    CS_RES_REGIST resMsg1 = msg.CsResRegist;
                    int res1 = (int)resMsg1.Result;
                    CsResManager.ResRegist(res1);
                    break;
                case MSGID.CsResLoginId:
                    CS_RES_LOGIN resMsg2 = msg.CsResLogin;
                    int res2 = (int)resMsg2.Result;
                    if (res2 == 0)
                    {
                        //开一个子线程，用于发送心跳包
                        Thread HeartPcgThread = new Thread(new ParameterizedThreadStart(HeartPcgThreadFunc));
                        HeartPcgThread.IsBackground = true;
                        HeartPcgThread.Start(NetManager.USERID);
                    }
                    CsResManager.ResLogin(res2);
                    break;
                case MSGID.CsResLoginoutId:
                    CS_RES_LOGINOUT resMsg3 = msg.CsResLoginout;
                    int res3 = (int)resMsg3.Result;
                    CsResManager.ResLoginOut(res3);
                    break;
                case MSGID.CsNtyLoginoutId:
                    CS_NTY_LOGINOUT resMsg4 = msg.CsNtyLoginout;
                    String uid4 = resMsg4.Id;
                    int reason4 = (int)resMsg4.Reason;
                    CsResManager.NtyLoginOut(uid4, reason4);
                    break;
                case MSGID.CsResMatchingId:
                    CS_RES_MATCHING resMsg5 = msg.CsResMatching;
                    int res5 = (int)resMsg5.Result;
                    CsResManager.ResMatching(res5);
                    break;
                case MSGID.CsNtyMatchingId:
                    CS_NTY_MATCHING resMsg6 = msg.CsNtyMatching;
                    String[] userList = new String[resMsg6.Userlist.Count];
                    for (int i = 0; i < resMsg6.Userlist.Count; i++)
                    {
                        userList[i] = resMsg6.Userlist[i].ToString();
                    }
                    int res6 = (int)resMsg6.Result;
                    CsResManager.NtyMatching(res6, userList);
                    break;
                default:
                    break;
            }
        }

        // 心跳包
        public static void HeartPcgThreadFunc(object userId)
        {
            String uid = userId as String;

            while (true)
            {
                byte[] data = new byte[1024];
                ReqHello(uid, ref data);
                NetManager.socket.Send(data, data.Length, 0);
                Thread.Sleep(10000);
            }
        }

        //心跳包
        public static byte[] ReqHello(String uid, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqHelloId;

            CS_REQ_HELLO reqMsg = new CS_REQ_HELLO();
            reqMsg.Id = uid;

            msg.CsReqHello = reqMsg;

            data = PBConverter.Serialize(msg);
            return data;
        }

        //请求注册
        public static byte[] ReqRegister(String uid, String password)
        {

            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqRegistId;

            CS_REQ_REGIST reqMsg = new CS_REQ_REGIST();
            reqMsg.Id = uid;
            reqMsg.Passwd = password;

            msg.CsReqRegist = reqMsg;

            byte[] data = PBConverter.Serialize(msg);
            return data;
        }

        // 请求匹配
        public static byte[] ReqMatching(String uid)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqMatchingId;
            CS_REQ_MATCHING reqMsg = new CS_REQ_MATCHING();
            reqMsg.Userid = uid;
            msg.CsReqMatching = reqMsg;
            byte[] data = PBConverter.Serialize(msg);
            return data;
        }

        //返回注册
        public static int ResRegister(byte[] data)
        {

            CS_RES_MSG msg = PBConverter.Deserialize<CS_RES_MSG>(data);

            CS_RES_REGIST resMsg = msg.CsResRegist;

            int res = (int)resMsg.Result;

            return res;
        }

        // 请求登录
        public static byte[] ReqLogin(String uid, String password)
        {

            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqLoginId;

            CS_REQ_LOGIN reqMsg = new CS_REQ_LOGIN();
            reqMsg.Id = uid;
            reqMsg.Passwd = password;

            msg.CsReqLogin = reqMsg;

            byte[] data = PBConverter.Serialize(msg);
            return data;
        }

        //返回登录
        public static int ResLogin(byte[] data)
        {

            CS_RES_MSG msg = PBConverter.Deserialize<CS_RES_MSG>(data);

            CS_RES_LOGIN resMsg = msg.CsResLogin;
            int res = (int)resMsg.Result;
            return res;
        }

        public static bool ResNtyLoginOut(byte[] data, ref NtyLoginOut nlo)
        {
            CS_RES_MSG msg = PBConverter.Deserialize<CS_RES_MSG>(data);
            if (msg.Msgid == MSGID.CsNtyLoginoutId)
            {
                CS_NTY_LOGINOUT resMsg = msg.CsNtyLoginout;
                nlo.uid = resMsg.Id;
                nlo.reason = (int)resMsg.Reason;
                return true;
            }
            return false;
        }

        //请求退出登录
        public static byte[] ReqLoginOut(String uid)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqLoginoutId;

            CS_REQ_LOGINOUT reqMsg = new CS_REQ_LOGINOUT();
            reqMsg.Id = uid;

            msg.CsReqLoginout = reqMsg;

            byte[] data = PBConverter.Serialize(msg);
            return data;
        }

        public static int ResLoginOut(byte[] data)
        {
            CS_RES_MSG msg = PBConverter.Deserialize<CS_RES_MSG>(data);

            CS_RES_LOGINOUT resMsg = msg.CsResLoginout;
            int res = (int)resMsg.Result;
            return res;
        }

        struct RoomInfo
        {
            int roomId;
            string[] userList;
        }
    }
}
