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
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_Developing;
using You_TimeTables.Timetables_WS;

namespace You_TimeTables
{
    /// <summary>
    /// Interaction logic for TimeTables2.xaml
    /// </summary>
    public partial class TimeTables2 : Page, YouPlugin
    {
        private double w, h;

        private int curso;
        private int ano;
        private bool firstTime = true;
        private int semestre;
        private int[,] _timeSlot = new int[5, 30];

        TimetableParser _parser = new TimetableParser();

        private Color[] _color = new Color[] {Colors.Tomato,
                                              Colors.LightGreen,
                                              Colors.Violet,
                                              Colors.LightYellow,
                                              Colors.PaleTurquoise,
                                              Colors.SandyBrown,
                                              Colors.Gray,
                                              Colors.Coral,
                                              Colors.SkyBlue,
                                              Colors.CornflowerBlue};

        public TimeTables2()
        {
            InitializeComponent();

            imagem.Width = w * 0.1;
            imagem.Height = h * 0.1;
            Canvas.SetTop(imagem, h * -0.1);
            Canvas.SetLeft(imagem, w * -0.1);

            KinectApi.bindRegion(YouTimeTables2Region);

            setWindow();
            setImages();

            MyCurso.cursoActivation +=Curso_cursoActivation;

            _parser.Changed += new ProgressChangedEventHandler(Parser_Changed);
            _parser.Start();
            //está a começar sempre no no...
            _parser.TimeTable_Left();
        }

        private void Curso_cursoActivation(int e)
        {
            //_parser.SetCurrentSemester(1);
            if (e == 0)
            {
                _parser.TimeTable_MIECT();

            }
            else if (e == 1)
            {
                _parser.TimeTable_MIEET();
            }
            else
            {
                _parser.TimeTable_TSI();
            }
        }

        /*
        public void startUp(int curso)
        {
            //_parser.SetCurrentSemester(1);
            if (curso == 0)
            {
                _parser.TimeTable_MIECT();

            }
            else if (curso == 1)
            {
                _parser.TimeTable_MIEET();
            }
            else
            {
                _parser.TimeTable_TSI();
            }
        }*/

        private void Parser_Changed(object sender, ProgressChangedEventArgs e)
        {
            // Parser just started, set up timetable and display header

            if (e.ProgressPercentage == 0)
            {
                // Clear array to check for overlapping classes
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 30; j++)
                        _timeSlot[i, j] = 0;

                //(re)Set the timetable
                SetUpTimeTable();

                // Get the text for the timetable header
                string text = (string)e.UserState;

                // Create the textblock for the header
                Border b = new Border();
                b.BorderBrush = new SolidColorBrush(Colors.Transparent);
                b.BorderThickness = new Thickness(1.0);

                TextBlock titleblock = new TextBlock();
                titleblock.Text = text;

                titleblock.FontSize = 20;

                titleblock.Padding = new Thickness(0, 0, 0, 15);
                titleblock.Foreground = new SolidColorBrush(Colors.Black);
                titleblock.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./SegoeWP.ttf#Segoe WP");
                titleblock.TextAlignment = TextAlignment.Center;
                titleblock.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                b.Child = titleblock;

                Grid.SetColumn(b, 0);
                Grid.SetRow(b, 0);
                Grid.SetColumnSpan(b, 30);

                // Place textblock on timetable
                grTimeTable.Children.Add(b);
            }
            else
            {
                // Get the Timetable item from the event args
                TimetableItem ti = (TimetableItem)e.UserState;

                // Create a label to place the item on the timetable
                Border b = new Border();
                b.BorderBrush = new SolidColorBrush(Colors.Black);
                b.BorderThickness = new Thickness(1.0);
                b.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                b.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                TextBlock tb = new TextBlock();
                tb.Text = ti.Subject;
                tb.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./SegoeWP.ttf#Segoe WP");
                tb.Background = new SolidColorBrush(_color[ti.Color]);


                tb.Height = 17;
                tb.FontSize = 11;

                tb.Padding = new Thickness(5.0, 0.0, 5.0, 0.0);

                // Check for overlapping classes
                int val = 0;
                for (int i = ti.StartTime; i < ti.StartTime + ti.Duration; i++)
                {
                    if (_timeSlot[ti.Day - 2, i] >= val)
                        val = _timeSlot[ti.Day - 2, i];


                    b.Margin = new Thickness(0, val * 15, 0, 0);


                    _timeSlot[ti.Day - 2, i] += 1;
                }


                Grid.SetColumn(b, ti.StartTime);
                Grid.SetRow(b, ti.Day);
                Grid.SetColumnSpan(b, ti.Duration);
                b.Child = tb;

                // Position the lable on the timetable
                grTimeTable.Children.Add(b);

                grTimeTable.UpdateLayout();
            }
        }


