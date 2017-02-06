using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;

namespace You_Pong
{
    /// <summary>
    /// Interaction logic for Pong2Players.xaml
    /// </summary>
    public partial class Pong2Players : Page, YouPlugin
    {
        public static Pong2Players p2p;
        private int IDpad1, IDpad2, move;
        private DispatcherTimer dispatcherTimer;
        private DispatcherTimer t;
        int score1, score2;
        private Player[] jogadores = new Player[2];
        private Player P1, P2;
        private Ball ball;
        private double h, w;
        private bool gamestarted;
        private bool playwinner = true;
        private bool gameended = false;
        private bool locked, halfpause;
        private int pausecount;
        private bool pause;
        private bool resume = false;
        int res;

        //constructor
        public Pong2Players()
        {
            InitializeComponent();
            p2p = this;

            locked = false;
            pause = false;
            halfpause = false;

            h = YouWindow.getHeight();
            w = YouWindow.getWidth();
            IDpad1 = -1;
            IDpad2 = -1;
            jogadores[0] = new Player();
            jogadores[1] = new Player();
            P1 = new Player();
            P2 = new Player();
            ball = new Ball(w * 0.5, h * 0.5, h * 0.04);

            move = 0;
            Bola.Width = h * 0.05;
            Bola.Height = h * 0.05;

            Pad1.Width = w * 0.03;
            Pad1.Height = P1.yactual = P1.yimage = h * 0.2;
            Pad2.Width = w * 0.03;
            Pad2.Height = P2.yactual = P2.yimage = h * 0.2;


            gamestarted = false;
            SetUp();
            Loaded += Pong2Players_Loaded;
            Unloaded += Pong2Players_Unloaded;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            t = new DispatcherTimer();
            t.Interval = new TimeSpan(0, 0, 0, 1);
            t.Tick += Tick_Resume;
            FrameUtils.Restart2P += FrameUtils_Restart;
            FrameUtils.Resume2P += FrameUtils_Resume;
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            setTutorial();
        }

        void Pong2Players_Unloaded(object sender, RoutedEventArgs e)
        {
            KinectApi.InteractionEvent -= KinectApi_InteractionEvent;

        }
        private void setTutorial()
        {
            Tutorial.Visibility = Visibility.Visible;
            Tutorial.Height = h * 0.7;
            Tutorial.Width = w;
            foreach (Viewbox v in Tutorial.Children)
            {
                v.Height = h * 0.1;
                v.Width = w * 0.8;
                if (v.Name == "t1")
                {
                    v.Height = h * 0.15;
                }
                
            }
            Canvas.SetTop(Tutorial, h * 0.15);
            Canvas.SetLeft(Tutorial, w * 0.05);

        }

        private void hideTutorial()
        {
            Tutorial.Visibility = Visibility.Collapsed;
        }

        void Pong2Players_Loaded(object sender, RoutedEventArgs e)
        {
            FrameUtils.SetMode("2p");
            KinectApi.InteractionEvent += KinectApi_InteractionEvent;
            

        }

