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
using System.IO;
using System.Collections;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;
using RabbitMQ.Client;

namespace You_Pong
{
    /// <summary>
    /// Interaction logic for Pong1Player.xaml
    /// </summary>

    public partial class Pong1Player : Page, YouPlugin
    {
        public static Pong1Player p1p;
        private int IDpad1, IDpad2, move;
        private DispatcherTimer dispatcherTimer;    // temporizador ~ 33 fps
        private DispatcherTimer t;                  // temporizador para apos pausa e startgame
        int score1, score2;
        private Player[] jogadores = new Player[2];
        private Player P1, P2;
        private Ball ball;          // bola
        private double h, w;        // altura e largura da janela
        private bool gamestarted;   //flag que define se o jogo comecou ou nao
        private bool playwinner = false;    //flag que define o vencedor da ronda para decidir a direcao da bola na ronda seguinte
        private int pausecount;
        private bool pause; //flag que indica se o jogo esta em pause ou nao
        private bool gameended = false; //flag que indica se o jogo terminou ou nao
        private double highscore = 0;
        private bool resume = false;
        int res;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private Queue<string> queue;    //queue auxiliar para guardar coordenadas de um ficheiro de texto


        //constructor
        public Pong1Player()
        {
            
            InitializeComponent();
            p1p = this; // This is just stupid ?
            pause = false; // Global variable for pause

            //h = YouWindow.getHeight(); // Window height
            //w = YouWindow.getWidth(); // Window weight
            h = 772;
            w = 1296;
            this.Height = h;
            this.Width = w;
            IDpad1 = -1; // Stupid variable to identify player in kinect ?
            IDpad2 = -1; // Stupid variable to identify player in kinect ?

            Console.WriteLine("MEDIDAS DA JANELA 1PLAYER: Width:" + this.Width.ToString() + " Heigth:" + this.Height.ToString());
            Console.WriteLine("MEDIDAS GET 1PLAYER: Width:" + w.ToString() + " Heigth:" + h.ToString());

            // This is just fucked up, we need a class to save a reference to all created objects and then destroy them when unload pong.

            //codigo supostamente desnecessario
            jogadores[0] = new Player(); 
            jogadores[1] = new Player(); 

            P1 = new Player(); // Player 1
            P2 = new Player(); //Player 2
            ball = new Ball(w * 0.5, h * 0.5, h * 0.04); // New ball centrada no ecra

            move = 0;
            Bola.Width = h * 0.05;  // variaveis visuais
            Bola.Height = h * 0.05;

            Pad1.Width = w * 0.03;
            Pad1.Height = h * 0.2;
            Pad2.Width = w * 0.03;
            Pad2.Height = h * 0.2;

            gamestarted = false;    // not started yet

            SetUp(); // New Game settings (pads position at center, ball position, etc)

            /*// Every 30 ms will call dispatcherTimer_Tick to update game (More or like 33 fps game)
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);    // intervalo de tempo do timer em milisegundos (0)
            dispatcherTimer.Tick += dispatcherTimer_Tick;

           


            FrameUtils.Restart1P += FrameUtils_Restart;//comecar jogo do inicio
            FrameUtils.Resume1P += FrameUtils_Resume; //continuar o jogo, neste caso do inico*/

            // When Load, call Pong1Player_Loaded()
            this.Loaded += Pong1Player_Loaded;
            // When unLoaded, call Pong1Player_Unloaded()
            this.Unloaded += Pong1Player_Unloaded;
            // Settings for tutorial (First page before game starts)
            setTutorial();
        }

        /**
         * What it does after app is loaded
         * 
         */
        public void Pong1Player_Loaded(object sender, RoutedEventArgs e)
        {
            
            // Sets 1 player game
            FrameUtils.SetMode("1p");
            Console.WriteLine("fui chamado 1 player");
            // When there is a interaction event in KinectAPI, it will call KinectApi_InteractionEvent() to update Y position of the hand
            KinectApi.InteractionEvent += KinectApi_InteractionEvent;
            // Starts the game
            StartGame();
        }

