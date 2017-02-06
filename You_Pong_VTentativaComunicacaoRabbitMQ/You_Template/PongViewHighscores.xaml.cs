using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;

namespace You_Pong
{
    /// <summary>
    /// Interaction logic for PongViewHighscores.xaml
    /// </summary>
    public partial class PongViewHighscores : Page, YouPlugin
    {
        private double h, w;

        public PongViewHighscores()
        {
            InitializeComponent();
            KinectApi.bindRegion(YouPongViewHighscoresRegion);
            SetWindow();
            this.Loaded += PongViewHighscores_Loaded;

        }

        void PongViewHighscores_Loaded(object sender, RoutedEventArgs e)
        {
            //deleteSources();
            refreshSources();
        }
        /*
        private void deleteSources()
        {
            foreach (var v in YouPongCanvas.Children)
            {
                if (v.GetType() == typeof(Image))
                {
                    var i = (Image)v;
                    if (i.Name.EndsWith("lugar"))
                    {
                        i.Source = null;

                    }

                }
            }
        }


        private void updatefiles(int x)
        {
            if (File.Exists("copy" + x + ".png")) ;
            File.Delete("copy" + x + ".png");
            File.Copy("Pong" + x + ".png", "copy" + x + ".png");
        }
        */
        private void refreshSources()
        {


            foreach (var v in YouPongCanvas.Children)
            {
                if (v.GetType() == typeof(Image))
                {
                    var i = (Image)v;
                    if (i.Name.EndsWith("lugar"))
                    {
                        switch (i.Name.Remove(i.Name.Length - 5))
                        {
                            case "primeiro":
                                {
                                    //updatefiles(1);
                                    i.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Pong1.png"));
                                    break;
                                }
                            case "segundo":
                                {
                                   // updatefiles(2);

                                    i.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Pong2.png"));
                                    break;
                                }
                            case "terceiro":
                                {
                                   // updatefiles(3);
                                    i.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Pong3.png"));
                                    break;
                                }
                            case "quarto":
                                {
                                  //  updatefiles(4);
                                    i.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Pong4.png"));
                                    break;
                                }
                            case "quinto":
                                {
                                 //   updatefiles(5);
                                    i.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Pong5.png"));
                                    break;
                                }
                        }

                    }

                }
            }

        }

        private void SetWindow()
        {
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            //title
            Canvas.SetTop(ponghighscores, h * 0.05);
            Canvas.SetLeft(ponghighscores, w * 0.35);

            //return button
            main.Width = w * 0.11;
            main.Height = h * 0.22;
            Canvas.SetTop(main, h * 0.01);
            Canvas.SetLeft(main, w * 0.01);

            //set das posicoes dos highscores 
            double padding = 0.15;
            TextBlock[] array = { second, third, fourth, fifth };
            TextBlock[] scores = {score2, score3, score4, score5};

            Canvas.SetTop(first, h * 0.2);
            Canvas.SetLeft(first, w * 0.32);
            Canvas.SetTop(score1,h*0.25);
            Canvas.SetLeft(score1,w*0.3);
            score1.Text = String.Format("{0:0.00}", FrameUtils.getHighscore(1))+" s";
            foreach (var t in array)
            {
                Canvas.SetLeft(t, w * padding);
                Canvas.SetTop(t, h * 0.65);
                padding += 0.2;
            }
            padding = 0.2;
            int i = 2;
            foreach (var s in scores)
            {
                Canvas.SetLeft(s,w*padding);
                Canvas.SetTop(s,h*0.65);
                s.Text = String.Format("{0:0.00}", FrameUtils.getHighscore(i)) + " s";
                padding += 0.2;
                i++;
            }



            //imagens dos highscores
            primeirolugar.Width = h * 0.5;
            primeirolugar.Height = h * 0.5;
            Canvas.SetTop(primeirolugar, h * 0.1);
            Canvas.SetLeft(primeirolugar, w * 0.40);
            
            Image[] array2 = { segundolugar, terceirolugar, quartolugar, quintolugar };
            i = 2;
            padding = 0.15;
            foreach (var t in array2)
            {
                t.Width = h * 0.3;
                t.Height = h * 0.3;
                Canvas.SetTop(t, h * 0.7);
                Canvas.SetLeft(t, w * padding);

                padding += 0.20;
                i++;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            if (b.Name == "main")
            {
                YouNavigation.requestFrameChange(this, "YouPong");
            }

        }
        private void Button_GripEvent(object sender, HandPointerEventArgs handPointerEventArgs)
        {
            var b = (YouButton)sender;
            if (b.Name == "main")
            {
                YouNavigation.requestFrameChange(this, "YouPong");
            }
        }

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_Pong";
        }
        // To identify the main page of the Plugin
        public bool getIsFirstPage()
        {
            return true;
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
            return YouPongViewHighscoresRegion;
        }

        #endregion

    }
}