        //*********************Novo************************************
        private void SetUpTimeTable()
        {
            // Clear any content currently on the grid.
            grTimeTable.Children.Clear();
            if (firstTime)
            {

                Canvas.SetTop(grTimeTable, h * 0.22);
                Canvas.SetLeft(grTimeTable, w * 0.22);
            }
            firstTime = false;

            Border b = new Border();
            TextBlock info = new TextBlock();
            info.Background = new LinearGradientBrush(Colors.DarkGray, Colors.DimGray, 90.0);
            b.BorderBrush = new SolidColorBrush(Colors.Black);
            info.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./SegoeWP.ttf#Segoe WP");
            b.BorderThickness = new Thickness(1.0);
            info.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            info.TextAlignment = TextAlignment.Center;

            info.Text = "Dia/Hora";
            info.FontSize = 14;
            b.Child = info;
            Grid.SetColumn(b, 0);
            Grid.SetRow(b, 1);
            Grid.SetColumnSpan(b, 2);
            grTimeTable.Children.Add(b);

            for (int i = 2; i < 7; i++)
            {
                Border border = new Border();
                TextBlock weekday = new TextBlock();
                weekday.Background = new LinearGradientBrush(Colors.DarkGray, Colors.DimGray, 90.0);
                border.BorderBrush = new SolidColorBrush(Colors.Black);
                weekday.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./SegoeWP.ttf#Segoe WP");
                border.BorderThickness = new Thickness(1.0);
                weekday.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                weekday.TextAlignment = TextAlignment.Center;
                weekday.FontSize = 14;

                weekday.Text = "" + i + "ª Feira";
                border.Child = weekday;
                Grid.SetColumn(border, 0);
                Grid.SetRow(border, i);
                Grid.SetColumnSpan(border, 2);
                grTimeTable.Children.Add(border);
            }

            for (int i = 1; i <= 12; i++)
            {
                Border border2 = new Border();
                TextBlock time = new TextBlock();
                time.Background = new LinearGradientBrush(Colors.DarkGray, Colors.DimGray, 90.0);
                border2.BorderBrush = new SolidColorBrush(Colors.Black);
                time.FontFamily = new FontFamily(new Uri("pack://application:,,,/Fonts/"), "./SegoeWP.ttf#Segoe WP");
                border2.BorderThickness = new Thickness(1.0);
                time.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                time.TextAlignment = TextAlignment.Left;
                time.FontSize = 14;

                time.Text = "" + (7 + i);
                border2.Child = time;
                Grid.SetColumn(border2, (i * 2));
                Grid.SetRow(border2, 1);
                Grid.SetColumnSpan(border2, 2);
                grTimeTable.Children.Add(border2);
            }
        }