        /**
         * What it does after app is unloaded
         * 
         */
        public void Pong1Player_Unloaded(object sender, RoutedEventArgs e)
        {
            // Kinect will no more call for function KinectApi_InteractionEvent() when there an kinect event
            KinectApi.InteractionEvent -= KinectApi_InteractionEvent;
        }

        /**
         * Change the settings of the tutorial (pre-game help) and sets it as visible
         * 
         */
        private void setTutorial()
        {
            Tutorial.Visibility = Visibility.Visible;
            Tutorial.Height = h*0.7;
            Tutorial.Width = w*0.9;
            foreach (Viewbox v in Tutorial.Children)
            {
                v.Height = h*0.1;
                v.Width = w*0.8;
                if (v.Name == "t1")
                {
                    v.Height = h*0.15;
                }
            }
            Canvas.SetTop(Tutorial,h*0.15);
            Canvas.SetLeft(Tutorial,w*0.05);
            
        }

        /**
         * Hides tutorial (pre-game help)
         * 
         */
        private void hideTutorial()
        {
            Tutorial.Visibility = Visibility.Collapsed;
        }

        
        /**
         * After Pause, it needs to resume
         * 
         */
        private void FrameUtils_Resume(Player p1, double y1, Player p2, double y2, Ellipse ibola, Ball b)
        {
            Console.WriteLine("Chamou FRAME RESUME");
            P1 = p1;
            Canvas.SetTop(Pad1, y1);
            P2 = p2;
            Canvas.SetTop(Pad2, y2);

            Bola = ibola;
            ball = b;
            gamestarted = false;
            /*if (res <= 0)
            {
                res = 3;
                resume = true;
                t.Start();
            }*/

            resume = false;
            Countdown.Content = "";
            gamestarted = true;
            pause = false;
            dispatcherTimer.Start();
        }

        /**
         * Restart Game
         * 
         */
        void FrameUtils_Restart()
        {
            highscore = 0;
            dispatcherTimer.Start();
            score1 = score2 = 0;
            score.Content = score1 + "         " + score2;
            winner.Visibility = Visibility.Hidden;
            gamestarted = false;
            pause = false;
            SetUp();
            setTutorial();
        }

        //Pong Logic
        public void SetUp()
        {
            P1.yactual = P1.yimage = h / 2 - Pad2.Height / 2;
            P2.yactual = P2.yimage = h / 2 - Pad2.Height / 2;

            ball.x = w * 0.5;   // codigo supostamente repetido
            ball.y = h * 0.5;

            Canvas.SetTop(Bola, ((h - Bola.Height) / 2));   // Canvas provavelmente para def dimensoes da area de jogo
            Canvas.SetLeft(Bola, ((w - Bola.Width) / 2));
            Canvas.SetTop(Pad2, h / 2 - Pad2.Height / 2);
            Canvas.SetRight(Pad2, w * 0.05);
            Canvas.SetLeft(Pad1, w * 0.05);
            Canvas.SetTop(Pad1, h / 2 - Pad1.Height / 2);
        } //Y

        /**
         * Change first settings to start the game
         * 
         */
        private void StartGame()
        {
            Console.WriteLine("StartGame 1 player");
            hideTutorial(); // after game starts hide tutorial menu
            
            Random r = new Random();

            //definir numero random entre -1 e 2
            int rand = 0;
            while (rand == 0)
            {
                rand = r.Next(-1, 2);
            }
            //definir velocidade e sentido da bola em x e em y consoante o vencedor
            //DESCOMENTAR PARA A BOLA SE MOVER
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

            gamestarted = true; //flag gamestarted a true
            winner.Content = "";    //limpar conteudo da janela que anuncia o vencedor

            //se o jogo comecou de novo apos ter terminado atualizase o painel do score e colocase a flag gameended a false
            if (gameended == true)
            {
                score.Content = score1.ToString() + "       " + score2.ToString();
                gameended = false;
            }

            dispatcherTimer.Start();//iniciar timer
        }//Y

