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
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Windows.Threading;
using YouInteract.YouBasic;
using System.Collections.ObjectModel;
using YouInteract.YouInteractAPI;

namespace YouInteractV1.Scheduler
{
    internal static class Scheduler
    {
        static private DispatcherTimer timer = new DispatcherTimer();
        static private bool activeSkeletons = false;
        static private int activetracker = 0;

        public static event timeOut_EventHandler timeOut = delegate { };
        public static event skeletonAppeared_EventHandler skeleton = delegate { };

        internal static void initialize()
        {
   
            var intervalo = new TimeSpan(0, 0, 1); //intervalo de tempo de 1 s
            setTimespan(intervalo);
            timer.Start();
            timer.Tick += timer_Tick;
            KinectApi.SkeletonEvent += KinectApi_SkeletonEvent;
        }
        private static void timer_Tick(object sender, EventArgs e)
        {
            activetracker += 1;
            activeSkeletons = false;
            if (activetracker == 19) // Timeout for screensaver
            {

                activetracker = 0;
                timer.IsEnabled = false;
                timeOut();
            }
        }

        private static void KinectApi_SkeletonEvent(SkeletonStreamArgs e)
        {
            //Console.WriteLine("Scheduler");
            /*foreach (var u in e.skeletons)
            {
                if (u.TrackingId != 0)
                {
                    activeSkeletons = true;
                    activetracker = 0;
                    skeleton();
                    timer.IsEnabled = true;
                    break;
                }
            }*/
            activeSkeletons = true;
            activetracker = 0;
            skeleton();
            timer.IsEnabled = true;
        }
        public static void setTimespan(TimeSpan x)
        {
            timer.Interval = x;
        }
        public delegate void timeOut_EventHandler();
        public delegate void skeletonAppeared_EventHandler();
    }
}
