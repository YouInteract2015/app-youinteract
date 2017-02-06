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

namespace You_Pong
{
    /// <summary>
    /// Interaction logic for PongPause.xaml
    /// </summary>
    /// 

    // Para guardar estado do jogo
    public partial class PongPause2 : Page, YouPlugin
    {
        private double w, h;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
      

        public PongPause2()
        {
            InitializeComponent();
            w = YouWindow.getWidth();
            h = YouWindow.getHeight();
            KinectApi.bindRegion(YouPongPauseRegion);
            

            // pauseMsg
            Canvas.SetTop(pauseblock, h * 0.1);
            Canvas.SetLeft(pauseblock, w * 0.35);


        }

        private void YouPongStandby_Loaded(object sender, RoutedEventArgs e)
        {
            //Criar conexao
            factory = new ConnectionFactory();
            factory.UserName = "teste";
            factory.Password = "teste";
            factory.HostName = "192.168.1.4";
            connection = factory.CreateConnection();

            //criar um canal
            channel = connection.CreateModel();
            Console.WriteLine("Criou conexão");

            //criar filas para envio e recepcao de das coordenadas das raquetes
            channel.QueueDeclare("Start1", false, false, false, null);
            channel.QueueDeclare("Start2", false, false, false, null);
            channel.QueueDeclare("Pause1", false, false, false, null);
            channel.QueueDeclare("Pause2", false, false, false, null);
            channel.QueueDeclare("Screen1", false, false, false, null);
            channel.QueueDeclare("Screen2", false, false, false, null);


            Console.WriteLine("Criou queues");

            //Enviar mensagem ao adversario a informar que esta pronto
            var body = Encoding.UTF8.GetBytes("Ready");

            Console.WriteLine("Fez encoding");

            //Limpar as queue de mensagens que possam ainda la estar de sessoes anteriores
            channel.QueuePurge("Start2");

            Console.WriteLine("fez purge");
            //enviar mensagem
            channel.BasicPublish("", "Start2", null, body);

            Console.WriteLine("publicou mensagem");


            Console.WriteLine("Vai comecar a pedir a mensagem");
            //esperar pela mensagem de prontidao do adversario
            RabbitMQ.Client.BasicGetResult message;
            while ((message = channel.BasicGet("Start1", true)) == null) {
                //Console.WriteLine("A espera do outro jogador");
            }

            Console.WriteLine("JOGO INICIADO");

            //iniciar jogo
            FrameUtils.requestRestart("1p");
            YouNavigation.requestFrameChange(this, "YouPong2Screens");

            

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
            return YouPongPauseRegion;
        }

        #endregion

       
    }
}