        /**
         * AI to Player 1 (On the left of the screen)
         * 
         */
        private void AiPadMovement()
        {
            if (Math.Abs(P1.yactual - P1.yimage) > h * 0.03)
            {
                if (P1.yactual > P1.yimage)
                {
                    P1.yimage = P1.yimage + h * 0.02;
                }
                else
                {
                    P1.yimage = P1.yimage - h * 0.02;
                }
            }
            if (Math.Abs(P1.yactual - P1.yimage) < h * 0.03)
            {
                P1.yimage = P1.yactual;
            }
            if (P1.yimage >= (h - Pad1.Height))
            {
                P1.yimage = h - Pad1.Height;
            }
            else if (P1.yimage <= 0)
            {
                P1.yimage = 0;
            }

            P1.yactual = ball.y;
            Canvas.SetTop(Pad1, P1.yimage);
        }//Y

        /**
         * Detect Collision of the ball
         * 
         */
        private void DetectCollision()
        {
            //reinicio do jogo e pountuacao quando o player 2(direita) vence
            if (ball.x <= ball.r)
            {
                playwinner = false;
                score2++;
                score.Content = score1 + "              " + score2;
                gamestarted = false;
                SetUp();

                if (score2 == 5)
                {
                    GameEnded();
                    highscore = 0;
                    FrameUtils_Restart();
                }
            }
            //reinicio do jogo e pountuacao quando o player 1(esquerda) vence
            if (ball.x >= w - ball.r)
            {
                playwinner = true;
                score1++;
                score.Content = score1 + "              " + score2;
                gamestarted = false;
                SetUp();
                if (score1 == 5)
                {
                    GameEnded();
                    highscore = 0;
                    FrameUtils_Restart();
                }
            }

            //bola bateu num dos tetos
            if (ball.y < ball.r / 4)
            {
                calculateNewAngleY();
            }
            if (ball.y > h - ball.r * 5 / 4)
            {
                calculateNewAngleY();
            }
            bool nbateu = true;

            // Pads 
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

            //calculo das coordenadas seguintes da bola onde se soma a velocidade desta a posicao atual
            if (nbateu)
            {
                ball.y = ball.y + (ball.speedy * h);
                ball.x = ball.x + (ball.speedx * w);
            }
        }//Y

        /**
         * Called when game ends
         * 
         */
        private void GameEnded()//Y
        {
            winner.Content = score1 > score2 ? "PLAYER 1 WINS" : "PLAYER 2 WINS";
            score1 = 0;
            score2 = 0;
            gameended = true;
            //FrameUtils.checkifHigh(this, highscore);
        }