        public void setWindow()
        {
            h = YouWindow.getHeight();
            w = YouWindow.getWidth();


            // Botão Main Menu
            MainMenuButton.Width = w * 0.11;
            MainMenuButton.Height = h * 0.2;
            Canvas.SetTop(MainMenuButton, h * 0.04);
            Canvas.SetLeft(MainMenuButton, w * 0.03);
            //Botão um
            um.Width = w * 0.11;
            um.Height = h * 0.2;
            Canvas.SetTop(um, h * 0.10);
            Canvas.SetLeft(um, w * 0.16);
            //Botão dois
            dois.Width = w * 0.11;
            dois.Height = h * 0.2;
            Canvas.SetTop(dois, h * 0.10);
            Canvas.SetLeft(dois, w * 0.30);
            //Botão tres
            tres.Width = w * 0.11;
            tres.Height = h * 0.2;
            Canvas.SetTop(tres, h * 0.10);
            Canvas.SetLeft(tres, w * 0.44);
            //Botão quatro
            quatro.Width = w * 0.11;
            quatro.Height = h * 0.2;
            Canvas.SetTop(quatro, h * 0.10);
            Canvas.SetLeft(quatro, w * 0.58);
            //Botão cinco
            cinco.Width = w * 0.11;
            cinco.Height = h * 0.2;
            Canvas.SetTop(cinco, h * 0.10);
            Canvas.SetLeft(cinco, w * 0.72);
            //Semestre
            //    primeiro.Width = w * 0.11;
            //    primeiro.Height = h * 0.2;
            //    Canvas.SetTop(primeiro, h * 0.62);
            //    Canvas.SetLeft(primeiro, w * 0.86);
            //Mudar o curso
            mudarCurso.Width = w * 0.11;
            mudarCurso.Height = h * 0.2;
            Canvas.SetTop(mudarCurso, h * 0.45);
            Canvas.SetLeft(mudarCurso, w * 0.86);

        }

