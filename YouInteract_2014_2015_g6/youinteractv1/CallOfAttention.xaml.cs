using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices;
using Microsoft.Kinect.Toolkit.Controls;
using YouInteract.YouBasic;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using YouInteract.YouPlugin_Developing;

namespace YouInteractV1
{
    /// <summary>
    /// Interaction logic for CallOfAttention.xaml
    /// </summary>
    public partial class CallOfAttention : Page
    {

        //[DllImport("User32.dll")]
        private double h;
        private double w;

        private int progressCount = 0;
        private bool showTarget = false;

        private Skeleton activeSkeleton = new Skeleton();

        //private int waitTime = 10;
        private Boolean skeletonVisible = false;

        /**
         * Verificar se um dos pés está na área marcada.
         */
        private Boolean isOver = false;
        private Boolean handOnTarget = false;

        private Boolean canOpen = true;
        private DrawingImage imagesource;
        private DrawingGroup draw;
        private bool rightposition = false;
        List<Image> setas = new List<Image>();
        DispatcherTimer t = new DispatcherTimer();


        public CallOfAttention()
        {
            InitializeComponent();


            isOver = false;


            w = YouWindow.getWidth();
            h = YouWindow.getHeight();
            Loaded += CallOfAttention_Loaded;

            t.Interval = TimeSpan.FromMilliseconds(100);
            pg.Width = w * 0.25;
            pg.Height = h * 0.04;
            Canvas.SetTop(pg, h * 0.90);
            Canvas.SetLeft(pg, w * 0.75 / 2);

            BitmapImage bckgrnd = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/20801.png"));

            pg.Minimum = 0;
            pg.Maximum = 20;
            System.Windows.Media.Animation.DoubleAnimation doubleanimation = new System.Windows.Media.Animation.DoubleAnimation();

            pg.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
            t.Tick += t_Tick;
            skedraw.Width = w;
            skedraw.Height = h;
            loadArrows();
            Unloaded += CallOfAttention_Unloaded;

        }

        void CallOfAttention_Unloaded(object sender, RoutedEventArgs e)
        {
            //KinectApi.SkeletonEvent -= KinectApi_SkeletonEvent;
            Console.WriteLine("unloaded");
        }

        void t_Tick(object sender, EventArgs e)
        {
            //progress bar animation
            if (progressCount == 20)    // 2 segundos (200 ms * 10 = 2 seg)
            {
                t.Stop();
                progressCount = 0;
                pg.Value = 0;
                MainWindow.Reference.YouFrame.Navigate(MainWindow.Reference.youMenu);
            }
            else
            {
                pg.Value = progressCount;

            }
            progressCount++;

        }

        private void loadArrows()
        {
            setas.Add(seta1);
            setas.Add(seta2);
            setas.Add(seta3);
            setas.Add(seta4);
            int i = 0;
            foreach (var s in setas)
            {
                if (i < 2)
                    s.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhadireita.png"));
                else
                    s.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhaesquerda.png"));
                s.Width = w * 0.15;
                s.Height = h * 0.1;
                Canvas.SetTop(s, h * 0.05);
                i++;

            }
            Canvas.SetLeft(seta1, 0.025 * w);
            Canvas.SetLeft(seta2, w * 0.225);
            Canvas.SetLeft(seta3, w * 0.625);
            Canvas.SetLeft(seta4, w * 0.825);

        }

        void CallOfAttention_Loaded(object sender, RoutedEventArgs e)
        {
            //KinectApi.setKinectRequirements(new KinectRequirements(false, true, true));
            //KinectApi.SkeletonEvent += KinectApi_SkeletonEvent;

            setArrows();
            this.draw = new DrawingGroup();
            this.imagesource = new DrawingImage(this.draw);
            skedraw.Source = imagesource;
            rightposition = true;

        }

        private void setArrows()
        {

        }

        void KinectApi_SkeletonEvent(YouInteract.YouInteractAPI.SkeletonStreamArgs e)
        {

            var s = e.skeletons;
            if (s.Count() > 0)
            {
                var ss = (from ske in s
                          where ske.TrackingId > 0
                          orderby ske.TrackingId
                          select ske).ToArray();
                if (ss.Count() > 0)
                {
                    activeSkeleton = ss[0];
                    skeletonVisible = true;
                    drawBones(activeSkeleton);
                }
                else
                {
                    skeletonVisible = false;
                }
            }
        }

