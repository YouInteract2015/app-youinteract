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
using RabbitMQ.Client;

namespace You_TicTacToe
{
    /// <summary>
    /// Interaction logic for WaitScreen.xaml
    /// </summary>
    public partial class TicTacToeWaitScreen : Page, YouPlugin
    {
        private double w, h;
        private string myqueue,oponentqueue;
        private int numerojogador;
        private bool hasConnection,goToGame;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private DispatcherTimer timer,timer2;
        
        public TicTacToeWaitScreen()
        {
            InitializeComponent();
            w = YouWindow.getWidth();
            h = YouWindow.getHeight();
            KinectApi.bindRegion(TicTacToeWaitScreenRegion);

            FrameUtils.WaitScreen += FrameUtils_WaitScreen;

            this.Loaded += TicTacToeWaitScreen_Loaded;
            this.Unloaded += TicTacToeWaitScreen_Unloaded;

            // Criar timer para verificar periodicamente os jogos disponiveis
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);    // intervalo de tempo do timer em milisegundos (0)
            timer.Tick += CheckOponent;

            //timer para update da tag
            timer2 = new DispatcherTimer();
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 10000);    // intervalo de tempo do timer em milisegundos (10 segundos)
            timer2.Tick += UpdateTag;

            setWindow();

        }

        private void setWindow() {
            // message board
            msgboard.Height = h * 0.1;
            msgboard.Width = w * 0.6;
            Canvas.SetTop(msgboard, h * 0.1);
            Canvas.SetLeft(msgboard, w * 0.20);

            //Quit button
            quitButton.Height = h * 0.2;
            quitButton.Width = w * 0.2;
            Canvas.SetTop(quitButton, h * 0.5);
            Canvas.SetLeft(quitButton, w * 0.38);

        }


        //verificar periodicamente se apareceu um oponente
        private void CheckOponent(object sender, EventArgs e) {
            try {
                //caso o adversario envie uma mensagem o jogo inicia-se
                RabbitMQ.Client.BasicGetResult message;
                if((message = channel.BasicGet(oponentqueue, true)) != null){
                    //repor mensagem do adversario para marcar jogo como ocupado
                    channel.BasicPublish("", oponentqueue, null, message.Body);

                    //parar a verificacao do oponente
                    timer.Stop();
                    timer2.Stop();

                    // partir para o jogo enviando os dados da conexao o numero do jogador e do oponente
                    goToGame = true;
                    //Console.WriteLine("VAI COMECAR O JOGO "+message.Body);
                    FrameUtils.RequestStartGame(numerojogador, factory.HostName, factory.UserName, factory.Password, factory.Port);
                    YouNavigation.requestFrameChange(this, "YouTicTacToe2Screens");
                }
            }
            catch (Exception exception) {
                ConnectionFailed();
            }
        }

        //fazer o update a tag
        private void UpdateTag(object sender, EventArgs e) {
            try{
                string queue = "Tag" + numerojogador;
                channel.QueueDeclare(queue, false, false, false, null);
                RabbitMQ.Client.BasicGetResult message;

                if ((message = channel.BasicGet(queue, true)) != null)
                {
                    
                    //converter numero da mensagem
                    var body = message.Body;
                    string msg = Encoding.UTF8.GetString(body);
                    int tag = Convert.ToInt32(msg);
                    //incrementar a tag
                    tag++;
                   // Console.WriteLine("Atualizar tag do jogador " + numerojogador+" para: "+tag);
                    body = Encoding.UTF8.GetBytes(tag.ToString());
                    //repor mensagem na queue
                    channel.BasicPublish("", queue, null, body);
                    
                }
            }
            catch(Exception exception){
                ConnectionFailed();
            }
        }
        private void TicTacToeWaitScreen_Loaded(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("Fez load do wait screen!");
            //Console.WriteLine("numero jogador= " + numerojogador +" hostname: "+ factory.HostName);
            //criar conexao
            hasConnection= createConnection();
            //se houver conexao estabelecida inicia a espera pelo oponente
            if (hasConnection)
            {
                channel.QueueDeclare(myqueue, false, false, false, null);
                channel.QueueDeclare(oponentqueue, false, false, false, null);
                //iniciar timer de recepcao ao teu oponente mas antes limpar a queue do adversario
                channel.QueuePurge(oponentqueue);
               // Console.WriteLine("limpou queue "+oponentqueue);
                timer.Start();
                timer2.Start();
            }
            else {
                ConnectionFailed();
            }
            goToGame = false;
        }

        private void TicTacToeWaitScreen_Unloaded(object sender, RoutedEventArgs e) {
            if (hasConnection && !goToGame)
            {
                //Console.WriteLine("Loby fez Purge da queue " + myqueue);
                channel.QueuePurge(myqueue);
            }
            timer.Stop();
            timer2.Stop();

        }

        private bool createConnection() {
            try
            {
                
                connection = factory.CreateConnection();
                //criar um canal
                channel = connection.CreateModel();
                msgb.Text = "Waiting for oponent";
                return true;
            }
            catch (Exception e)
            {
                msgb.Text = "Error: Connection to server has failed!";
                return false;
                //Voltar ao lobby
                //YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
            }
        }

        //funcao utilizada para impedir que o programa crashe quando a conexao falha, parando a consulta das queues e avisando o utilizador
        private void ConnectionFailed()
        {
            hasConnection = false;
            timer.Stop();
            timer2.Stop();
            msgb.Text = "Error: Connection to server has failed!";
        }
        private void FrameUtils_WaitScreen(int nj, string hn,string un, string pass, int p)
        {
          
            factory = new ConnectionFactory();
            factory.UserName = un;
            factory.Password = pass;
            factory.Port = p;
            factory.HostName = hn;
            numerojogador = nj;
            myqueue = "Start" + numerojogador;
            int noponente = numerojogador + 1;
            oponentqueue = "Start" + noponente;

        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
           
            YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
            
        }

        //YouPlugin
        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_TicTacToe";
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
            return new KinectRequirements(true, false, false);
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
            return TicTacToeWaitScreenRegion;
        }

        #endregion

        
    }


}
