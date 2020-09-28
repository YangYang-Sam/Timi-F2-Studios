using System;
using System.Collections.Generic;
using System.Text;

namespace NetTest
{
    class CsResManager
    {
        public static event System.Action<int> LoginResult;
        public static event System.Action<int> RegistResult;
        public static void ResHello(int result)
        {
            Console.WriteLine(result);
        }
        public static void ResLogin(int result)
        {
            Console.WriteLine(result);
            if (LoginResult != null)
            {
                LoginResult(result);
            }
        }
        public static void ResRegist(int result)
        {
            Console.WriteLine(result);
            if (RegistResult != null)
            {
                RegistResult(result);
            }
        }
        public static void NtyLoginOut(String uid, int reason)
        {
            //
            Console.WriteLine(uid);
        }
        public static void ResLoginOut(int result)
        {
            //
            Console.WriteLine(result);
        }
        public static void ResMatching(int result)
        {
            Console.WriteLine(result);
        }
        public static void NtyMatching(int result, String[] userList)
        {
            Console.WriteLine(result);
        }
    }
}