        //botões janela
        public void setUm(int fed)
        {
            if (fed == 1)
            {
                BitmapImage bitmap1 = new BitmapImage();
                Image img1 = new Image();
                bitmap1.BeginInit();
                bitmap1.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao1fb.png", UriKind.Relative);
                bitmap1.EndInit();
                img1.Stretch = Stretch.Fill;
                img1.Source = bitmap1;
                um.Content = img1;
                um.Label = null;
                um.Background = new ImageBrush(bitmap1);
            }
            else
            {
                BitmapImage bitmap1 = new BitmapImage();
                Image img1 = new Image();
                bitmap1.BeginInit();
                bitmap1.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao1.png", UriKind.Relative);
                bitmap1.EndInit();
                img1.Stretch = Stretch.Fill;
                img1.Source = bitmap1;
                um.Content = img1;
                um.Label = null;
                um.Background = new ImageBrush(bitmap1);

            }
        }
        public void setDois(int fed)
        {

            if (fed == 1)
            {
                BitmapImage bitmap2 = new BitmapImage();
                Image img2 = new Image();
                bitmap2.BeginInit();
                bitmap2.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao2fb.png", UriKind.Relative);
                bitmap2.EndInit();
                img2.Stretch = Stretch.Fill;
                img2.Source = bitmap2;
                dois.Content = img2;
                dois.Label = null;
                dois.Background = new ImageBrush(bitmap2);
            }
            else
            {
                BitmapImage bitmap2 = new BitmapImage();
                Image img2 = new Image();
                bitmap2.BeginInit();
                bitmap2.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao2.png", UriKind.Relative);
                bitmap2.EndInit();
                img2.Stretch = Stretch.Fill;
                img2.Source = bitmap2;
                dois.Content = img2;
                dois.Label = null;
                dois.Background = new ImageBrush(bitmap2);
            }
        }
        public void setTres(int fed)
        {
            if (fed == 1)
            {
                BitmapImage bitmap3 = new BitmapImage();
                Image img3 = new Image();
                bitmap3.BeginInit();
                bitmap3.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao3fb.png", UriKind.Relative);
                bitmap3.EndInit();
                img3.Stretch = Stretch.Fill;
                img3.Source = bitmap3;
                tres.Content = img3;
                tres.Label = null;
                tres.Background = new ImageBrush(bitmap3);
            }
            else
            {
                BitmapImage bitmap3 = new BitmapImage();
                Image img3 = new Image();
                bitmap3.BeginInit();
                bitmap3.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao3.png", UriKind.Relative);
                bitmap3.EndInit();
                img3.Stretch = Stretch.Fill;
                img3.Source = bitmap3;
                tres.Content = img3;
                tres.Label = null;
                tres.Background = new ImageBrush(bitmap3);
            }
        }
        public void setQuatro(int fed)
        {
            if (fed == 1)
            {
                BitmapImage bitmap4 = new BitmapImage();
                Image img4 = new Image();
                bitmap4.BeginInit();
                bitmap4.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao4fb.png", UriKind.Relative);
                bitmap4.EndInit();
                img4.Stretch = Stretch.Fill;
                img4.Source = bitmap4;
                quatro.Content = img4;
                quatro.Label = null;
                quatro.Background = new ImageBrush(bitmap4);
            }
            else
            {
                BitmapImage bitmap4 = new BitmapImage();
                Image img4 = new Image();
                bitmap4.BeginInit();
                bitmap4.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao4.png", UriKind.Relative);
                bitmap4.EndInit();
                img4.Stretch = Stretch.Fill;
                img4.Source = bitmap4;
                quatro.Content = img4;
                quatro.Label = null;
                quatro.Background = new ImageBrush(bitmap4);
            }
        }
        public void setCinco(int fed)
        {
            if (fed == 1)
            {
                BitmapImage bitmap5 = new BitmapImage();
                Image img5 = new Image();
                bitmap5.BeginInit();
                bitmap5.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao5fb.png", UriKind.Relative);
                bitmap5.EndInit();
                img5.Stretch = Stretch.Fill;
                img5.Source = bitmap5;
                cinco.Content = img5;
                cinco.Label = null;
                cinco.Background = new ImageBrush(bitmap5);
            }
            else
            {
                BitmapImage bitmap5 = new BitmapImage();
                Image img5 = new Image();
                bitmap5.BeginInit();
                bitmap5.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao5.png", UriKind.Relative);
                bitmap5.EndInit();
                img5.Stretch = Stretch.Fill;
                img5.Source = bitmap5;
                cinco.Content = img5;
                cinco.Label = null;
                cinco.Background = new ImageBrush(bitmap5);
            }
        }
        public void setImages()
        {
            //um
            BitmapImage bitmap1 = new BitmapImage();
            Image img1 = new Image();
            bitmap1.BeginInit();
            bitmap1.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao1fb.png", UriKind.Relative);
            bitmap1.EndInit();
            img1.Stretch = Stretch.Fill;
            img1.Source = bitmap1;
            img1.IsHitTestVisible = false;
            um.Content = img1;
            um.Label = null;
            um.Background = new ImageBrush(bitmap1);

            //dois
            BitmapImage bitmap2 = new BitmapImage();
            Image img2 = new Image();
            bitmap2.BeginInit();
            bitmap2.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao2.png", UriKind.Relative);
            bitmap2.EndInit();
            img2.Stretch = Stretch.Fill;
            img2.Source = bitmap2;
            img2.IsHitTestVisible = false;
            dois.Content = img2;
            dois.Label = null;
            dois.Background = new ImageBrush(bitmap2);

            //tres
            BitmapImage bitmap3 = new BitmapImage();
            Image img3 = new Image();
            bitmap3.BeginInit();
            bitmap3.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao3.png", UriKind.Relative);
            bitmap3.EndInit();
            img3.Stretch = Stretch.Fill;
            img3.Source = bitmap3;
            tres.Content = img3;
            tres.Label = null;
            tres.Background = new ImageBrush(bitmap3);

            //quatro
            BitmapImage bitmap4 = new BitmapImage();
            Image img4 = new Image();
            bitmap4.BeginInit();
            bitmap4.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao4.png", UriKind.Relative);
            bitmap4.EndInit();
            img4.Stretch = Stretch.Fill;
            img4.Source = bitmap4;
            quatro.Content = img4;
            quatro.Label = null;
            quatro.Background = new ImageBrush(bitmap4);

            //cinco
            BitmapImage bitmap5 = new BitmapImage();
            Image img5 = new Image();
            bitmap5.BeginInit();
            bitmap5.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/botao5.png", UriKind.Relative);
            bitmap5.EndInit();
            img5.Stretch = Stretch.Fill;
            img5.Source = bitmap5;
            cinco.Content = img5;
            cinco.Label = null;
            cinco.Background = new ImageBrush(bitmap5);

            //primeiro
            /* BitmapImage bitmap1s = new BitmapImage();
             Image img1s = new Image();
             bitmap1s.BeginInit();
             bitmap1s.UriSource = new Uri("/DETI_Interact_v3;component/Images/Theme1/Horarios/2s.png", UriKind.Relative);
             bitmap1s.EndInit();
             img1s.Stretch = Stretch.Fill;
             img1s.Source = bitmap1s;
             primeiro.Content = img1s;
             primeiro.Label = null;
             primeiro.Background = new ImageBrush(bitmap1s);

             */

            //mudar curso
            BitmapImage bitmapMc = new BitmapImage();
            Image imgMc = new Image();
            bitmapMc.BeginInit();
            bitmapMc.UriSource = new Uri("/You_TimeTables;component/Images/Themes/Theme1/TimeTables/curso.png", UriKind.Relative);
            bitmapMc.EndInit();
            imgMc.Stretch = Stretch.Fill;
            imgMc.Source = bitmapMc;
            imgMc.IsHitTestVisible = false;
            mudarCurso.Content = imgMc;
            mudarCurso.Label = null;
            mudarCurso.Background = new ImageBrush(bitmapMc);
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
            return YouTimeTables2Region;
        }

