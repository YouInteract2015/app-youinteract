﻿using System;

using System.Collections.Generic;

using System.Linq;

using System.Runtime.Remoting;

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

using YouInteract.YouBasic;

using YouInteractV1.LoaderData;

using YouInteract.YouPlugin_Developing;

using System.Diagnostics;

using System.Threading;

namespace YouInteractV1
{

    /// <summary>

    /// Interaction logic for Screensaver.xaml

    /// </summary>

    public partial class Screensaver : Page
    {

        private double h, w;

        private DispatcherTimer timer, timerScheduler;

        private Random r = new Random();

        private int status; // 0 for Logo, 1 .. n for schedulers array

        private schedulers[] activeSchedulers;

        private int index;
        private int nSchedulers;

        public Screensaver()
        {

            InitializeComponent();

            activeSchedulers = YouInteractV1.LoaderData.ManageStructs.GetActiveSchedulers().ToArray();
            index = 1;
            nSchedulers = activeSchedulers.Length;
            status = 0;

            w = MainWindow.Reference.Width;
            h = MainWindow.Reference.Height;

            ScreensaverImage.Width = h * 0.2;
            ScreensaverImage.Height = h * 0.2;
            ScreensaverImage.Visibility = System.Windows.Visibility.Visible;

            Canvas.SetTop(ScreensaverImage, h * 0.5);
            Canvas.SetRight(ScreensaverImage, w * 0.5);

            SchedulerImage.Width = w;
            SchedulerImage.Height = h;

            //SchedulerVideo = w;
            //SchedulerVideo = h;
            //VideoControl.ScrubbingEnabled = true;

            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 2) };
            timer.Tick += timer_Tick;
            timerScheduler = new DispatcherTimer { Interval = new TimeSpan(0, 0, 8) };
            timerScheduler.Tick += schedulerTick;

            if (activeSchedulers[0].type == "Image")
            {
                SchedulerImage.Source = new BitmapImage(new Uri("file://" + AppDomain.CurrentDomain.BaseDirectory + "images\\" + activeSchedulers[0].path));
                SchedulerImage.Visibility = System.Windows.Visibility.Visible;
                mediaPlayer.Visibility = System.Windows.Visibility.Hidden;
                moveImg();
            }
            else if (activeSchedulers[0].type == "Video")
            {
                mediaPlayer.Source = new Uri(("file://" + AppDomain.CurrentDomain.BaseDirectory + "videos\\" + activeSchedulers[0].path));
                mediaPlayer.Visibility = System.Windows.Visibility.Visible;
                SchedulerImage.Visibility = System.Windows.Visibility.Hidden;
            }

        }

        public void activateTimer()
        {

            timer.Start();

            timerScheduler.Start();

        }

        public void stopTimer()
        {

            timer.Stop();

            timerScheduler.Stop();

        }

        void timer_Tick(object sender, EventArgs e)
        {

            if (status == 0)

                moveImg();

        }

        void schedulerTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            int week_day = (int)DateTime.Now.DayOfWeek;
            int sund = 0;
            int mond = 1;
            int tuesd = 2;
            int wed = 3;
            int thursd = 4;
            int frid = 5;
            int sat = 6;
            ScreensaverImage.Visibility = System.Windows.Visibility.Hidden;
            foreach (schedulers scheduler in activeSchedulers)
            {
                DateTime dtinit = DateTime.Parse(scheduler.startAt);
                DateTime dend = DateTime.Parse(scheduler.endAt);

                if (dtinit.Date < now.Date && dend.Date > now.Date && scheduler.active == "True")
                {
                    if ((week_day == sund && scheduler.sunday == 1) || (week_day == mond && scheduler.monday == 1) || (week_day == tuesd && scheduler.tuesday == 1) || (week_day == wed && scheduler.wednesday == 1) || (week_day == thursd && scheduler.thursday == 1) || (week_day == frid && scheduler.friday == 1) || (week_day == sat && scheduler.saturday == 1))
                    {
                        if (r.Next(1, 4) < 3)
                            continue;
                        if (scheduler.type == "Image")
                        {
                            SchedulerImage.Source = new BitmapImage(new Uri("file://" + AppDomain.CurrentDomain.BaseDirectory + "images\\" + scheduler.path));
                            SchedulerImage.Visibility = System.Windows.Visibility.Visible;
                            mediaPlayer.Visibility = System.Windows.Visibility.Hidden;
                            moveImg();
                        }
                        else if (scheduler.type == "Video")
                        {
                            mediaPlayer.Source = new Uri(("file://" + AppDomain.CurrentDomain.BaseDirectory + "videos\\" + scheduler.path));
                            mediaPlayer.Visibility = System.Windows.Visibility.Visible;
                            SchedulerImage.Visibility = System.Windows.Visibility.Hidden;
                        }
                        return;
                    }
                }
            }
            schedulers aux = activeSchedulers[(index++) % nSchedulers];
            if (aux.type == "Image")
            {
                SchedulerImage.Source = new BitmapImage(new Uri("file://" + AppDomain.CurrentDomain.BaseDirectory + "images\\" + aux.path));
                SchedulerImage.Visibility = System.Windows.Visibility.Visible;
                mediaPlayer.Visibility = System.Windows.Visibility.Hidden;
                moveImg();
            }
            else if (aux.type == "Video")
            {
                mediaPlayer.Source = new Uri(("file://" + AppDomain.CurrentDomain.BaseDirectory + "videos\\" + aux.path));
                mediaPlayer.Visibility = System.Windows.Visibility.Visible;
                SchedulerImage.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void moveImg()
        {

            Canvas.SetRight(SchedulerImage, (w - Canvas.GetRight(SchedulerImage)) * r.NextDouble());
            Canvas.SetTop(SchedulerImage, (h - Canvas.GetTop(SchedulerImage)) * r.NextDouble());
            //Console.WriteLine(Canvas.GetRight(SchedulerImage) + "," + Canvas.GetTop(SchedulerImage));
        }

    }

}


