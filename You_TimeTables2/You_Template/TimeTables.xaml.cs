using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;

namespace You_TimeTables
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>
    public partial class TimeTables : Page, YouPlugin
    {
        private double w, h;
        private int curso;

        public TimeTables()
        {
            InitializeComponent();
            KinectApi.bindRegion(YouTimeTablesRegion);

            setWindow();
            setImages();
        }

        public void setImages()
        {
            //ect
            BitmapImage bitmapECT = new BitmapImage();
            Image imgECT = new Image();
            bitmapECT.BeginInit();
            bitmapECT.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/ect.png", UriKind.Relative);
            bitmapECT.EndInit();
            imgECT.Stretch = Stretch.Fill;
            imgECT.Source = bitmapECT;
            Ect.Content = imgECT;
            Ect.Label = null;
            Ect.Background = new ImageBrush(bitmapECT);

            //eet
            BitmapImage bitmapEET = new BitmapImage();
            Image imgEET = new Image();
            bitmapEET.BeginInit();
            bitmapEET.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/eet.png", UriKind.Relative);
            bitmapEET.EndInit();
            imgEET.Stretch = Stretch.Fill;
            imgEET.Source = bitmapEET;
            Eet.Content = imgEET;
            Eet.Label = null;
            Eet.Background = new ImageBrush(bitmapEET);

            //tsi
            BitmapImage bitmapTSI = new BitmapImage();
            Image imgTSI = new Image();
            bitmapTSI.BeginInit();
            bitmapTSI.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/tsi.png", UriKind.Relative);
            bitmapTSI.EndInit();
            imgTSI.Stretch = Stretch.Fill;
            imgTSI.Source = bitmapTSI;
            Tsi.Content = imgTSI;
            Tsi.Label = null;
            Tsi.Background = new ImageBrush(bitmapTSI);

            //titulo
            BitmapImage bitmapT = new BitmapImage();
            Image imgT = new Image();
            bitmapT.BeginInit();
            bitmapT.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/horarios.png", UriKind.Relative);
            bitmapT.EndInit();
            imgT.Stretch = Stretch.Fill;
            imgT.Source = bitmapT;
            Titulo.Stretch = Stretch.Fill;
            Titulo.Source = bitmapT;

            Titulo.Width = w * 0.6;
            Titulo.Height = h * 0.25;
            Canvas.SetTop(Titulo, h * 0.05);
            Canvas.SetLeft(Titulo, w * 0.5 - Titulo.Width * 0.5);
        }

        public void setWindow()
        {
            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Back Button
            MainMenuButton.Width = w * 0.11;
            MainMenuButton.Height = h * 0.22;
            Canvas.SetTop(MainMenuButton, h * 0.01);
            Canvas.SetLeft(MainMenuButton, w * 0.01);

            //Botão ect
            Ect.Width = w * 0.25;
            Ect.Height = h * 0.35;
            Canvas.SetTop(Ect, h * 0.38);
            Canvas.SetLeft(Ect, w * 0.07);
            //Botão eet
            Eet.Width = w * 0.25;
            Eet.Height = h * 0.35;
            Canvas.SetTop(Eet, h * 0.38);
            Canvas.SetLeft(Eet, w * 0.37);
            //Botão tsi
            Tsi.Width = w * 0.25;
            Tsi.Height = h * 0.35;
            Canvas.SetTop(Tsi, h * 0.38);
            Canvas.SetLeft(Tsi, w * 0.67);

        }

        public int getCurso()
        {
            return curso;
        }


        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_TimeTables";
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
            return YouTimeTablesRegion;
        }

        #endregion

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            click(b.Name);

        }

        private void click(string name)
        {
            switch (name)
            {

                case "MainMenuButton":
                    {
                        YouNavigation.navigateToMainMenu(this);
                        break;
                    }
                case "Ect":
                    {
                        curso = 0;
                        MyCurso.setActiveCurso(0);
                        //MainWindow.window.timeTables2.startUp(0);
                        YouNavigation.requestFrameChange(this, "YouTimeTables2");
                        break;
                    }
                case "Eet":
                    {
                        curso = 1;
                        MyCurso.setActiveCurso(1);
                        //MainWindow.window.timeTables2.startUp(1);
                        YouNavigation.requestFrameChange(this, "YouTimeTables2");
                        break;
                    }
                case "Tsi":
                    {
                        curso = 2;
                        MyCurso.setActiveCurso(2);
                        //MainWindow.window.timeTables2.startUp(2);
                        YouNavigation.requestFrameChange(this, "YouTimeTables2");
                        break;
                    }

            }
        }

        private void Button_GripEvent(object sender, HandPointerEventArgs e)
        {
            var b = (YouButton)sender;
            click(b.Name);
        }

        #endregion
    }
}
