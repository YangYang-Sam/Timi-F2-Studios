using System;
using System.Collections.Generic;
using System.Text;

namespace NetTest
{
    class CsResManager
    {
        public static event System.Action<int> LoginResult;
        public static event System.Action<int> RegistResult;
        public static event System.Action MatchingBeginEvent;
        public static event System.Action<string[], int[], int[]> MatchSuccessEvent;
        public static event System.Action RoundEndEvent;
        public static event System.Action<string, int, int> UseCardEvent;
        public static void ResHello(int result)
        {
            Console.WriteLine("Hello:" + result);
        }
        public static void ResLogin(int result)
        {
            Console.WriteLine("Login:" + result);
            if (LoginResult != null)
            {
                LoginResult(result);
            }
        }
        public static void ResRegist(int result)
        {
            Console.WriteLine("Register:" + result);
            if (RegistResult != null)
            {
                RegistResult(result);
            }
        }
        public static void NtyLoginOut(String uid, int reason)
        {
            //
            Console.WriteLine("LoginOut:" + uid);
        }
        public static void ResLoginOut(int result)
        {
            //
            Console.WriteLine("LoginOut:" + result);
        }
        public static void ResMatching(int result)
        {
            Console.WriteLine("Matching:" + result);
            if (RegistResult != null)
            {
                MatchingBeginEvent();
            }
        }
        public static void NtyMatching(int result, String[] userList, int[] raceList, int[] RandomList)
        {
            Console.WriteLine("NtyMatching:" + result);
            int len = userList.Length;
            for (int i = 0; i < len; i++)
            {
                Console.WriteLine("NtyMatchingUser " + i + ":" + userList[i]);
            }
            if (MatchSuccessEvent != null)
            {
                MatchSuccessEvent(userList,raceList,RandomList);
            }            
        }
        public static void ResStopMatching(int result)
        {
            Console.WriteLine("StopMatching:" + result);
        }
        public static void NtyRoundEnd()
        {
            if (RoundEndEvent != null)
            {
                RoundEndEvent();
            }    
        }
        public static void NtyUseCard(string uid, int cardID,int hexID)
        {
            if (UseCardEvent != null)
            {
                UseCardEvent(uid, cardID, hexID);
            }
        }
    }
}
