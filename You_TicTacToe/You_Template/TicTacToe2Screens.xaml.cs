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
    /// Interaction logic for PongPause.xaml
    /// </summary>
    /// 

    // Para guardar estado do jogo
    public partial class TicTacToe2Screens : Page, YouPlugin
    {
        private double w, h;
        private string player,oponent,playerQueue,oponentQueue,startQueue,startOpQueue,optionsQueue,opOptionsQueue;
        private bool isPlayerTurn,hasConnection,gameFinished;
        private DispatcherTimer timer,timer2;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private int playCount,numerojogador;

        public TicTacToe2Screens()
        {
            InitializeComponent();
            //Console.WriteLine("Construtor Jogo do galo chamado!");
            w = YouWindow.getWidth();
            h = YouWindow.getHeight();
            
            KinectApi.bindRegion(TicTacToe2ScreensRegion);

            // Every 30 ms will call Timer_Tick to update game (More or like 33 fps game)
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);    // intervalo de tempo do timer em milisegundos (0)
            timer.Tick += Timer_Tick;

            timer2 = new DispatcherTimer();
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 10000);    // intervalo de tempo do timer em milisegundos (10 segundos)
            timer2.Tick += UpdateTag;
            //Console.WriteLine("Criou Timer");

            // When Load, call TicTacToe2Screens_Loaded
            this.Loaded += TicTacToe2Screens_Loaded;
            this.Unloaded += TicTacToe2Screens_Unloaded;
            FrameUtils.Restart2S += FrameUtils_Restart; //comecar jogo do inicio
            FrameUtils.Start2S += FrameUtils_StartGame;

            setUpGame();

        }
    
        public void TicTacToe2Screens_Loaded(object sender, RoutedEventArgs e)
        {

            // Sets 2 screens game
            FrameUtils.SetMode("2s");
           // Console.WriteLine("TicTacToe 2 Screens loaded!!!!");

            hasConnection=CreateConnection();
            if(hasConnection){
                playCount = 0;
                gameFinished = false;
                timer.Start();
                //apenas ativa se for o jogador 1
                if (numerojogador % 2 != 0) { 
                    timer2.Start();
                }
            }
            else{
                //reencaminhar e avisar de mensagem de erro
                ConnectionFailed();
            }
        }
        public void TicTacToe2Screens_Unloaded(object sender, RoutedEventArgs e) {
            timer.Stop();
            timer2.Stop();
            //Console.WriteLine("Timer jogo parou!");
        }

        private void setUpGame() {

            

            // message board
            msgboard.Height = h * 0.1;
            msgboard.Width = w * 0.6;
            Canvas.SetTop(msgboard, h * 0.08);
            Canvas.SetLeft(msgboard, w * 0.22);

            //Quit button
            quitButton.Height = h * 0.2;
            quitButton.Width = w * 0.2;
            Canvas.SetTop(quitButton, h * 0.7);
            Canvas.SetLeft(quitButton, w * 0.75);

            //Restart button
            restartButton.Height = h * 0.2;
            restartButton.Width = w * 0.25;
            Canvas.SetTop(restartButton, h * 0.4);
            Canvas.SetLeft(restartButton, w * 0.75);

            //redimensionar grelha
            grelha.Height = h * 0.7;
            grelha.Width = h * 0.7;
            Canvas.SetTop(grelha, h * 0.1);
            Canvas.SetLeft(grelha, w * 0.1);

            //calcular medida para redimensao dos botoes da grelha e limites
            double medidacelula = (h / 3) * 0.7;
            
            //redimensionar botoes da grelha
            Botao1.Height = medidacelula;
            Botao1.Width = medidacelula;
            Botao2.Height = medidacelula;
            Botao2.Width = medidacelula;
            Botao3.Height = medidacelula;
            Botao3.Width = medidacelula;
            Botao4.Height = medidacelula;
            Botao4.Width = medidacelula;
            Botao5.Height = medidacelula;
            Botao5.Width = medidacelula;
            Botao6.Height = medidacelula;
            Botao6.Width = medidacelula;
            Botao7.Height = medidacelula;
            Botao7.Width = medidacelula;
            Botao8.Height = medidacelula;
            Botao8.Width = medidacelula;
            Botao9.Height = medidacelula;
            Botao9.Width = medidacelula;

            //redimensionar limites
            GridLengthConverter converter = new GridLengthConverter();
            linha1.Height = (GridLength)converter.ConvertFrom(medidacelula.ToString());
            linha2.Height = (GridLength)converter.ConvertFrom(medidacelula.ToString());
            linha3.Height = (GridLength)converter.ConvertFrom(medidacelula.ToString());
            coluna1.Width = (GridLength)converter.ConvertFrom(medidacelula.ToString());
            coluna2.Width = (GridLength)converter.ConvertFrom(medidacelula.ToString());
            coluna3.Width = (GridLength)converter.ConvertFrom(medidacelula.ToString());
  
        }

        
    
        private void Timer_Tick(object sender, EventArgs e) {
            //Verificar se ha vencedor (PASSAR PARA TIMER)
            if (!hasConnection) {
                //Reconduzir para lobby e enviar mensagem de aviso de falha do servidor
                msgb.Text = "Error: Connection to server has failed!";
                YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
            }
            checkWinner();

            try
            {
              RabbitMQ.Client.BasicGetResult message;

                //verificar se o adversario enviou alguma mensagem de desistencia ou reinicio do jogo
                if ((message = channel.BasicGet(opOptionsQueue, true)) != null)
                {
                    var body2 = message.Body;
                    string msg2 = Encoding.UTF8.GetString(body2);

                    //limpar queues
                    purgeQueues(msg2);

                    if(msg2.Equals("Restart")){
                        FrameUtils_Restart();
                    }
                    else if (msg2.Equals("Quit"))
                    {    
                        timer.Stop();
                        timer2.Stop();
                        //retornar ao menu inicial
                        YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
                    }
               
                }

                //caso nao seja o turno do jogador verifica se o adversario ja fez a sua jogada
                //caso o adversario tenha jogado, registase a jogada do adversario na nossa grelha e comeca o nosso turno
                else if (!isPlayerTurn) {
                    if ((message = channel.BasicGet(oponentQueue, true)) != null)
                    {
                        //Console.WriteLine("Jogada do adversário recebida");
                        var body2 = message.Body;
                        string msg2 = Encoding.UTF8.GetString(body2);

                        if (msg2.Equals("Botao1")) {
                            Botao1.Foreground = Brushes.Red;
                            Botao1.Label = oponent;
                            //Botao1.IsEnabled = false;
                           
                        }
                        else if (msg2.Equals("Botao2"))
                        {
                            Botao2.Foreground = Brushes.Red;
                            Botao2.Label = oponent;
                            //Botao2.IsEnabled = false;

                        }  
                        else if (msg2.Equals("Botao3"))
                        {
                            Botao3.Foreground = Brushes.Red;
                            Botao3.Label = oponent;
                            //Botao3.IsEnabled = false;
        
                        }
                        else if (msg2.Equals("Botao4"))
                        {
                            Botao4.Foreground = Brushes.Red;
                            Botao4.Label = oponent;
                            //Botao4.IsEnabled = false;
                       
                        }
                        else if (msg2.Equals("Botao5"))
                        {
                            Botao5.Foreground = Brushes.Red;
                            Botao5.Label = oponent;
                            //Botao5.IsEnabled = false;

                        }
                        else if (msg2.Equals("Botao6"))
                        {
                            Botao6.Foreground = Brushes.Red;
                            Botao6.Label = oponent;
                            //Botao6.IsEnabled = false;

                        }
                        else if (msg2.Equals("Botao7"))
                        {
                            Botao7.Foreground = Brushes.Red;
                            Botao7.Label = oponent;
                            //Botao7.IsEnabled = false;

                        }
                        else if (msg2.Equals("Botao8"))
                        {
                            Botao8.Foreground = Brushes.Red;
                            Botao8.Label = oponent;
                            //Botao8.IsEnabled = false;

                        }
                        else if (msg2.Equals("Botao9"))
                        {
                            Botao9.Foreground = Brushes.Red;
                            Botao9.Label = oponent;
                            //Botao9.IsEnabled = false;

                        }
                        playCount++;    //numero de jogadas incrementado
                        //passa a ser o turno do jogador
                        isPlayerTurn = true;
                        msgb.Text = "It's your turn to play!";
                    }
                
                }
            }
            catch(Exception exception)
            {
                hasConnection = false;
                ConnectionFailed();
            }
        }

        private void UpdateTag(object sender, EventArgs e)
        {
            try
            {
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
                    //Console.WriteLine("Atualizar tag do jogador " + numerojogador + " para: " + tag);
                    body = Encoding.UTF8.GetBytes(tag.ToString());
                    //repor mensagem na queue
                    channel.BasicPublish("", queue, null, body);

                }
            }
            catch (Exception exception)
            {
                ConnectionFailed();
            }
        }

    
        //funcao chamada quando um botao da grelha e ativado
        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            try{
                var b = (KinectTileButton)e.OriginalSource;
                if(isPlayerTurn && b.Label.Equals("") && !gameFinished){
                    
                   //bloquear botao e colocar marca do jogador
                    b.Foreground = Brushes.Blue;
                  b.Label = player;
                
                   //Console.WriteLine("Button pressed!"+b.Name.ToString());
                   //b.IsEnabled = false;
                 playCount++;    //numero de jogadas incrementado

                    //enviar jogada ao adversario e passar o turno
                    var body = Encoding.UTF8.GetBytes(b.Name);
                    channel.QueuePurge(playerQueue);
                    //enviar mensagem
                  channel.BasicPublish("", playerQueue, null, body);

                   //terminar turno do jogador
                 isPlayerTurn = false;   
                 msgb.Text = "It's the oponent's turn!";
             }
          }
          catch(Exception exception) {
              ConnectionFailed();

          }
        }

        //funcao que verifica se houve um vencedor
        private void checkWinner() {
            //coluna 1
            if ((Botao1.Label.Equals(Botao2.Label) && Botao3.Label.Equals(Botao2.Label)) && !Botao2.Label.Equals("")) {
                endGame(Botao2.Label.ToString());
            }
            //coluna 2
            else if ((Botao4.Label.Equals(Botao5.Label) && Botao6.Label.Equals(Botao5.Label)) && !Botao5.Label.Equals(""))
            {
                endGame(Botao5.Label.ToString());
            }
           
            //coluna 3
            else if ((Botao7.Label.Equals(Botao8.Label) && Botao9.Label.Equals(Botao8.Label)) && !Botao8.Label.Equals(""))
            {
                endGame(Botao8.Label.ToString());
            }
            
            //linha1
            else if ((Botao1.Label.Equals(Botao4.Label) && Botao7.Label.Equals(Botao4.Label)) && !Botao4.Label.Equals(""))
            {
                endGame(Botao4.Label.ToString());
            }
            
            //linha2
            else if ((Botao2.Label.Equals(Botao5.Label) && Botao8.Label.Equals(Botao5.Label)) && !Botao5.Label.Equals(""))
            {
                endGame(Botao5.Label.ToString());
            }
            
            //linha3
            else if ((Botao3.Label.Equals(Botao6.Label) && Botao9.Label.Equals(Botao6.Label)) && !Botao6.Label.Equals(""))
            {
                endGame(Botao6.Label.ToString());
            }
            
            //diagonal esquerda-direita
            else if ((Botao1.Label.Equals(Botao5.Label) && Botao9.Label.Equals(Botao5.Label)) && !Botao5.Label.Equals(""))
            {
                endGame(Botao5.Label.ToString());
            }
            
            //diagonal direita-esquerda
            else if ((Botao3.Label.Equals(Botao5.Label) && Botao7.Label.Equals(Botao5.Label)) && !Botao5.Label.Equals(""))
            {
                endGame(Botao5.Label.ToString());
            }
            else if (playCount == 9) {
                msgb.Text = "It's a draw!";
            }

        }
        private void endGame(String p) {

            if(player.Equals(p)) {
                msgb.Text = "You've Won!";
            }else{
                msgb.Text = "You've Lost!";
            }
            gameFinished = true;
            //impedir selecao de mais botoes
            /*Botao1.IsEnabled = false;
            Botao2.IsEnabled = false;
            Botao3.IsEnabled = false;
            Botao4.IsEnabled = false;
            Botao5.IsEnabled = false;
            Botao6.IsEnabled = false;
            Botao7.IsEnabled = false;
            Botao8.IsEnabled = false;
            Botao9.IsEnabled = false;*/
            
            //YouNavigation.requestFrameChange(this, "YouTicTacToe");

        }


        private void FrameUtils_Restart()
        {

            playCount = 0;
            Botao1.Label = "";
            Botao2.Label = "";
            Botao3.Label = "";
            Botao4.Label = "";
            Botao5.Label = "";
            Botao6.Label = "";
            Botao7.Label = "";
            Botao8.Label = "";
            Botao9.Label = "";
            Botao1.IsEnabled = true;
            Botao2.IsEnabled = true;
            Botao3.IsEnabled = true;
            Botao4.IsEnabled = true;
            Botao5.IsEnabled = true;
            Botao6.IsEnabled = true;
            Botao7.IsEnabled = true;
            Botao8.IsEnabled = true;
            Botao9.IsEnabled = true;

            gameFinished = false;
            
            //o jogador X comeca primeiro
           if(player.Equals("X")){
               isPlayerTurn = true;
               msgb.Text = "It's your turn to play!";
           }
           else {
               isPlayerTurn = false;
               msgb.Text = "It's the oponent's turn!";
           }
        }

        private void FrameUtils_StartGame(int nj, string hn, string un, string pass, int p)
        {
            factory = new ConnectionFactory();
            factory.UserName = un;
            factory.Password = pass;
            factory.Port = p;
            factory.HostName = hn;

            numerojogador = nj;
            //definir queues e posicao do jogador
            playerQueue = "Player"+nj.ToString();
            startQueue = "Start" + nj.ToString();
            optionsQueue = "Options" + nj.ToString();

            //se o numero do jogador for par trata-se do jogador 2 caso contrario e o jogador 1
            if (nj % 2 == 0) {
                isPlayerTurn = false;
                player = "O";
                oponent = "X";
                int noponente = nj-1;
                oponentQueue = "Player" + noponente.ToString();
                startOpQueue = "Start" + noponente.ToString();
                opOptionsQueue = "Options" + noponente.ToString();
                msgb.Text = "It's the oponent's turn!";
            }
            else {
                isPlayerTurn = true;
                player = "X";
                oponent = "O";
                int noponente = nj + 1;
                oponentQueue = "Player" + noponente.ToString();
                startOpQueue = "Start" + noponente.ToString();
                opOptionsQueue = "Options" + noponente.ToString();
                msgb.Text = "It's your turn to play!";

            }


            //private string player,oponent,playerQueue,oponentQueue,startQueue,startOpQueue,optionsQueue,opOptionsQueue;
             //private bool isPlayerTurn;
        }


        private void Options_Click(object sender, RoutedEventArgs e)
        {
            try{
                var b = (KinectTileButton)e.OriginalSource;

                if (b.Name.Equals("quitButton") && hasConnection) //botao de desistencia de jogo
                {
                    //enviar mensagem de desistencia ao adversario
                    var body = Encoding.UTF8.GetBytes("Quit");
                    channel.QueuePurge(optionsQueue);
                    //enviar mensagem
                    channel.BasicPublish("", optionsQueue, null, body);

                    timer.Stop();
                    timer2.Stop();
                    //retornar ao menu inicial
                    YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
                
                }
                else if (b.Name.Equals("restartButton") && hasConnection) //botao de reinicio de jogo
                {
                    //enviar mensagem de desistencia ao adversario
                    var body = Encoding.UTF8.GetBytes("Restart");
                    channel.QueuePurge(optionsQueue);
                    //enviar mensagem
                    channel.BasicPublish("", optionsQueue, null, body);
                    FrameUtils_Restart();
                }
            }
            catch (Exception exception) {
                hasConnection = false;
            }

        }

        private bool CreateConnection()
        {
            try {
                  
                //Console.WriteLine("Queue jogador "+playerQueue +"Queue adversario"+ oponentQueue);
                connection = factory.CreateConnection();

                //criar um canal
                channel = connection.CreateModel();

                //criar filas para envio e recepcao de das coordenadas das raquetes
                channel.QueueDeclare(playerQueue, false, false, false, null);
                channel.QueueDeclare(oponentQueue, false, false, false, null);
                channel.QueueDeclare(startQueue, false, false, false, null);
                channel.QueueDeclare(oponentQueue, false, false, false, null);
                channel.QueueDeclare(optionsQueue, false, false, false, null);
                channel.QueueDeclare(opOptionsQueue, false, false, false, null);

                //uint msgcount = status.MessageCount; //result.MessageCount;

                //definir se o jogador sera player x ou o
                //definePlayer(msgcount);

                //Reinicia o campo de jogo
                FrameUtils_Restart();

               // Console.WriteLine("JOGO INICIADO");
                return true;
            }
            catch (Exception e) {
                return false;
            }

        }

        private void ConnectionFailed()
        {
            hasConnection = false;
            timer.Stop();
            timer2.Stop();
            msgb.Text = "Error: Connection to server has failed!";
            YouNavigation.requestFrameChange(this, "YouTicTacToeLobby");
        }

        //Limpar as queues de mensagens
        private void purgeQueues(string option)
        {
           try {
            if(option.Equals("Quit")){
                channel.QueuePurge(playerQueue);
                channel.QueuePurge(oponentQueue);
                channel.QueuePurge(startQueue);
                channel.QueuePurge(startOpQueue);
                channel.QueuePurge(optionsQueue);
                channel.QueuePurge(opOptionsQueue);
            }
            else {
                channel.QueuePurge(playerQueue);
                channel.QueuePurge(oponentQueue);
                channel.QueuePurge(optionsQueue);
                channel.QueuePurge(opOptionsQueue);
            }
           }
           catch(Exception exception) {
               ConnectionFailed();
           }
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
            return TicTacToe2ScreensRegion;
        }

        #endregion

    }
}