        private void FrameUtils_Resume(Player p1, double y1, Player p2, double y2, Ellipse ibola, Ball b)
        {
            P1 = p1;
            Canvas.SetTop(Pad1, y1);
            P2 = p2;
            Canvas.SetTop(Pad2, y2);

            Bola = ibola;
            ball = b;
            gamestarted = false;
            if (res <= 0)
            {
                resume = true;
                res = 3;
                t.Start();
            }
        }
        private void FrameUtils_Restart()
        {
            setTutorial();
            score1 = score2 = 0;
            score.Content = score1 + "         " + score2;
            gamestarted = false;
            pause = false;
            SetUp();
            dispatcherTimer.Start();
        }
        //Pong Logic
        public void SetUp()
        {
            P1.yactual = P1.yimage = h / 2 - Pad2.Height / 2;
            P2.yactual = P2.yimage = h / 2 - Pad2.Height / 2;
            ball.x = w * 0.5;
            ball.y = h * 0.5;
            Canvas.SetTop(Bola, ((h - Bola.Height) / 2));
            Canvas.SetLeft(Bola, ((w - Bola.Width) / 2));
            Canvas.SetTop(Pad2, h / 2 - Pad2.Height / 2);
            Canvas.SetRight(Pad2, w * 0.05);
            Canvas.SetLeft(Pad1, w * 0.05);
            Canvas.SetTop(Pad1, h / 2 - Pad1.Height / 2);
            halfpause = false;
            
        }//y
        private void StartGame()
        {
            Random r = new Random();
            hideTutorial();
            int rand = 0;
            while (rand == 0)
            {
                rand = r.Next(-1, 2);
            }
            if (playwinner)
            {
                ball.speedx = 0.01;
                ball.speedy = (0.01 * rand);
            }
            else
            {
                ball.speedx = -0.01;
                ball.speedy = 0.01 * rand;
            }
            gamestarted = true;
            winner.Content = "";
            if (gameended == true)
            {
                score.Content = score1.ToString() + "       " + score2.ToString();
                gameended = false;
            }
            dispatcherTimer.Start();

        }
        private void DetectCollision()
        {
            // Walls DONE
            if (ball.x <= ball.r)
            {
                playwinner = false;
                score2++;
                score.Content = score1 + "           " + score2;
                gamestarted = false;
                SetUp();
                if (score2 == 5)
                {
                    GameEnded();
                }
            }
            if (ball.x >= w - ball.r)
            {
                playwinner = true;
                score1++;
                score.Content = score1 + "            " + score2;
                gamestarted = false;
                SetUp();
                if (score1 == 5)
                {
                    GameEnded();
                }
            }
            if (ball.y < ball.r / 4)
            {
                calculateNewAngleY();
            }
            if (ball.y > h - ball.r * 5 / 4)
            {
                calculateNewAngleY();
            }
            bool nbateu = true;

            // Pads TODO
            // Bola próxima do Pad1
            if (ball.x + ball.speedx * 0.8 * w <= (w * 0.05 + Pad1.Width) && ball.x + ball.speedx * w >= w * 0.05)
            {
                double nexty = ball.y + ball.speedy * h + ball.r;

                if ((nexty + 3 / 2 * ball.r >= P1.yimage) && (nexty <= (P1.yimage + Pad1.Height * 1 / 3)))
                {
                    nbateu = false;
                    if (ball.speedy > 0)
                    {
                        calculateNewAngleX();
                        calculateNewAngleY();
                    }
                    else
                    {
                        calculateNewAngleX();
                    }
                    if (ball.speedx < 0.25)
                        ball.speedx += 0.001;
                }
                else if ((nexty + 3 / 2 * ball.r >= P1.yimage) && (nexty <= (P1.yimage + (Pad1.Height - 2 / 3 * Pad1.Height))))
                {
                    nbateu = false;
                    if (ball.speedy < 0)
                    {
                        calculateNewAngleX();
                        calculateNewAngleY();
                    }
                    else
                    {
                        calculateNewAngleX();
                    }
                    if (ball.speedx < 0.025)
                        ball.speedx += 0.001;
                }
            }
            // Bola próxima do Pad2
            else if ((ball.x + ball.r * 2 + ball.speedx * w >= (w - w * 0.03 - Pad2.Width)) && ball.x + ball.r * 2 + ball.speedx <= w - w * 0.05)
            {
                double nexty = ball.y + ball.speedy * h;
                if ((nexty + 3 / 2 * ball.r >= P2.yimage) && (nexty <= (P2.yimage + Pad2.Height * 1 / 3)))
                {
                    nbateu = false;
                    if (ball.speedy > 0)
                    {
                        calculateNewAngleX();
                        calculateNewAngleY(-0.003);
                    }
                    else
                    {
                        calculateNewAngleX();
                        ball.speedy -= 0.003;
                    }
                    if (ball.speedx > -0.025)
                        ball.speedx -= 0.001;
                }
                else if ((nexty + 3 / 2 * ball.r >= P2.yimage) && (nexty <= (P2.yimage + (Pad2.Height - 2 / 3 * Pad2.Height))))
                {
                    nbateu = false;
                    if (ball.speedy < 0)
                    {
                        calculateNewAngleX();
                        calculateNewAngleY(0.003);
                    }
                    else
                    {
                        calculateNewAngleX();
                        ball.speedy += 0.003;
                    }
                    if (ball.speedx > -0.025)
                        ball.speedx -= 0.001;
                }
            }

            if (nbateu)
            {
                ball.y = ball.y + (ball.speedy * h);
                ball.x = ball.x + (ball.speedx * w);
            }
        }//y
        private void GameEnded()
        {
            winner.Content = score1 > score2 ? "PLAYER 1 WINS" : "PLAYER 2 WINS";
            score1 = 0;
            score2 = 0;
            gameended = true;
        }//Y
        private void Pause()
        {
            if (halfpause)
            {
                if (!gamestarted)
                {
                    pause = false;
                    pausecount = 0;
                    Pad1.Fill = Brushes.White;
                    Pad2.Fill = Brushes.White;
                    P1.closed = false;
                    P2.closed = false;
                    halfpause = false;
                    StartGame();
                }
                else
                {
                    P1.closed = false;
                    P2.closed = false;
                    halfpause = false;
                    pausecount = 0;
                    Pad1.Fill = Brushes.White;
                    Pad2.Fill = Brushes.White;
                    gamestarted = false;
                    pause = true;
                    dispatcherTimer.Stop();
                    FrameUtils.requestPause(P1, P1.yimage, P2, P2.yimage, Bola, ball);
                    YouNavigation.requestFrameChange(this, "YouPongPause");
                }
            }
            else
            {
                halfpause = true;
                pausecount = 60;
            }
        }

