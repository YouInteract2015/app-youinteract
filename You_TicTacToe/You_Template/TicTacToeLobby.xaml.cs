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
using System.Xml.Linq;
using System.Xml;
using System.Collections;

namespace You_TicTacToe
{
    /// <summary>
    /// Interaction logic for TicTacToeLobby.xaml
    /// </summary>
    public partial class TicTacToeLobby : Page,YouPlugin
    {
        private double w, h;
        private string Nickname;
        private int nlinhas, ncolunas;
        private YouButton[,] botoes;
        private Brush[,] slotStatus;
        private int[,] slotCheckCounter;
        private int[,] slotMsgTag;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private bool hasConnection;
        private DispatcherTimer timer,timer2;
        private XDocument config;
        public TicTacToeLobby()
        {
            InitializeComponent();
            KinectApi.bindRegion(TicTacToeLobbyRegion);
            this.Loaded += TicTacToeLobby_Loaded;
            this.Unloaded += TicTacToeLobby_Unloaded;

            

            // Criar timer para verificar periodicamente os jogos disponiveis
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);    // intervalo de tempo do timer em milisegundos(30)
            timer.Tick += checkGames;
            //Console.WriteLine("Criou Timer");

            // Criar timer para verificar periodicamente os jogos disponiveis
            timer2 = new DispatcherTimer();
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 1000);    // intervalo de tempo do timer em milisegundos(30)
            timer2.Tick += checkActivity;
            //Console.WriteLine("Criou Timer");

