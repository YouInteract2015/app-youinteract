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
    /// Interaction logic for TicTacToe1Player.xaml
    /// </summary>
    public partial class TicTacToe1Player : Page,YouPlugin
    {
       private double w, h;
       private string player, oponent;
        private bool isPlayerTurn,gameFinished;
        
        private int playCount;

        public TicTacToe1Player()
        {
            InitializeComponent();
            //Console.WriteLine("Construtor Jogo do galo chamado!");
            w = YouWindow.getWidth();
            h = YouWindow.getHeight();
            
            KinectApi.bindRegion(TicTacToe1PlayerRegion);


            // When Load, call TicTacToe2Screens_Loaded
            this.Loaded += TicTacToe1Player_Loaded;
            this.Unloaded += TicTacToe1Player_Unloaded;
           

            setUpGame();

        }
    
        public void TicTacToe1Player_Loaded(object sender, RoutedEventArgs e)
        {
            player = "X";
            oponent = "O";
            FrameUtils_Restart();
            // Sets 1 palyer game
            FrameUtils.SetMode("1p");
            
  
                playCount = 0;
                gameFinished = false;
                isPlayerTurn = true;
                msgb.Text = "Let's play Tic Tac Toe!!";
            
        }
        public void TicTacToe1Player_Unloaded(object sender, RoutedEventArgs e) {
              
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
    
        
    
        //funcao chamada quando um botao da grelha e ativado
        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            var b = (KinectTileButton)e.OriginalSource;
            if(isPlayerTurn && b.Label.Equals("") && !gameFinished){
                
                //bloquear botao e colocar marca do jogador
                b.Foreground = Brushes.Blue;
                b.Label = player;
                
                //Console.WriteLine("Button pressed!"+b.Name.ToString());
                //b.IsEnabled = false;
                playCount++;    //numero de jogadas incrementado


                //terminar turno do jogador
                isPlayerTurn = false;   

                //verificar vencedor
                checkWinner();

                if(!gameFinished){
                    //jogada do computador
                    CPUTurn();
                    //verificar vencedor
                    checkWinner();
                } 
            }    
        }

        //jogada do computador
        private void CPUTurn() {

            //o computador verifica se pode vencer
            FillSeries(oponent);

            if(!gameFinished && !isPlayerTurn){
                //o computador verifica se o jogador esta a uma jogada de vencer
                FillSeries(player);
            }

            if (!gameFinished && !isPlayerTurn)
            {
                //verificar se o espaco do meio esta vazio
                if (Botao5.Label.Equals(""))
                {
                    Botao5.Label = oponent;
                    Botao5.Foreground = Brushes.Red;
                    isPlayerTurn = true;
                    playCount++;
                }
                //coluna 1
                else if (Botao1.Label.Equals("") && (Botao2.Label.Equals(player) || Botao2.Label.Equals("")) && (Botao3.Label.Equals(player) || Botao3.Label.Equals("")))
                {
                    Botao1.Label = oponent;
                    Botao1.Foreground = Brushes.Red;
                    isPlayerTurn = true;
                    playCount++;
                }
                else if (Botao3.Label.Equals("") && (Botao2.Label.Equals(player) || Botao2.Label.Equals("")) && (Botao1.Label.Equals(player) || Botao1.Label.Equals("")))
                {
                    Botao3.Label = oponent;
                    Botao3.Foreground = Brushes.Red;
                    isPlayerTurn = true;
                    playCount++;
                }

                //coluna 2
                else if (Botao4.Label.Equals("") && (Botao5.Label.Equals(player) || Botao5.Label.Equals("")) && (Botao6.Label.Equals(player) || Botao6.Label.Equals("")))
                {
                    Botao4.Label = oponent;
                    Botao4.Foreground = Brushes.Red;
                    isPlayerTurn = true;
                    playCount++;
                }
                else if (Botao6.Label.Equals("") && (Botao5.Label.Equals(player) || Botao5.Label.Equals("")) && (Botao4.Label.Equals(player) || Botao4.Label.Equals("")))
                {
                    Botao6.Label = oponent;
                    Botao6.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                //coluna 3
                else if (Botao7.Label.Equals("") && (Botao8.Label.Equals(player) || Botao8.Label.Equals("")) && (Botao9.Label.Equals(player) || Botao9.Label.Equals("")))
                {
                    Botao7.Label = oponent;
                    Botao7.Foreground = Brushes.Red;
                    isPlayerTurn = true;
                    playCount++;
                }
                else if (Botao9.Label.Equals("") && (Botao8.Label.Equals(player) || Botao8.Label.Equals("")) && (Botao7.Label.Equals(player) || Botao7.Label.Equals("")))
                {
                    Botao9.Label = oponent;
                    Botao9.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao1.Label.Equals(""))
                {
                    Botao1.Label = oponent;
                    Botao1.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao2.Label.Equals(""))
                {
                    Botao2.Label = oponent;
                    Botao2.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao3.Label.Equals(""))
                {
                    Botao3.Label = oponent;
                    Botao3.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao4.Label.Equals(""))
                {
                    Botao4.Label = oponent;
                    Botao4.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao5.Label.Equals(""))
                {
                    Botao5.Label = oponent;
                    Botao5.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao6.Label.Equals(""))
                {
                    Botao6.Label = oponent;
                    Botao6.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao7.Label.Equals(""))
                {
                    Botao7.Label = oponent;
                    Botao7.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao8.Label.Equals(""))
                {
                    Botao8.Label = oponent;
                    Botao8.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                else if (Botao9.Label.Equals(""))
                {
                    Botao9.Label = oponent;
                    Botao9.Foreground = Brushes.Red;
                    playCount++;
                    isPlayerTurn = true;
                }
                
            }
            
            
        }

        //o computador verifica alguma sequencia em que apenas falte um elemento
        private void FillSeries(string p){
            //coluna 1
            if (Botao1.Label.Equals(p) && Botao2.Label.Equals(p) && Botao3.Label.Equals(""))
            {
                Botao3.Label = oponent;
                Botao3.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;

            }
            else if (Botao1.Label.Equals(p) && Botao3.Label.Equals(p) && Botao2.Label.Equals(""))
            {
                Botao2.Label = oponent;
                Botao2.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao2.Label.Equals(p) && Botao3.Label.Equals(p) && Botao1.Label.Equals(""))
            {
                Botao1.Label = oponent;
                Botao1.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }

            //coluna 2
            else if (Botao4.Label.Equals(p) && Botao5.Label.Equals(p) && Botao6.Label.Equals(""))
            {
                Botao6.Label = oponent;
                Botao6.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao4.Label.Equals(p) && Botao6.Label.Equals(p) && Botao5.Label.Equals(""))
            {
                Botao5.Label = oponent;
                Botao5.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao5.Label.Equals(p) && Botao6.Label.Equals(p) && Botao4.Label.Equals(""))
            {
                Botao4.Label = oponent;
                Botao4.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }

            //coluna 3
            else if (Botao7.Label.Equals(p) && Botao8.Label.Equals(p) && Botao9.Label.Equals(""))
            {
                Botao9.Label = oponent;
                Botao9.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao7.Label.Equals(p) && Botao9.Label.Equals(p) && Botao8.Label.Equals(""))
            {
                Botao8.Label = oponent;
                Botao8.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao8.Label.Equals(p) && Botao9.Label.Equals(p) && Botao7.Label.Equals(""))
            {
                Botao7.Label = oponent;
                Botao7.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }

            //linha 1
            else if (Botao1.Label.Equals(p) && Botao4.Label.Equals(p) && Botao7.Label.Equals(""))
            {
                Botao7.Label = oponent;
                Botao7.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao1.Label.Equals(p) && Botao7.Label.Equals(p) && Botao4.Label.Equals(""))
            {
                Botao4.Label = oponent;
                Botao4.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao7.Label.Equals(p) && Botao4.Label.Equals(p) && Botao1.Label.Equals(""))
            {
                Botao1.Label = oponent;
                Botao1.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }

             //linha 2
            else if (Botao2.Label.Equals(p) && Botao5.Label.Equals(p) && Botao8.Label.Equals(""))
            {
                Botao8.Label = oponent;
                Botao8.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao2.Label.Equals(p) && Botao8.Label.Equals(p) && Botao5.Label.Equals(""))
            {
                Botao5.Label = oponent;
                Botao5.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao5.Label.Equals(p) && Botao8.Label.Equals(p) && Botao2.Label.Equals(""))
            {
                Botao2.Label = oponent;
                Botao2.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }

             //linha 3
            else if (Botao3.Label.Equals(p) && Botao6.Label.Equals(p) && Botao9.Label.Equals(""))
            {
                Botao9.Label = oponent;
                Botao9.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao3.Label.Equals(p) && Botao9.Label.Equals(p) && Botao6.Label.Equals(""))
            {
                Botao6.Label = oponent;
                Botao6.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao9.Label.Equals(p) && Botao6.Label.Equals(p) && Botao3.Label.Equals(""))
            {
                Botao3.Label = oponent;
                Botao3.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }

             //diagonal esquerda-direita
            else if (Botao1.Label.Equals(p) && Botao5.Label.Equals(p) && Botao9.Label.Equals(""))
            {
                Botao9.Label = oponent;
                Botao9.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao1.Label.Equals(p) && Botao9.Label.Equals(p) && Botao5.Label.Equals(""))
            {
                Botao5.Label = oponent;
                Botao5.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao9.Label.Equals(p) && Botao5.Label.Equals(p) && Botao1.Label.Equals(""))
            {
                Botao1.Label = oponent;
                Botao1.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }

            //diagonal direita-esquerda
            else if (Botao3.Label.Equals(p) && Botao5.Label.Equals(p) && Botao7.Label.Equals(""))
            {
                Botao7.Label = oponent;
                Botao7.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao3.Label.Equals(p) && Botao7.Label.Equals(p) && Botao5.Label.Equals(""))
            {
                Botao5.Label = oponent;
                Botao5.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
            }
            else if (Botao7.Label.Equals(p) && Botao5.Label.Equals(p) && Botao3.Label.Equals(""))
            {
                Botao3.Label = oponent;
                Botao3.Foreground = Brushes.Red;
                isPlayerTurn = true;
                playCount++;
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
           }
           else {
               isPlayerTurn = false;
              
           }
           msgb.Text = "Let's play Tic Tac Toe!!";
        }
           


        private void Options_Click(object sender, RoutedEventArgs e)
        {
            
                var b = (KinectTileButton)e.OriginalSource;

                if (b.Name.Equals("quitButton")) //botao de desistencia de jogo
                {
                    
                    YouNavigation.requestFrameChange(this, "YouTicTacToe");
                
                }
                else if (b.Name.Equals("restartButton")) //botao de reinicio de jogo
                {
                    FrameUtils_Restart();
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
            return TicTacToe1PlayerRegion;
        }

        #endregion

    }
 }

