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
                        HeartPcgThread.Start(UserData.instance.UID);
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

                    int[] raceList = new int[resMsg6.Racelist.Count];
                    for (int i = 0; i < resMsg6.Racelist.Count; i++)
                    {
                        raceList[i] = resMsg6.Racelist[i];
                    }

                    int[] RandomSeeds = new int[resMsg6.Randomlist.Count];
                    for (int i = 0; i < resMsg6.Randomlist.Count; i++)
                    {
                        RandomSeeds[i] = resMsg6.Randomlist[i];
                    }

                    int res6 = (int)resMsg6.Result;
                    CsResManager.NtyMatching(res6, userList, raceList, RandomSeeds);
                    break;
                case MSGID.CsResStopMatchingId:
                    CS_RES_STOP_MATCHING resMsg7 = msg.CsResStopMatching;
                    int res7 = (int)resMsg7.Result;
                    CsResManager.ResStopMatching(res7);
                    break;
                case MSGID.CsNtyEndRoundId:
                    CS_NTY_END_ROUND resMsg8 = msg.CsNtyEndRound;
                    UserData ud = UserData.instance;
                    ud.AllPoses = new int[resMsg8.Posid.Count];
                    for (int i = 0; i < ud.AllPoses.Length; i++)
                    {
                        ud.AllPoses[i] = resMsg8.Posid[i] - 1;
                    }
                    ud.RandomSeeds = new int[resMsg8.Randomlist.Count];
                    for (int i = 0; i < resMsg8.Randomlist.Count; i++)
                    {
                        ud.RandomSeeds[i] = resMsg8.Randomlist[i];
                    }
                    CsResManager.NtyRoundEnd();
                    break;
                case MSGID.CsNtyPlayerUseCardId:
                    CS_NTY_PLAYER_USE_CARD resMsg9 = msg.CsNtyPlayerUseCard;
                    int cardID = resMsg9.Cardid - 1;
                    string uid = resMsg9.Userid;
                    int hexID = resMsg9.Pos - 1;
                    CsResManager.NtyUseCard(uid, cardID, hexID);
                    break;
                default:
                    break;
            }
        }

        // 心跳包（每十秒向服务器发送一次请求）
        public static void HeartPcgThreadFunc(object userId)
        {
            String uid = userId as String;

            while (true)
            {
                Thread.Sleep(10000);
                byte[] data = new byte[1024];
                ReqHello(uid, ref data);
                NetManager.socket.Send(data, data.Length, 0);                
            }
        }

        //心跳包
        public static void ReqHello(String uid, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqHelloId;

            CS_REQ_HELLO reqMsg = new CS_REQ_HELLO();
            reqMsg.Id = uid;

            msg.CsReqHello = reqMsg;

            data = PBConverter.Serialize(msg);
        }

        //请求注册
        public static void ReqRegister(String uid, String password, ref byte[] data)
        {

            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqRegistId;

            CS_REQ_REGIST reqMsg = new CS_REQ_REGIST();
            reqMsg.Id = uid;
            reqMsg.Passwd = password;

            msg.CsReqRegist = reqMsg;

            data = PBConverter.Serialize(msg);
        }

        // 请求匹配
        public static void ReqMatching(String uid,int raceIndex, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqMatchingId;
            CS_REQ_MATCHING reqMsg = new CS_REQ_MATCHING();
            reqMsg.Userid = uid;
            reqMsg.Race = raceIndex;
            msg.CsReqMatching = reqMsg;
            data = PBConverter.Serialize(msg);
        }

        //请求停止匹配
        public static void ReqStopMatching(String uid, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqStopMatchingId;
            CS_REQ_STOP_MATCHING reqMsg = new CS_REQ_STOP_MATCHING();
            reqMsg.Userid = uid;
            msg.CsReqStopMatching = reqMsg;
            data = PBConverter.Serialize(msg);
        }

        // 请求登录
        public static void ReqLogin(String uid, String password, ref byte[] data)
        {

            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqLoginId;

            CS_REQ_LOGIN reqMsg = new CS_REQ_LOGIN();
            reqMsg.Id = uid;
            reqMsg.Passwd = password;

            msg.CsReqLogin = reqMsg;

            data = PBConverter.Serialize(msg);
        }

        //请求退出登录
        public static void ReqLoginOut(String uid, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqLoginoutId;

            CS_REQ_LOGINOUT reqMsg = new CS_REQ_LOGINOUT();
            reqMsg.Id = uid;

            msg.CsReqLoginout = reqMsg;

            data = PBConverter.Serialize(msg);
        }

        // 发送移动指令
        public static void ReqSetDestiny(String uid, int index, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqPlayerMovId;

            CS_REQ_PLAYER_MOV reqMsg = new CS_REQ_PLAYER_MOV();
            reqMsg.Posid = index;
            reqMsg.Userid = uid;

            msg.CsReqPlayerMov = reqMsg;

            data = PBConverter.Serialize(msg);
        }

        // 发送用卡指令
        public static void ReqUseCard(String uid, int CardID, int HexID, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqPlayerUseCardId;

            CS_REQ_PLAYER_USE_CARD reqMsg = new CS_REQ_PLAYER_USE_CARD();
            reqMsg.Cardid = CardID + 1;
            reqMsg.Userid = uid;
            reqMsg.Pos = HexID + 1;

            msg.CsReqPlayerUseCard = reqMsg;

            data = PBConverter.Serialize(msg);
        }

        // 发送回合结束指令
        public static void ReqTurnEnd(String uid, ref byte[] data)
        {
            CS_REQ_MSG msg = new CS_REQ_MSG();
            msg.Msgid = MSGID.CsReqChgPlayerStatusId;

            CS_REQ_CHG_PLAYER_STATUS reqMsg = new CS_REQ_CHG_PLAYER_STATUS();
            reqMsg.Userid = uid;

            msg.CsReqChgPlayerStatus = reqMsg;

            data = PBConverter.Serialize(msg);
        }
    }
}