        private void calculateNewAngleY(double variation = 0)
        {
            ball.speedy *= -1 + variation;
        }
        private void calculateNewAngleX()
        {
            ball.speedx *= -1;
        }
        //Timers
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (gamestarted)
            {
                if (move == 1)
                {
                    HandChanged();
                    move = -1;
                }
                move++;
                DetectCollision();

                Canvas.SetLeft(Bola, ball.x);
                Canvas.SetTop(Bola, ball.y);
            }
            if (pausecount == 0)
            {
                P1.closed = false;
                P2.closed = false;
                halfpause = false;
                Pad2.Fill = Brushes.White;
                Pad1.Fill = Brushes.White;
            }
            if (pausecount > 0)
                pausecount--;
           /* if (IDpad1 == -1 && IDpad2 == -1)
            {
                Maos.Content = "No Bones :(";
            }
            else if ((IDpad1 == -1) ^ (IDpad2 == -1))
            {
                Maos.Content = "1 Skeleton";
            }
            else
            {
                Maos.Content = "Ready to Play!";
            }*/

        }
        private void Tick_Resume(object sender, EventArgs e)
        {
            Countdown.Content = res.ToString();
            if (res <= 0)
            {
                resume = false;
                t.Stop();
                Countdown.Content = "";
                gamestarted = true;
                pause = false;
                dispatcherTimer.Start();
            }
            res--;
        }
        //Kinect  
        private void HandChanged()
        {
            if (Math.Abs(P1.yactual - P1.yimage) > h * 0.05)
            {
                if (P1.yactual > P1.yimage)
                {
                    P1.yimage = P1.yimage + h * 0.05;
                }
                else
                {
                    P1.yimage = P1.yimage - h * 0.05;
                }
            }
            if (Math.Abs(P1.yactual - P1.yimage) < h * 0.05)
            {
                P1.yimage = P1.yactual;
            }
            if (Math.Abs(P2.yactual - P2.yimage) > h * 0.05)
            {
                if (P2.yactual > P2.yimage)
                {
                    P2.yimage = P2.yimage + h * 0.05;
                }
                else
                {
                    P2.yimage = P2.yimage - h * 0.05;
                }
            }
            if (Math.Abs(P2.yactual - P2.yimage) < h * 0.05)
            {
                P2.yimage = P2.yactual;
            }

            if (P1.yimage >= (h - Pad1.Height))
            {
                P1.yimage = h - Pad1.Height;
            }
            else if (P1.yimage <= 0)
            {
                P1.yimage = 0;
            }
            if (P2.yimage >= (h - Pad2.Height))
            {
                P2.yimage = h - Pad2.Height;
            }
            else if (P2.yimage <= 0)
            {
                P2.yimage = 0;
            }

            Canvas.SetTop(Pad1, P1.yimage);
            Canvas.SetTop(Pad2, P2.yimage);
        }
        private void KinectApi_InteractionEvent(InteractionStreamArgs e)
        {

            Console.WriteLine("Interaction Stream - pong2P");

            if (FrameUtils.GetMode() != "2p" || pause) return;

            IEnumerable<UserInfo> activeusers = (from u in e.userinfo
                                                 where u.SkeletonTrackingId > 0
                                                 orderby u.SkeletonTrackingId
                                                 select u);

            UserInfo[] aux = activeusers.ToArray();
            Maos.Content = aux.Count();

            bool L = false, R = false;

            foreach (var v in aux)
            {

                if (v.SkeletonTrackingId == IDpad1)
                {
                    L = true;
                }
                if (v.SkeletonTrackingId == IDpad2)
                {
                    R = true;
                }

            }

            int i = 0;


            //if (s == ((IDpad1.ToString() + IDpad2.ToString()) || (s == (IDpad2.ToString() + IDpad1.ToString())))) 
            if (L == false || R == false)
            {

                locked = false;
                IDpad1 = -1;
                IDpad2 = -1;


            }
            if (!locked)
            {
                if (!Empty(activeusers))
                {
                    foreach (var u in aux)
                    {
                        if (u.HandPointers.Count > 0)
                        {
                            foreach (var hand in u.HandPointers)
                            {

                                if (i < 2)
                                {
                                    if (hand.IsPrimaryForUser)
                                    {

                                        jogadores[i].ID = u.SkeletonTrackingId;
                                        jogadores[i].x = hand.X * w;
                                        jogadores[i].yactual = hand.Y * h;
                                        i++;
                                        if (i == 2)
                                            locked = true;
                                    }
                                }
                            }
                        }
                    }
                }
                // score.Content = jogadores[0].ID.ToString() + " /.....?" + jogadores[1].ID.ToString();
                if (jogadores[0].x > jogadores[1].x)
                {
                    IDpad1 = jogadores[0].ID;
                    P1 = jogadores[0];
                    IDpad2 = jogadores[1].ID;
                    P2 = jogadores[1];

                }
                else
                {
                    IDpad1 = jogadores[1].ID;
                    IDpad2 = jogadores[0].ID;
                    P1 = jogadores[1];
                    P2 = jogadores[0];
                }

            }
            else
            {
                UserInfo[] aux1 = new UserInfo[2];
                int j = 0;
                foreach (var v in aux)
                {
                    if (v.SkeletonTrackingId == IDpad1 || v.SkeletonTrackingId == IDpad2)
                    {
                        aux1[j] = v;
                        j++;
                    }

                }
                foreach (var p in aux1)
                {
                    foreach (var h in p.HandPointers)
                    {
                        if (h.IsPrimaryForUser && p.SkeletonTrackingId == IDpad1)
                        {
                            P1.yactual = h.Y * this.h;
                            if (!resume)
                                if (h.HandEventType == InteractionHandEventType.Grip)
                                {
                                    Pad1.Fill = Brushes.Red;

                                    if (!P1.closed)
                                        Pause();

                                    P1.closed = true;

                                }

                        }
                        if (h.IsPrimaryForUser && p.SkeletonTrackingId == IDpad2)
                        {
                            P2.yactual = h.Y * this.h;
                            if (!resume)
                                if (h.HandEventType == InteractionHandEventType.Grip)
                                {
                                    Pad2.Fill = Brushes.Blue;
                                    if (!P2.closed)
                                        Pause();

                                    P2.closed = true;
                                }

                        }
                    }

                }

            }
        }
        private bool Empty(IEnumerable<UserInfo> p)
        {
            return p == null || !p.Any();
        }

        //YouPlugin
        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_Pong";
        }
        // To identify the main page of the Plugin
        public bool getIsFirstPage()
        {
            return false;
        }
        // To identify which Kinect Requirements need to be active
        // Kinect Region; Skeleton Stream; Interaction Stream
        public KinectRequirements getKinectRequirements()
        {
            return new KinectRequirements(false, true, true);
        }
        // To identify the page name
        public string getName()
        {
            return this.Name;
        }
        // This Page
        public Page getPage()
        {
            return this;
        }
        // To identigy the kinect Region
        // Return your Kinect Region if it is active
        // else return Null
        public KinectRegion getRegion()
        {
            return null;
        }

        #endregion



    }


}