        #endregion

        #region YouButtonEventHandlers

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var b = (YouButton)e.OriginalSource;
            Click(b.Name);
        }

        

        private void Click(string name)
        {

            switch (name)
            {

                case "MainMenuButton":
                    {
                        imagem.Width = w * 0.1;
                        imagem.Height = h * 0.1;
                        Canvas.SetTop(imagem, h * -0.1);
                        Canvas.SetLeft(imagem, w * -0.1);
                        firstTime = true;
                        setUm(1);
                        setDois(0);
                        setTres(0);
                        setQuatro(0);
                        setCinco(0);
                        ano = 1;
                        semestre = 1;
                        curso = 0;
                        _parser.TimeTable_AnoUm();
                        YouNavigation.navigateToMainMenu(this);
                        break;
                    }
                case "mudarCurso":
                    {
                        imagem.Width = w * 0.1;
                        imagem.Height = h * 0.1;
                        Canvas.SetTop(imagem, h * -0.1);
                        Canvas.SetLeft(imagem, w * -0.1);
                        firstTime = true;
                        setUm(1);
                        setDois(0);
                        setTres(0);
                        setQuatro(0);
                        setCinco(0);
                        ano = 1;
                        _parser.TimeTable_AnoUm();
                        YouNavigation.requestFrameChange(this, "YouTimeTables");


                        break;
                    }
                case "um":
                    {
                        imagem.Width = w * 0.1;
                        imagem.Height = h * 0.1;
                        Canvas.SetTop(imagem, h * -0.1);
                        Canvas.SetLeft(imagem, w * -0.1);
                        Canvas.SetTop(grTimeTable, h * 0.22);
                        Canvas.SetLeft(grTimeTable, w * 0.22);
                        setUm(1);
                        setDois(0);
                        setTres(0);
                        setQuatro(0);
                        setCinco(0);
                        ano = 1;
                        _parser.TimeTable_AnoUm();
                        break;
                    }
                case "dois":
                    {
                        imagem.Width = w * 0.1;
                        imagem.Height = h * 0.1;
                        Canvas.SetTop(imagem, h * -0.1);
                        Canvas.SetLeft(imagem, w * -0.1);
                        if (_parser._selectedCourseIndex == 2)
                        {
                            Canvas.SetTop(grTimeTable, h * 0.22);
                            Canvas.SetLeft(grTimeTable, w * 0.19);
                        }
                        else
                        {
                            Canvas.SetTop(grTimeTable, h * 0.22);
                            Canvas.SetLeft(grTimeTable, w * 0.14);
                        }
                        setUm(0);
                        setDois(1);
                        setTres(0);
                        setQuatro(0);
                        setCinco(0);
                        ano = 2;
                        _parser.TimeTable_AnoDois();
                        break;
                    }
                case "tres":
                    {
                        imagem.Width = w * 0.1;
                        imagem.Height = h * 0.1;
                        Canvas.SetTop(imagem, h * -0.1);
                        Canvas.SetLeft(imagem, w * -0.1);
                        //ect
                        if (_parser._selectedCourseIndex == 1)
                        {
                            Canvas.SetTop(grTimeTable, h * 0.22);
                            Canvas.SetLeft(grTimeTable, w * 0.20);
                        }
                        else if (_parser._selectedCourseIndex == 2)
                        {
                            Canvas.SetTop(grTimeTable, h * 0.22);
                            Canvas.SetLeft(grTimeTable, w * 0.38);
                        }
                        else
                        {
                            Canvas.SetTop(grTimeTable, h * 0.22);
                            Canvas.SetLeft(grTimeTable, w * 0.18);
                        }
                        setUm(0);
                        setDois(0);
                        setTres(1);
                        setQuatro(0);
                        setCinco(0);
                        ano = 3;
                        _parser.TimeTable_AnoTres();
                        break;
                    }
                case "quatro":
                    {
                        imagem.Width = w * 0.1;
                        imagem.Height = h * 0.1;
                        Canvas.SetTop(imagem, h * -0.1);
                        Canvas.SetLeft(imagem, w * -0.1);
                        //ect
                        if (_parser._selectedCourseIndex == 1)
                        {
                            Canvas.SetTop(grTimeTable, h * 0.22);
                            Canvas.SetLeft(grTimeTable, w * 0.23);
                            setUm(0);
                            setDois(0);
                            setTres(0);
                            setQuatro(1);
                            setCinco(0);
                            ano = 4;
                            _parser.TimeTable_AnoQuatro();
                        }
                        else if (_parser._selectedCourseIndex == 0)
                        {
                            Canvas.SetTop(grTimeTable, h * 0.22);
                            Canvas.SetLeft(grTimeTable, w * 0.14);
                            setUm(0);
                            setDois(0);
                            setTres(0);
                            setQuatro(1);
                            setCinco(0);
                            ano = 4;
                            _parser.TimeTable_AnoQuatro();
                        }
                        else
                        {
                            setUm(0);
                            setDois(0);
                            setTres(0);
                            setQuatro(1);
                            setCinco(0);
                            grTimeTable.Children.Clear();
                            imagem.Width = w * 0.70;
                            imagem.Height = h * 0.50;
                            Canvas.SetTop(imagem, h * 0.34);
                            Canvas.SetLeft(imagem, w * 0.15);
                        }

                        break;
                    }
                case "cinco":
                    {
                        grTimeTable.Children.Clear();
                        imagem.Width = w * 0.70;
                        imagem.Height = h * 0.50;
                        Canvas.SetTop(imagem, h * 0.34);
                        Canvas.SetLeft(imagem, w * 0.15);

                        //img4.Stretch = Stretch.Fill;

                        setUm(0);
                        setDois(0);
                        setTres(0);
                        setQuatro(0);
                        setCinco(1);

                        break;

                    }

            }            
        }



        #endregion

        private void Button_GripEvent(object sender, HandPointerEventArgs e)
        {
            var b = (YouButton)sender;
            Click(b.Name);
        }
    }
}