        /**
         * Pause the game and go to Pause screen, can be also unpause.
         * 
         */
        private void Pause()//Y
        {
            // if game not started yet its not a pause
            if (!gamestarted)
            {
                pause = false;
                pausecount = 0;
                Pad1.Fill = Brushes.White;
                Pad2.Fill = Brushes.White;
                Console.WriteLine("Nova ronda 1 player");
                StartGame();    // inicia jogo

                
            }
            // if game is started its really a pause
            else
            {
                pausecount = 0;
                gamestarted = false;
                Pad1.Fill = Brushes.White;
                Pad2.Fill = Brushes.White;
                pause = true;
                dispatcherTimer.Stop();
                FrameUtils.requestPause(P1, P1.yimage, P2, P2.yimage, Bola, ball);  // suponha que guarde o estado do jogo ?
                YouNavigation.requestFrameChange(this, "YouPongPause"); // Mudar de imagem apresentando menu de pausa
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
        /**
         * baseTimer, calculating what to do in each frame
         * 
         */
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Console.WriteLine("CHAMADO TICK 1 PLAYER");
            // se timer tick durante o jogo
            if (gamestarted)
            {
                if (move == 1)
                {
                    //SaveCoordinates(); //Descomentar para guardar coordenadas da raquete do jogador num ficheiro de texto
                    //SendTextCoordinates();  //Descomentar para enviar coordenadas do ficheiro de texto para o adversario
                    //SendMessage();//enviar coordenadas para o outro Pong         
                    //ReceiveMessage();
                    //HandChanged(P1, Pad1); //movimento do pad do adversario
                    HandChanged(P2,Pad2); //movimento do pad do utilizador
                    AiPadMovement();    //movimento do pad do cpu
                    move = -1;
                }
                move++;
                DetectCollision(); //movimento da bola e as suas consequencias (vitoria, derrota etc)

                //Movimentacao da bola em x e em y
                Canvas.SetLeft(Bola, ball.x);
                Canvas.SetTop(Bola, ball.y);
                highscore += 0.03; // BUT WHY ???
            }

            if (pausecount == 0)
            {
                //P1.closed = false;
                //P2.closed = false;
                Pad2.Fill = Brushes.White;
                Pad1.Fill = Brushes.White;
            }
            if (pausecount > 0)
                pausecount--;

            if (IDpad2 != -1)
            {
                Maos.Content = "Ready To Play";
            }
        }//Y

        /**
         * When game starts, it will start the timer event
         * 
         */
        private void Tick_Resume(object sender, EventArgs e)
        {
            Console.WriteLine("Chamou TICK RESUME");
            //Countdown.Content = res.ToString();
            //if (res <= 0)
            //{
                resume = false;
                //t.Stop();
                Countdown.Content = "";
                gamestarted = true;
                pause = false;
                dispatcherTimer.Start();
            //}
            //res--;
        }//Y
        
        //Kinect
        /**
         * Change player pad position
         * 
         */
        private void HandChanged(Player p, Rectangle pad)
        {
            //Pad2 variavel de xaml
            //P2 variavel player
            if (Math.Abs(p.yactual - p.yimage) > h * 0.05)
            {
                if (p.yactual > p.yimage)
                {
                    p.yimage = p.yimage + h * 0.05;
                }
                else
                {
                    p.yimage = p.yimage - h * 0.05;
                }
            }
            if (Math.Abs(p.yactual - p.yimage) < h * 0.05)
            {
                p.yimage = p.yactual;
            }

            if (p.yimage >= (h - pad.Height))
            {
                p.yimage = h - pad.Height;
            }
            else if (p.yimage <= 0)
            {
                p.yimage = 0;
            }

            Canvas.SetTop(pad, p.yimage);
        }//Y

        /**
         * Function called whenever there is a kinect event (like hand position changed)
         * 
         */
        private void KinectApi_InteractionEvent(InteractionStreamArgs e)
        {
            /*Console.WriteLine("Interaction Stream - Pong 1P - " + DateTime.Now);
            Console.WriteLine(e.userinfo.First().HandPointers.First().Y);*/
            if (FrameUtils.GetMode() != "1p" || pause) return;
            if (e.userinfo.First() != null)
            {
                P2.yactual = e.userinfo.First().HandPointers[1].Y * this.h;
                if (e.userinfo.First().HandPointers[1].HandEventType == InteractionHandEventType.Grip)
                {
                    Pad1.Fill = Brushes.Red;
                    Pad2.Fill = Brushes.Blue;
                    P2.closed = true;
                    P1.closed = true;
                    Pause();
                }
            }


            // Resumi o código todo de baixo para o código acima
            /*UserInfo activeuser = (from u in e.userinfo
                                   where u.SkeletonTrackingId > 0
                                   orderby u.SkeletonTrackingId
                                   select u).First();

            bool R = activeuser != null;
            int i = 0;
            if (R)
            {
                Console.WriteLine(activeuser.HandPointers.Count()); // 2
                foreach (var h in activeuser.HandPointers)
                {
                    Console.WriteLine(i++);
                    if (h.IsPrimaryForUser)
                    {
                        Console.WriteLine("Y: " + h.Y);
                        if (e.userinfo.First() != null) Console.WriteLine(e.userinfo.First().HandPointers.First().Y);
                        if (e.userinfo.First() != null) Console.WriteLine(e.userinfo.First().HandPointers[1].Y);
                        P2.yactual = h.Y * this.h;
                        if (!resume)
                            if (h.HandEventType == InteractionHandEventType.Grip)
                            {
                                Pad1.Fill = Brushes.Red;
                                Pad2.Fill = Brushes.Blue;
                                P2.closed = true;
                                P1.closed = true;
                                Pause();
                            }
                    }

                }
            }*/
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


        //Funcao para criar conexao ao servidor RabbitMQ
        public void CreateConnection(){

            //readFile(); //Descomentar para testes com envio de coordenadas do ficheiro de texto

            //Criar conexao
            factory = new ConnectionFactory();
            factory.UserName = "teste";
            factory.Password = "teste";
            factory.HostName = "192.168.1.8";
            connection = factory.CreateConnection();

            //criar um canal
            channel = connection.CreateModel();

            //criar filas para envio e recepcao de das coordenadas das raquetes
            channel.QueueDeclare("Player1", false, false, false, null);
            channel.QueueDeclare("Player2", false, false, false, null);

            //Limpar as queues de mensagens que possam ainda la estar de sessoes anteriores
            channel.QueuePurge("Player1");
            channel.QueuePurge("Player2");
        }

        //Envio das coordenadas da raquete do jogador
        public void SendMessage() {
            //criacao da mensagem que e codificada para array de bytes
            string message = P2.yactual.ToString();
            var body = Encoding.UTF8.GetBytes(message);

            //enviar mensagem
            channel.BasicPublish("", "Player2", null, body);
        }

        //Recepcao das coordenadas da raquete do jogador adversario
        private void ReceiveMessage()
        {
            RabbitMQ.Client.BasicGetResult message;
            if ((message = channel.BasicGet("Player1", true)) != null)
            {
                var body = message.Body;
                String valor = Encoding.UTF8.GetString(body);
                valor = valor.Replace(".", ",");
                P1.yactual = Convert.ToDouble(valor);
                //mensagem de debug da consola
                Console.WriteLine(" [x] Received {0}", P1.yactual);
            }
        }

        //Funcao de teste para trabalhar quando existe um kinect para testar a ligacao entre 2 maquinas
        //a funcao pega nas coordenadas detetadas a cada frame pela kinect e guarda-as num ficheiro de texto
        //o ficheiro resultante sera usado pela maquina sem kinect com a ajuda da funcao ReadCoordinates para simular 
        //a detecao e envio de coordenadas para a maquina com kinect
        private void SaveCoordinates() {
            using (StreamWriter writer = new StreamWriter("racket_coordinates2.txt", true))
            {  
                    writer.WriteLine(P2.yactual.ToString());
            }
        }

        private void SendTextCoordinates() {

            if(queue.Count>0){
                string message = queue.Dequeue();
                var body = Encoding.UTF8.GetBytes(message);
 
                
                message = message.Replace(".", ",");
                P2.yactual = Convert.ToDouble(message);

                //enviar mensagem
                
                channel.BasicPublish("", "Player2", null, body);
            }
            
        }

        //funcao auxiliar que le as coordenadas do ficheiro de texto e as passa para uma queue local
        //usada em CreateConnection
        private void readFile() {

            queue = new Queue<string>();
            var lines = File.ReadLines("racket_coordinates.txt");
            foreach (var line in lines)
            {
                queue.Enqueue(line);
            }
        }
    }
}
