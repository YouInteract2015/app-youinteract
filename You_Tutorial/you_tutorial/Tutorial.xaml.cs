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
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouPlugin_Developing;

namespace You_Tutorial
{
    /// <summary>
    /// Interaction logic for Tutorial.xaml
    /// </summary>
    public partial class Tutorial : Page, YouPlugin
    {
        private double w, h;
        private int i;
        DispatcherTimer dispatcherTimer;

        public Tutorial()
        {
            InitializeComponent();
            KinectApi.bindRegion(YouTutorialRegion);
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);
            this.Loaded += Tutorial_Loaded;
        }

        void Tutorial_Loaded(object sender, RoutedEventArgs e)
        {
            SetUp();
        }

        private void SetUp()
        {
            Console.WriteLine("DA MERDA AQUI?");
            w = YouWindow.getWidth();
            h = YouWindow.getHeight();

            TextGrab.Visibility = Visibility.Hidden;
            GrabButton.Visibility = Visibility.Hidden;
            Scroll.Visibility = Visibility.Hidden;
            TextScroll.Visibility = Visibility.Hidden;

            // Main
            MainMenuButton.Width = w * 0.11;
            MainMenuButton.Height = h * 0.22;
            Canvas.SetTop(MainMenuButton, h * 0.01);
            Canvas.SetLeft(MainMenuButton, w * 0.01);

            // Press Button
            PressButton.Visibility = Visibility.Visible;
            PressButton.ClearValue(Button.BackgroundProperty);
            PressButton.Label = "Pressiona-me!";
            PressButton.Height = h * 0.28;
            PressButton.Width = w * 0.16;
            Canvas.SetTop(PressButton, h * 0.2);
            Canvas.SetLeft(PressButton, w * 0.7);

            PanelPress.Width = w * 0.55;
            PanelPress.Height = h * 0.2;
            TextPress.Text = "Após colocares a mão por cima de um botão podes\npressionar em direção ao ecrã para clicar";
            Canvas.SetTop(PanelPress, h * 0.2);
            Canvas.SetLeft(PanelPress, w * 0.1);

            // Grab Button
            GrabButton.ClearValue(Button.BackgroundProperty);
            GrabButton.Label = "Agarra-me!";
            GrabButton.Height = h * 0.28;
            GrabButton.Width = w * 0.16;
            Canvas.SetTop(GrabButton, h * 0.2);
            Canvas.SetLeft(GrabButton, w * 0.7);

            PanelGrab.Width = w * 0.55;
            PanelGrab.Height = h * 0.2;
            TextGrab.Text = "Também poderá fechar a mão em cima do botão para\nclicar e poder aceder ao conteúdo do botão";
            Canvas.SetTop(PanelGrab, h * 0.4);
            Canvas.SetLeft(PanelGrab, w * 0.1);

            // Scroll
            Scroll.Height = h * 0.5;
            Scroll.Width = w * 0.15;
            Canvas.SetTop(Scroll, h * 0.5);
            Canvas.SetLeft(Scroll, w * 0.7);

            PanelScroll.Width = w * 0.55;
            PanelScroll.Height = h * 0.2;
            TextScroll.Text = "Para fazer scroll, deixar a mão fechada e arrastar\nem cima do painel \n\nFim do Tutorial";
            Canvas.SetTop(PanelScroll, h * 0.6);
            Canvas.SetLeft(PanelScroll, w * 0.1);

            for (i = 0; i < 10; i++)
            {

                var button = new KinectTileButton { Label = "Content" };
                button.Background = new SolidColorBrush(Colors.LightSeaGreen);
                button.Width = w * 0.15;
                button.Height = h * 0.2;
                button.Name = "content" + (i + 1).ToString();
                this.WrapScrollPanel.Children.Add(button);
            }
        }

        #region YourPlugin Interface Methods
        public string getAppName()
        {
            return "You_Tutorial";
        }

        public KinectRequirements getKinectRequirements()
        {
            return new KinectRequirements(true, false, false);
        }

        public string getName()
        {
            return this.Name;
        }

        public Page getPage()
        {
            return this;
        }

        public KinectRegion getRegion()
        {
            return YouTutorialRegion;
        }

        public bool getIsFirstPage()
        {
            return true;
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            YouNavigation.navigateToMainMenu(this);
        }

        private void Button_GripEvent(object sender, RoutedEventArgs e)
        {
            YouNavigation.navigateToMainMenu(this);
        }

        private void Press(object sender, RoutedEventArgs e)
        {
            PressButton.Label = "Congratulations";
            PressButton.Background = System.Windows.Media.Brushes.Green;
            dispatcherTimer.Start();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick_Press);
        }

        private void dispatcherTimer_Tick_Press(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            PressButton.Visibility = Visibility.Hidden;
            GrabButton.Visibility = Visibility.Visible;
            TextGrab.Visibility = Visibility.Visible;
        }


        private void dispatcherTimer_Tick_Grab(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            Scroll.Visibility = Visibility.Visible;
            TextScroll.Visibility = Visibility.Visible;

        }

        private void Button_GripEventTutorial(object sender, RoutedEventArgs e)
        {
            GrabButton.Label = "Congratulations";
            GrabButton.Background = Brushes.Green;
            dispatcherTimer.Start();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick_Grab);
        }
    }
}
