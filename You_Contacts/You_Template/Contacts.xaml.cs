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
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;
using You_Contacts.Loader;
using You_Contacts.Teachers_WS;

namespace You_Contacts
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>
    public partial class Contacts : Page, YouPlugin
    {
        public static readonly DependencyProperty PageLeftEnabledProperty = DependencyProperty.Register(
            "PageLeftEnabled", typeof(bool), typeof(Contacts), new PropertyMetadata(false));

        public static readonly DependencyProperty PageRightEnabledProperty = DependencyProperty.Register(
            "PageRightEnabled", typeof(bool), typeof(Contacts), new PropertyMetadata(false));


        private const double ScrollErrorMargin = 0.001;
        private const int PixelScrollByAmount = 15;
        private int i;
        private double w, h;
        private Prof[] docentes;

        private Image imgProfessor;
        private Viewbox PanelTeachers;
        private TextBlock TeachersDescription;

        public YouLoader youLoader;

        public Contacts()
        {
            InitializeComponent();
            youLoader = new YouLoader();
            imgProfessor = new Image();

            KinectApi.bindRegion(YouContactsRegion);

            DownloadTeachers download = new DownloadTeachers();
            download.download();
            docentes = new Prof[DownloadTeachers.teacherList.Count];

            setWindow();
        }

        public void setWindow()
        {
            // Get Window Measures
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();

            // Set Title
            BitmapImage bitmapT = new BitmapImage();
            Image imgT = new Image();
            bitmapT.BeginInit();
            bitmapT.UriSource = new Uri("/You_Contacts;component/Images/Themes/Theme1/Contacts/contactos.png", UriKind.Relative);
            bitmapT.EndInit();
            imgT.Stretch = Stretch.Fill;
            imgT.Source = bitmapT;
            Titulo.Stretch = Stretch.Fill;
            Titulo.Source = bitmapT;

            Titulo.Width = w * 0.6;
            Titulo.Height = h * 0.25;
            Canvas.SetTop(Titulo, h * 0);
            Canvas.SetLeft(Titulo, w * 0.5 - Titulo.Width * 0.5);

            // Set Scroll Arrows
            YouWindow.setScrollArrows(ScrollLeft, ScrollRight, 0.65, 0.1, 0.23, 0, 0.23, 0.9);

            // Scroll Panel
            WrapScrollPanel.Height = h * 0.63;
            //wrapScrollPanel.Width = w;
            Canvas.SetTop(WrapScrollPanel, h * 0.27);
            Canvas.SetLeft(WrapScrollPanel, 0);

            // scrollViewer
            ScrollViewer.HoverBackground = Brushes.Transparent;
            ScrollViewer.Height = h * 0.63;
            ScrollViewer.Width = w;
            Canvas.SetTop(ScrollViewer, h * 0.26);
            Canvas.SetLeft(ScrollViewer, 0);

            // Back Button
            MainMenuButton.Width = w * 0.11;
            MainMenuButton.Height = h * 0.22;
            Canvas.SetTop(MainMenuButton, h * 0.01);
            Canvas.SetLeft(MainMenuButton, w * 0.01);

            /*
            // Fill contacts:
            DownloadTeachers download = new DownloadTeachers();
            download.download();
            docentes = new Prof[DownloadTeachers.teacherList.Count];
            */
            
            for (i = 0; i < DownloadTeachers.teacherList.Count; i++)
            {
                // Image:
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                string imageURI = DownloadTeachers.teacherList.ElementAt(i).Name.Replace(" ", "") + ".jpg";
                bi.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + "/App/You_Contacts/Teachers/" + imageURI, UriKind.Relative);
                bi.EndInit();

                // AlfredoMiguelMeloMatos

                // Fill in Professor ARRAY
                docentes[i] = new Prof(i, DownloadTeachers.teacherList.ElementAt(i).Name, DownloadTeachers.teacherList.ElementAt(i).Office, DownloadTeachers.teacherList.ElementAt(i).PhoneExt, bi);

                // Add Button to Wrap Panel
                var button = new YouButton { Background = new ImageBrush(bi) };
                button.Width = w * 0.13;
                button.Height = h * 0.21;
                button.Name = "Teacher" + docentes[i].getId();
                this.WrapScrollPanel.Children.Add(button);
                button.Margin = new Thickness(0, 0, w * 0.03, 0);
                button.EnterEvent += new onHandEnterHandler(Button_Hover_Event);
                button.LeaveEvent += new onhandLeaveHandler(Button_Leave_Event);

                // Text Box to show Professor name
                var PanelName = new Viewbox();
                var TeachersName = new TextBlock();

                PanelName.Stretch = Stretch.Uniform;
                TeachersName.TextWrapping = TextWrapping.Wrap;
                TeachersName.TextTrimming = TextTrimming.CharacterEllipsis;
                TeachersName.FontSize = 20;

                // Add Text Box to Wrap Panel
                PanelName.Child = TeachersName;
                PanelName.Width = w * 0.14;
                PanelName.Height = h * 0.03;
                TeachersName.Text = DownloadTeachers.teacherList.ElementAt(i).Name;
                this.WrapScrollPanel.Children.Add(PanelName);
                PanelName.Margin = new Thickness(0, 0, w * 0.03, 0);

            }
             
            // Hover Panel
            PanelTeachers = new Viewbox();
            TeachersDescription = new TextBlock();

            PanelTeachers.Stretch = Stretch.Uniform;
            PanelTeachers.Child = TeachersDescription;
            TeachersDescription.TextWrapping = TextWrapping.Wrap;
            TeachersDescription.TextTrimming = TextTrimming.CharacterEllipsis;
            TeachersDescription.FontSize = 20;
            TeachersDescription.Text = "Passe a mão por cima do professor para ver detalhes";

            PanelTeachers.Width = w * 0.55;
            PanelTeachers.Height = h * 0.2;

            imgProfessor.Stretch = Stretch.Fill;
            imgProfessor.Visibility = Visibility.Visible;
            imgProfessor.Width = w * 0.13;
            imgProfessor.Height = h * 0.24;

            // Add to Display
            this.YouContactsCanvas.Children.Add(imgProfessor);
            this.YouContactsCanvas.Children.Add(PanelTeachers);

            Canvas.SetLeft(imgProfessor, w * 0.2);
            Canvas.SetBottom(imgProfessor, h * 0.015);
            Canvas.SetBottom(PanelTeachers, h * 0.03);
            Canvas.SetLeft(PanelTeachers, w * 0.4);
        }

        public void myHoverHandler(int id)
        {
            // Not Hover
            if (id == -1)
            {
                imgProfessor.Visibility = Visibility.Hidden;
                TeachersDescription.Text = "Mão em cima do professor para ver os seus detalhes";
            }
            // Hover
            else
            {
                imgProfessor.Visibility = Visibility.Visible;
                imgProfessor.Source = docentes[id].getImage();
                TeachersDescription.Text = docentes[id].getName() + "\n" + "Office: " + docentes[id].getOffice() + "\n" + "Phone: " + docentes[id].getPhone();
            }
        }

        #region YourPlugin Interface Methods

        // Name of the app and namespace. MUST start by "You_*"
        public string getAppName()
        {
            return "You_Contacts";
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
            return YouContactsRegion;
        }

        #endregion


        #region Scroller Controllers
        public bool PageLeftEnabled
        {
            get
            {
                return (bool)GetValue(PageLeftEnabledProperty);
            }

            set
            {
                this.SetValue(PageLeftEnabledProperty, value);
            }
        }
        public bool PageRightEnabled
        {
            get
            {
                return (bool)GetValue(PageRightEnabledProperty);
            }

            set
            {
                this.SetValue(PageRightEnabledProperty, value);
            }
        }

        private void UpdatePagingButtonState()
        {
            this.PageLeftEnabled = ScrollViewer.HorizontalOffset > ScrollErrorMargin;
            this.PageRightEnabled = ScrollViewer.HorizontalOffset < ScrollViewer.ScrollableWidth - ScrollErrorMargin;
        }
        private void PageRightButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + PixelScrollByAmount);
        }
        private void PageLeftButtonClick(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - PixelScrollByAmount);
        }
        #endregion

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            if (b.Name == "MainMenuButton")
            {
                YouNavigation.navigateToMainMenu(this);
            }   
        }

        

        private void Button_Leave_Event(object sender, HandPointerEventArgs e)
        {
            myHoverHandler(-1);
        }

        private void Button_Hover_Event(object sender, HandPointerEventArgs e)
        {

            var b = (YouButton)e.OriginalSource;
            if (b.Name.Contains("Teacher"))
            {
                string teacherNumber = "";
                char[] myChar = b.Name.ToCharArray();
                
                foreach (char ch in myChar)
                {
                    if (char.IsDigit(ch))
                    {
                        teacherNumber += ch.ToString();
                    }
                }
                int teacherID = Convert.ToInt32(teacherNumber);
                myHoverHandler(teacherID);    
            }
        }

        private void Button_GripEvent(object sender, HandPointerEventArgs e)
        {
            var b = (YouButton)sender;
            if (b.Name == "MainMenuButton")
            {
                YouNavigation.navigateToMainMenu(this);
            }   
        }

        #endregion

        
    }
}