        private void drawBones(Skeleton skeleton)
        {

            using (DrawingContext dc = this.draw.Open())
            {
                var i = new ImageBrush();
                i.ImageSource = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/coabg.JPG"));
                dc.DrawRectangle(i, null, new Rect(0.0, 0.0, 640, 480));
                                                         

                // Pulso direito e cotovole direito
                if (skeleton.Joints[JointType.WristRight].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.ElbowRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.WristRight], skeleton.Joints[JointType.ElbowRight], skeleton, dc);
                }
                // Cotovelo direito e ombro direito
                if (skeleton.Joints[JointType.ElbowRight].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.ShoulderRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.ElbowRight], skeleton.Joints[JointType.ShoulderRight], skeleton, dc);
                }
                // Ombro direito e coluna
                if (skeleton.Joints[JointType.ShoulderRight].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.ShoulderRight], skeleton.Joints[JointType.ShoulderCenter], skeleton, dc);
                }
                // Coluna
                if (skeleton.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.Spine].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.Spine], skeleton, dc);
                }

                // Coluna e ombro esquerdo
                if (skeleton.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.ShoulderLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.ShoulderLeft], skeleton, dc);
                }
                // Ombro esquerdo e cotovelo esquerdo
                if (skeleton.Joints[JointType.ShoulderLeft].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.ElbowLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.ShoulderLeft], skeleton.Joints[JointType.ElbowLeft], skeleton, dc);
                }
                // Cotovelo esquerdo e pulso esquerdo
                if (skeleton.Joints[JointType.ElbowLeft].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.WristLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.ElbowLeft], skeleton.Joints[JointType.WristLeft], skeleton, dc);
                }


                // Coluna com o quadril central
                if (skeleton.Joints[JointType.Spine].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.HipCenter].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.Spine], skeleton.Joints[JointType.HipCenter], skeleton, dc);
                }
                // Anca central com o direito
                if (skeleton.Joints[JointType.HipCenter].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.HipRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipRight], skeleton, dc);
                }
                // Anca central com o esquerdo
                if (skeleton.Joints[JointType.HipCenter].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.HipLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.HipCenter], skeleton.Joints[JointType.HipLeft], skeleton, dc);
                }
                // Anca direita com o joelho direito
                if (skeleton.Joints[JointType.HipRight].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.KneeRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.HipRight], skeleton.Joints[JointType.KneeRight], skeleton, dc);
                }
                // Joelho direito com o tornozelo direito
                if (skeleton.Joints[JointType.KneeRight].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.AnkleRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.KneeRight], skeleton.Joints[JointType.AnkleRight], skeleton, dc);
                    //Layer_1.Visibility = System.Windows.Visibility.Hidden;
                }

                // Tornozelo Direito com o pé direito
                if (skeleton.Joints[JointType.AnkleRight].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.FootRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.AnkleRight], skeleton.Joints[JointType.FootRight], skeleton, dc);
                }
                // Anca esquerda com o joelho esquerdo
                if (skeleton.Joints[JointType.HipLeft].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.KneeLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.HipLeft], skeleton.Joints[JointType.KneeLeft], skeleton, dc);
                }
                // Joelho esquerdo com o tornozelo esquerdo
                if (skeleton.Joints[JointType.KneeLeft].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.AnkleLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.KneeLeft], skeleton.Joints[JointType.AnkleLeft], skeleton, dc);
                    //Layer_1.Visibility = System.Windows.Visibility.Hidden;
                }

                // Tornozelo esquerdo com o pé esquerdo
                if (skeleton.Joints[JointType.AnkleLeft].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.FootLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.AnkleLeft], skeleton.Joints[JointType.FootLeft], skeleton, dc);
                }

                // Mão direita e pulso direito
                if (skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.WristRight].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.WristRight], skeleton, dc);
                }
                // Pulso esquerdo e mão esquerda
                if (skeleton.Joints[JointType.WristLeft].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.HandLeft], skeleton.Joints[JointType.WristLeft], skeleton, dc);
                }
                // Coluna e cabeça
                if (skeleton.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked &&
                   skeleton.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked)
                {
                    this.drawBone(skeleton.Joints[JointType.ShoulderCenter], skeleton.Joints[JointType.Head], skeleton, dc);
                }
                this.draw.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, 640, 480));

            }

        }

        private void drawBone(Joint trackedJoint1, Joint trackedJoint2, Skeleton skeleton, DrawingContext dc)
        {

            if (skeleton == null)
                return;

            // Define an image for the head
            if (trackedJoint2.JointType == JointType.Head)
            {
                var p = ScalePosition(trackedJoint2.Position);
                checkforposition(p);
                p.X = p.X - w * 0.025;
                p.Y = p.Y - h * 0.04;
                var area = new Rect(p, new Size(w * 0.05, h * 0.08));
                dc.DrawImage(new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/robot-head.png")), area);

                //ib.SetValue(Canvas.LeftProperty, trackedJoint2.Position.X * w + (w / 2));
                //ib.SetValue(Canvas.TopProperty, h - trackedJoint2.Position.Y * h);
                CallCanvas.UpdateLayout();
            }
            else
            {
                var a = ScalePosition(trackedJoint1.Position);
                var b = ScalePosition(trackedJoint2.Position);

                dc.DrawLine(new Pen(Brushes.Gray, 12), a, b);

                if (trackedJoint1.JointType == JointType.HandRight)
                {
                    var p = ScalePosition(trackedJoint1.Position);
                    p.X = p.X - w * 0.0175;
                    p.Y = p.Y - h * 0.0225;
                    var area = new Rect(p, new Size(w * 0.035, h * 0.045));
                    dc.DrawImage(new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/maodireita.png")), area);
                }

                if (trackedJoint1.JointType == JointType.HandLeft)
                {
                    var p = ScalePosition(trackedJoint1.Position);
                    p.X = p.X - w * 0.0175;
                    p.Y = p.Y - h * 0.0225;
                    var area = new Rect(p, new Size(w * 0.035, h * 0.045));
                    dc.DrawImage(new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/maoesquerda.png")), area);
                }
            }
        }

        private void checkforposition(Point p)
        {
            var div1 = (p.X / 640);
            int div = (int)Math.Ceiling(div1 / 0.2);

            if (div == 3)
            {

                pg.Visibility = Visibility.Visible;
                t.Start();

            }
            else
            {
                pg.Visibility = Visibility.Hidden;
                pg.Value = 0;
                progressCount = 0;
                t.Stop();
            }

            switch (div)
            {
                case (1):
                    {
                        seta1.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhadireita.png"));
                        seta2.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhadireita.png"));
                        seta3.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdeesquerda.png"));
                        seta4.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdeesquerda.png"));
                        rightposition = false;

                        break;
                    }
                case (2):
                    {
                        seta1.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdedireita.png"));
                        seta2.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhadireita.png"));
                        seta3.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdeesquerda.png"));
                        seta4.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdeesquerda.png"));
                        rightposition = false;

                        break;
                    }
                case (3):
                    {
                        seta1.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdedireita.png"));
                        seta2.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdedireita.png"));
                        seta3.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdeesquerda.png"));
                        seta4.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdeesquerda.png"));
                        rightposition = true;

                        // Timer
                        break;
                    }
                case (4):
                    {
                        seta1.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdedireita.png"));
                        seta2.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdedireita.png"));
                        seta3.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhaesquerda.png"));
                        seta4.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdeesquerda.png"));
                        rightposition = false;
                        break;
                    }
                case (5):
                    {
                        seta1.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdedireita.png"));
                        seta2.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setaverdedireita.png"));
                        seta3.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhaesquerda.png"));
                        seta4.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhaesquerda.png"));
                        rightposition = false;

                        break;
                    }
                default:
                    {
                        seta1.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhadireita.png"));
                        seta2.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhadireita.png"));
                        seta3.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhaesquerda.png"));
                        seta4.Source = new BitmapImage(new Uri("pack://application:,,,/YouInteractV1;component/Images/Logo/setavermelhaesquerda.png"));
                        rightposition = false;

                        break;
                    }
            }
        }

        private Point ScalePosition(SkeletonPoint skeletonPoint)
        {
            var s = KinectApi.getSensor();

            DepthImagePoint depthPoint = s.Kinect.CoordinateMapper.MapSkeletonPointToDepthPoint(skeletonPoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }
    }
}
