using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using YouInteract.YouBasic;
using YouInteract.YouPlugin_Developing;

namespace You_TicTacToe
{
    public static class FrameUtils
    {
        private static string gameMode;

        public static string GetMode()
        {
            return gameMode;
        }

        

        public static void SetMode(string mode)
        {
            gameMode = mode;
        }

        public static void requestRestart(string mode)
        {
            Console.WriteLine("Request Restart Chamado!");
            switch (mode)
            {
                case "2s":
                    Console.WriteLine("Modo restart 2s ativado!");
                    Restart2S();
                    break;
                case "2p":
                    Restart2P();
                    break;
            }
        }
        public static void RequestWaitScreen(int numerojogador,string hostname,string username,string password, int port) {
            WaitScreen(numerojogador,hostname,username,password, port);
        }

        public static void RequestStartGame(int numerojogador, string hostname, string username, string password, int port)
        {
            Start2S(numerojogador, hostname, username, password, port);
        }

        
        public static event RestartEvent2S Restart2S = delegate { };
        public static event RestartEvent2P Restart2P = delegate { };
        public static event WaitScreen2S WaitScreen = delegate { };
        public static event StartGame2S Start2S = delegate { };

        public delegate void RestartEvent2S();
        public delegate void RestartEvent2P();
        public delegate void WaitScreen2S(int numerojogador, string hostname,string username, string password, int port);
        public delegate void StartGame2S(int numerojogador, string hostname, string username, string password, int port);
        
    }
}
