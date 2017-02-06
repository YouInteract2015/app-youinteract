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

namespace You_Pong
{
    public static class FrameUtils
    {
        private static string gameMode;

        public static string GetMode()
        {
            return gameMode;
        }

        public static void checkifHigh(YouPlugin u, double i)
        {

            gameMode = "none";

            HighScores[] high = new HighScores[5];
            bool ishigh = false;
            int count = 0;

            var xml = XDocument.Load("Highscores.xml");
            var q = (from c in xml.Root.Descendants("Entry")
                     select c.Element("id").Value + "/" +
                            c.Element("value").Value).ToArray();
            foreach (var v in q)
            {
                var s = v.Split('/');
                high[count] = new HighScores(int.Parse(s[0]), Convert.ToDouble(s[1]));
                count++;
            }

            foreach (var v in high)
            {
                Console.WriteLine(v.ToString());
            }

            high = high.OrderBy(x => x.pos).Reverse().ToArray();

            foreach (var h in high)
            {
                Console.WriteLine(h.ToString());
            }
            var a = (from h in high
                     where h.value > i
                     select h).OrderBy(x => x.pos).ToArray();

            int n = 6 - a.Length;



            if (n != 6)
            {

                for (int x = a.Length - 1; x > 0; x--)
                {
                    a[x] = a[x - 1];
                    a[x].pos = a[x].pos + 1;
                }

                a[0] = new HighScores(n, i);

                a = a.Reverse().ToArray();
                for (int x = 0; x < a.Length; x++)
                {

                    high[x] = a[x];

                }

                ishigh = true;
                using (XmlWriter w = XmlWriter.Create("Highscores.xml"))
                {
                    w.WriteStartDocument();
                    w.WriteStartElement("HighScores");
                    foreach (var v in high)
                    {
                        w.WriteStartElement("Entry");

                        w.WriteElementString("id", v.pos.ToString());
                        w.WriteElementString("value", v.value.ToString());
                        w.WriteEndElement();
                    }
                    w.WriteEndElement();
                    w.WriteEndDocument();
                }
                count = 4;
                bool first = true;
                while (count >= n)
                {

                    if (first)
                    {
                        File.Replace("Pong4.png", "Pong5.png", "pongas");
                        first = false;
                    }
                    else
                    {
                        File.Copy("Pong" + count + ".png", "Pong" + (count + 1) + ".png");
                        File.Delete("Pong" + count + ".png");
                    }
                    count--;


                }
            }
            if (ishigh)
            {
                KinectApi.setColor(true);
                NewHigh(n);
                YouNavigation.requestFrameChange(u, "YouPongPhoto");
            }
            else
                YouNavigation.requestFrameChange(u, "YouPong");

        }

        public static double getHighscore(int pos)
        {
            var xml = XDocument.Load("Highscores.xml");
            if (xml.Root == null) return 0;
            var q = (from c in xml.Root.Descendants("Entry") let xElement = c.Element("value") where xElement != null select double.Parse(xElement.Value, CultureInfo.InvariantCulture)).ToArray();

            foreach (var d in q)
            {
                Console.WriteLine(d);
            }
            return (q[5 - pos]);
        }

        public static void SetMode(string mode)
        {
            gameMode = mode;
        }

        public static void requestRestart(string mode)
        {
            switch (gameMode)
            {
                case "1p":
                    Restart1P();
                    break;
                case "2p":
                    Restart2P();
                    break;
            }
        }

        public static void requestResume(string mode, Player p1, double y1, Player p2, double y2, Ellipse ibola, Ball b)
        {
            switch (gameMode)
            {
                case "1p":
                    Resume1P(p1, y1, p2, y2, ibola, b);
                    break;
                case "2p":
                    Resume2P(p1, y1, p2, y2, ibola, b);
                    break;
            }
        }

        public static void requestPause(Player p1, double y1, Player p2, double y2, Ellipse ibola, Ball b)
        {
            Pause(p1, y1, p2, y2, ibola, b);
        }

        public static event RestartEvent1P Restart1P = delegate { };
        public static event RestartEvent2P Restart2P = delegate { };
        public static event ResumeEvent1P Resume1P = delegate { };
        public static event ResumeEvent2P Resume2P = delegate { };
        public static event PauseEvent Pause = delegate { };
        public static event NewHighScore NewHigh = delegate { };

        public delegate void NewHighScore(int t);
        public delegate void RestartEvent1P();
        public delegate void RestartEvent2P();
        public delegate void ResumeEvent2P(Player p1, double y1, Player p2, double y2, Ellipse ibola, Ball b);
        public delegate void ResumeEvent1P(Player p1, double y1, Player p2, double y2, Ellipse ibola, Ball b);
        public delegate void PauseEvent(Player p1, double y1, Player p2, double y2, Ellipse ibola, Ball b);
    }
}
