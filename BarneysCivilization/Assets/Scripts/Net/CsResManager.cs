using System;
using System.Collections.Generic;
using System.Text;

namespace NetTest
{
    class CsResManager
    {
        public static void ResHello(int result)
        {
            Console.WriteLine(result);
        }
        public static void ResLogin(int result)
        {
            Console.WriteLine(result);
        }
        public static void ResRegist(int result)
        {
            Console.WriteLine(result);
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
        public static void ResCreateRoom(int[] result)
        {
            //
            Console.WriteLine(result);
        }
        public static void ResQuitRoom(int result)
        {
            //
            Console.WriteLine(result);
        }
    }
}