            setWindow();
            
            
        }
        public void TicTacToeLobby_Loaded(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("Fez load do Lobby!");
            InitializeConfig();

            hasConnection = createConnection();

            //apenas verifica jogos se existir conexao
            if(hasConnection){
                //Console.WriteLine("Tem conexao");
                createButton.Visibility = Visibility.Visible;
                timer.Start();
                timer2.Start();
            }
            else {
                ConnectionFailed();
            }
            
        }
        public void TicTacToeLobby_Unloaded(object sender, RoutedEventArgs e){
            timer.Stop();
            timer2.Stop();
            //Console.WriteLine("Timer lobby parou!");
        }

        private void InitializeConfig() {
            config = null;
            try {
                config = YouXMLFetcher.getAppXml(this);
                string name="";
                string ip="";
                string port="0";
                foreach (var child in config.Descendants("name"))
                {
                    name = child.Element("value").Value;
                    //Console.WriteLine("LEU ISTO DO NOME: " + name);
                }
                foreach (var child in config.Descendants("ip"))
                {
                    ip = child.Element("value").Value;
                    //Console.WriteLine("LEU ISTO DO IP: " + ip);
                }
                foreach (var child in config.Descendants("port"))
                {
                    port = child.Element("value").Value;
                    //Console.WriteLine("LEU ISTO DO port: " + port);
                }

                //Criar conexao
                factory = new ConnectionFactory();
                factory.Port = Convert.ToInt32(port);
                factory.HostName = ip;
                Nickname = name;
          
            }
            catch {
                msgb.Text = "Error: Invalid portal configurations";
            }
        }

        private void setWindow()
        {
            h = this.Height;
            w = this.Width;

            //posicionar e redimensionar grelha de jogos
            grelhaJogos.Height = h * 0.6;
            grelhaJogos.Width = w;
            Canvas.SetTop(grelhaJogos, 0);
            Canvas.SetLeft(grelhaJogos, 0);

            // message board
            msgboard.Height = h * 0.1;
            msgboard.Width = w * 0.5;
            Canvas.SetTop(msgboard, h * 0.65);
            Canvas.SetLeft(msgboard, w * 0.22);

            // Create game button
            createButton.Height = h * 0.2;
            createButton.Width = w * 0.25;
            Canvas.SetTop(createButton, h * 0.75);
            Canvas.SetLeft(createButton, w * 0.1);
            createButton.Visibility = Visibility.Hidden;

            //Quit button
            quitButton.Height = h * 0.2;
            quitButton.Width = w * 0.2;
            Canvas.SetTop(quitButton, h * 0.75);
            Canvas.SetLeft(quitButton, w * 0.6);

            //calcular medida para redimensao dos botoes da grelha
            double alturacelula = (h / 3) * 0.6;
            double larguracelula = (w / 4);

            //definir numero de linhas e numero de colunas
            nlinhas = 3;
            ncolunas = 4;
            botoes = new YouButton[nlinhas, ncolunas];
            slotStatus = new Brush[nlinhas, ncolunas];
            slotCheckCounter = new int[nlinhas, ncolunas];
            slotMsgTag = new int[nlinhas, ncolunas];

            //atribuir botoes a grelha de jogos disponiveis
            int i = 0;
            for (; i < nlinhas; i++)
            {
                int j = 0;
                for (; j < ncolunas; j++)
                {
                    //Definir configuracao de cada botao
                    slotCheckCounter[i, j] = 0;
                    slotMsgTag[i, j] = 0;
                    slotStatus[i, j] = Brushes.White;
                    botoes[i, j] = new YouButton();
                    botoes[i, j].Background = Brushes.White;
                    botoes[i, j].LabelBackground = null;
                    botoes[i, j].Foreground = Brushes.White;
                    botoes[i,j].HorizontalLabelAlignment=HorizontalAlignment.Center;
                    botoes[i, j].VerticalLabelAlignment = VerticalAlignment.Top;
                    botoes[i, j].Visibility = Visibility.Hidden;
                    botoes[i, j].Label = "";
                    int numerojogador = ((((i * ncolunas) + 1) + j) * 2) - 1;//conversao do numero da slot para o respetivo jogador 1 da slot
                    botoes[i, j].Tag = numerojogador;
                    botoes[i,j].Click +=GameButton_Click;
                    Grid.SetRow(botoes[i, j], i);
                    Grid.SetColumn(botoes[i, j], j);
                    botoes[i, j].Height = alturacelula;
                    botoes[i, j].Width = larguracelula;
                    grelhaJogos.Children.Add(botoes[i, j]);

                }
            }

            //redimensionar limites
            GridLengthConverter converter = new GridLengthConverter();
            linha1.Height = (GridLength)converter.ConvertFrom(alturacelula.ToString());
            linha2.Height = (GridLength)converter.ConvertFrom(alturacelula.ToString());
            linha3.Height = (GridLength)converter.ConvertFrom(alturacelula.ToString());
            coluna1.Width = (GridLength)converter.ConvertFrom(larguracelula.ToString());
            coluna2.Width = (GridLength)converter.ConvertFrom(larguracelula.ToString());
            coluna3.Width = (GridLength)converter.ConvertFrom(larguracelula.ToString());
            coluna4.Width = (GridLength)converter.ConvertFrom(larguracelula.ToString());
        }
        private bool createConnection() {
            try
            {
                
                factory.UserName = "teste";
                factory.Password = "teste";
                connection = factory.CreateConnection();
                //criar um canal
                channel = connection.CreateModel();
                msgb.Text = "Choose a game or create a new one!";
                return true;
            }
            catch (Exception e)
            {
                msgb.Text = "Error: Connection to server has failed!";
                return false;
            }
            
        }

        //verificar que jogos estao a ser jogados e quais os disponiveis
        private void checkGames(object sender, EventArgs e)
        {
            try{
            //percorrer cada uma das slots para verificar se um jogo ja se encontra la ou nao
                int i;
                for(i=0;i<nlinhas;i++){
                    int j;
                    for (j = 0; j < ncolunas;j++ )
                    {
                        int numerojogador = ((((i * ncolunas)+1)+j)*2)-1;//conversao do numero da slot para o respetivo jogador 1 da slot
                        //Console.WriteLine("Checking player"+numerojogador+" lina e coluna: "+i.ToString()+" "+j.ToString());

                        //verificar se ja existe um jogo a ser executado nesta slot pela existencia ou nao de mensagens na queue
                        QueueDeclareOk status = channel.QueueDeclare("Start"+numerojogador.ToString(), false, false, false, null);
                        uint msgcount = status.MessageCount; //result.MessageCount;
                        if (msgcount > 0) {
                            botoes[i, j].Visibility = Visibility.Visible;
                            botoes[i, j].Background = Brushes.Blue;
                            //Console.WriteLine("Ha jogo no jogador "+numerojogador.ToString());
                            //

                            //verificar se o jogo esta disponivel para entrar ou nao verificando se o jogador 2 da respetiva slot existe
                            status = channel.QueueDeclare("Start" + (numerojogador+1), false, false, false, null);
                            msgcount = status.MessageCount; //result.MessageCount;
                            if (msgcount > 0){
                                botoes[i, j].Background = Brushes.Red;
                            }
                        }
                        //a slot encontrase vazia
                        else {
                           // Console.WriteLine("JOGO MUDADO PARA LIVRE " + numerojogador);
                            //Console.WriteLine("Coluna "+ i.ToString()+" "+j.ToString()+ " esta vazia");
                            botoes[i, j].Background = Brushes.White;
                            botoes[i, j].Visibility = Visibility.Hidden;
                            botoes[i, j].Label = "";

                        }
                        //caso o estado da slot tenha mudado para um jogo disponivel ou ocupado verifica-se os jogadores presentes
                        if(!slotStatus[i,j].Equals(botoes[i,j].Background)){

                            //caso o jogo esteja disponivel a informaçao completa encontra-se na queue do jogador1 caso esteja ocupado encontrase na queue do jogador 2
                            if (botoes[i, j].Background.Equals(Brushes.Blue))
                            {
                                //Console.WriteLine("JOGO MUDADO PARA DISPONIVEL " + numerojogador);
                                checkPlayersInfo(numerojogador,i,j);
                            }
                            else if (botoes[i, j].Background.Equals(Brushes.Red))
                            {
                                int noponente = numerojogador + 1;
                                //Console.WriteLine("JOGO MUDADO PARA OCUPADO "+noponente);
                                checkPlayersInfo(noponente, i, j);
                            
                            }
                            //atualizar o estado da slot
                            slotStatus[i, j] = botoes[i, j].Background;
                        }
                    }
                }
            }
            catch(Exception exception) { //parar de consultar as queues caso a connexao falhe
                ConnectionFailed();
            }
        }

        //verificar atividade dos jogos para limpar jogos crashados
        private void checkActivity(object sender, EventArgs e) {
            try {
                //Console.WriteLine("CHECK ACTIVITY");
                int i;
                for(i=0;i<nlinhas;i++){
                    int j;
                    for (j = 0; j < ncolunas;j++ )
                    {
                        //Verifica apenas os jogos sinalizados por jogadores
                        if (botoes[i, j].Background.Equals(Brushes.Blue) || botoes[i, j].Background.Equals(Brushes.Red))
                        {
                            int numerojogador = ((((i * ncolunas)+1)+j)*2)-1;//conversao do numero da slot para o respetivo jogador 1 da queue
                            string queue = "Tag" + numerojogador.ToString();
                            channel.QueueDeclare(queue, false, false, false, null);
                            RabbitMQ.Client.BasicGetResult message;
                          //verificar se existe
                                if ((message = channel.BasicGet(queue, true)) != null)
                                {
                                    //repor mensagem na queue
                                    channel.BasicPublish("", queue, null, message.Body);

                                    //converter numero da mensagem
                                    var body = message.Body;
                                    string msg = Encoding.UTF8.GetString(body);
                                    int tag = Convert.ToInt32(msg);
                                    //verificar atualizacao da tag
                                    checkLobbyActive(tag,i,j,numerojogador);
                                         
                                }
                          }
                    }
             }
            }
            catch (Exception exception)
            { //parar de consultar as queues caso a connexao falhe
                ConnectionFailed();
            }
        }

        private void checkPlayersInfo(int nj,int i,int j) {
            RabbitMQ.Client.BasicGetResult message;
            string queue= "Start"+nj;
            //retirar informacao do jogador
            while((message = channel.BasicGet(queue, true))==null);
            var body = message.Body;
            var msg = Encoding.UTF8.GetString(body);

            //repor mensagem do jogador        
            channel.BasicPublish("", queue, null,message.Body);

            //colocar informacao no botao de acordo com o numero de jogadores presentes no jogo
            if (nj % 2 == 0) {
                string[] players = msg.Split('_');
                botoes[i, j].Label = players[0] + "\n" + "vs" + "\n" + players[1];
                //Console.WriteLine("JOGO OCUPADO COM : " + msg);
            }
            else {
                botoes[i, j].Label = msg;
                //Console.WriteLine("JOGO DISPONIVEL COM : " + msg);
            }
        }

        //verificar se houve atualizacao da tag do jogo em questao.
       //Tag nao atualizou-> incrementa-se o contador da slot
       //Tag atualizou-> Reinicia-se o contador e guarda-se a nova tag
       //o contador ao atingir um atingir um determinado limite
        private void checkLobbyActive(int tag,int i,int j,int nj) {
            //Console.WriteLine("Tag de "+nj+ " : "+tag);
            if(slotMsgTag[i,j]==tag){
             
               slotCheckCounter[i,j]++;
            }
            else {
                slotMsgTag[i, j] = tag;
                slotCheckCounter[i, j] = 0;
            }
            if (slotCheckCounter[i,j] >= 60) {
                //fazer purge das queues de todas as queues desta slot
                PurgeQueues(nj);
                botoes[i, j].Background = Brushes.White;
                botoes[i, j].Visibility = Visibility.Hidden;
                botoes[i, j].Label = "";
                //Console.WriteLine("Reiniciou a slot" + i.ToString() + " " + j.ToString());
                slotMsgTag[i, j] = 0;
                slotCheckCounter[i, j] = 0;
            }
        }

        //limpar queues de um determinado jogo
        private void PurgeQueues(int nj) {
            try {
                int noponente =nj +1;
                channel.QueueDeclare("Start" + nj, false, false, false, null);
                channel.QueueDeclare("Start" + noponente, false, false, false, null);
                channel.QueueDeclare("Player" + nj, false, false, false, null);
                channel.QueueDeclare("Player" + noponente, false, false, false, null);
                channel.QueueDeclare("Options" + nj, false, false, false, null);
                channel.QueueDeclare("Options" + noponente, false, false, false, null);

                channel.QueuePurge("Start" + nj);
                channel.QueuePurge("Start" + noponente);
                channel.QueuePurge("Player" + nj);
                channel.QueuePurge("Player" + noponente);
                channel.QueuePurge("Options" + nj);
                channel.QueuePurge("Options" + noponente);
                channel.QueuePurge("Tag" + nj);
            }
            catch(Exception e){
                ConnectionFailed();
            }
        }

        //funcao utilizada para impedir que o programa crashe quando a conexao falha, parando a consulta das queues e avisando o utilizador
        private void ConnectionFailed() {
            hasConnection = false;
            timer.Stop();
            timer2.Stop();
            msgb.Text = "Error: Connection to server has failed!";
        }
        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            if(hasConnection){
                //percorrer cada um dos botoes ate encontrar uma slot livre
                bool slotLivre = false;
                int i;
                for(i=0;i<nlinhas && !slotLivre;i++){
                    int j;
                    for (j = 0; j < ncolunas;j++ )
                    {
                        //Console.WriteLine("Nao breakou na linha e coluna "+i.ToString()+" "+j.ToString());
                        //ao encontrar uma slot livre cria o jogo
                        if (botoes[i, j].Visibility == Visibility.Hidden) {
                            slotLivre = true;
                            int numerojogador = ((((i * ncolunas) + 1) + j) * 2) - 1;//conversao do numero da slot para o respetivo jogador 1 da slot

                            //declarar queue para verificacao da tag
                            string queue = "Tag" + numerojogador;
                            channel.QueueDeclare(queue, false, false, false, null);
                            var body = Encoding.UTF8.GetBytes("1");
                            channel.QueuePurge(queue);
                            channel.BasicPublish("", queue, null, body);


                            queue = "Start" + numerojogador.ToString();
                            channel.QueueDeclare(queue, false, false, false, null);
                            
                            //enviar mensagem  de reserva de slot
                            body = Encoding.UTF8.GetBytes(Nickname);
                            channel.QueuePurge(queue);
                            channel.BasicPublish("", queue, null, body);
                           
                            
                            
                            FrameUtils.RequestWaitScreen(numerojogador, factory.HostName, factory.UserName, factory.Password, factory.Port);
                            YouNavigation.requestFrameChange(this, "YouTicTacToeWaitScreen");
                            break;
                        }
                  }
                
                }
                //Caso nao tenha encontrado uma livre o jogador e informado que o servidor esta cheio
                if (!slotLivre)
                {
                    msgb.Text = "The server is full at the moment. Try later!";
                }
            }
        }

        //Juntar a um jogo caso este esteja disponivel
        private void GameButton_Click(object sender, RoutedEventArgs e) {
            var b = (KinectTileButton)e.OriginalSource;
            if(b.Visibility== Visibility.Visible && b.Background == Brushes.Blue && hasConnection){
                //Enviar mensagem de join para o adversario e entrar como jogador 2 no jogo(verificar se queue>1 caso alguem carregue ao mesmo tempo neste jogo)
                int numerojogador2 = Convert.ToInt32(b.Tag)+1;
                string queue = "Start" + numerojogador2.ToString();
                QueueDeclareOk status = channel.QueueDeclare(queue, false, false, false, null);
                uint msgcount = status.MessageCount; //result.MessageCount;

                if(msgcount==0){
                    //enviar mensagem  de join e passar para jogo
                    var body = Encoding.UTF8.GetBytes(b.Label+"_"+Nickname);
                    channel.BasicPublish("", queue, null, body);
                    //fazer restart da tag desta slot
                    int numerojogador = numerojogador2 - 1;
                    queue = "Tag" + numerojogador;
                    while((channel.BasicGet(queue, true)) == null);
                    body = Encoding.UTF8.GetBytes("1");
                    channel.BasicPublish("", queue, null, body);

                    FrameUtils.RequestStartGame(numerojogador2, factory.HostName, factory.UserName, factory.Password, factory.Port);
                    YouNavigation.requestFrameChange(this, "YouTicTacToe2Screens");
                }
                else {
                    msgb.Text = "Someone entered that game first!";
                }
                
            }
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {
            YouNavigation.requestFrameChange(this, "YouTicTacToe");
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
            return TicTacToeLobbyRegion;
        }

        #endregion

        

       
    }
}
