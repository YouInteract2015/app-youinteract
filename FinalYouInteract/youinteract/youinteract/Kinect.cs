using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;
using System.Collections.ObjectModel;
using YouInteract.YouInteractAPI;
using YouInteract.YouPlugin_System;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace YouInteract.YouBasic
{

    /// <summary>
    /// The Kinect API for developing in <c>YouInteractAPI</c>.
    /// </summary>
    public static class KinectApi
    {
        private static bool video = false;
        private static KinectSensorChooser sensor = new KinectSensorChooser();
        private static KinectSensor bind;
        private static Skeleton[] skeletons;
        private static UserInfo[] userinfos;
        private static InteractionStream interact;
        private static KinectRegion activeRegion = null;
        private static KinectRequirements req = new KinectRequirements(false, true, false);

        /// <summary>
        /// Event Handler
        /// </summary>
        public delegate void EventHandler(BitmapSource e);

        /// <summary>
        /// This is the Color Event that contains the UserInfo Array to be read by the developer
        /// </summary>
        public static event EventHandler ColorStreamEvent = delegate { };
        /// <summary>
        /// This is the Interaction Event that contains the UserInfo Array to be read by the developer
        /// </summary>
        public static event InteractionEventHandler InteractionEvent = delegate { };

        /// <summary>
        /// This is the Skeleton Event that contains the Skeleton Array to be read by the developer
        /// </summary>
        public static event SkeletonEventHandler SkeletonEvent = delegate { };

        /// <summary>
        /// Sets the Color Requirement needed for the Color Stream to be read
        /// </summary>
        /// <param name="b">True sets the ColorStream on. False sets the ColorStream off></param>
        public static void setColor(bool b)
        {
            video = b;
        }

        private static void sensor_KinectChanged(object sender, KinectChangedEventArgs args)
        {
            bool error = false;
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    //args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = true; // (cpatricio@ua.pt - 03/06/2015)
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
                    args.OldSensor.ColorStream.Disable();
                }
                catch (InvalidOperationException)
                {
                    error = true;
                }
            }
            if (args.NewSensor != null)
            {
                try
                {
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                    args.NewSensor.DepthStream.Range = DepthRange.Default;
                    args.NewSensor.SkeletonStream.EnableTrackingInNearRange = true; // (cpatricio@ua.pt - 03/06/2015)
                    args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default; // 20 skeletons joints (10 for Seated)
                    args.NewSensor.SkeletonStream.Enable();
                    args.NewSensor.ColorStream.Enable();

                }
                catch (InvalidOperationException) { error = true; }
            }
            if (!error)
            {
                {
                    if (activeRegion != null)
                        activeRegion.KinectSensor = args.NewSensor;
                    else
                        bind = args.NewSensor;
                }

            }

        }
        private static void SensorOnSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;

                try
                {
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                    var acc = sensor.Kinect.AccelerometerGetCurrentReading();
                    if (req.getInteractionStreamReq())
                    {
                        if (interact != null)
                            interact.ProcessSkeleton(skeletons, acc, skeletonFrame.Timestamp);
                    }

                }
                catch (InvalidOperationException) { }
            }
            var s = skeletons.Where(x=>x.TrackingState == SkeletonTrackingState.Tracked).ToArray();
            if(s.Length >0)
                SkeletonEvent(new SkeletonStreamArgs(s));
            
        }

        /// <summary>
        /// Depth Stream Frame Rady Event
        /// </summary>
        private static void SensorOnDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            if (req.getInteractionStreamReq())
            {
                using (DepthImageFrame depth = e.OpenDepthImageFrame())
                {
                    if (depth == null)
                        return;
                    try
                    {
                        interact.ProcessDepth(depth.GetRawPixelData(), depth.Timestamp);
                    }
                    catch (InvalidOperationException) { }
                }
            }
        }
        /// <summary>
        /// Interaction Stream Frame Ready Event
        /// </summary>
        private static void InteractionStreamOnInteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            if (req.getSkeletonStreamReq())
            {
                using (var isf = e.OpenInteractionFrame())
                {

                    if (isf == null)
                        return;
                    isf.CopyInteractionDataTo(userinfos);

                }
                UserInfo[] users = (from u in userinfos
                                    where u.SkeletonTrackingId > 0
                                    select u).ToArray();
                if (users.Count() > 0)
                    InteractionEvent(new InteractionStreamArgs(users));

            }

        }


        /// <summary>
        /// The onLoaded method that should be called on the project's MainWindow
        /// <b>WARNING:</b> Should not be called in a Page
        /// </summary>
        public static void onLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Console.WriteLine("onLoaded-Kinect.cs");
            YouPluginManager.KinectReqReady += YouPluginManager_KinectReqReady;
            sensor.KinectChanged += sensor_KinectChanged;
            sensor.Start();

            try
            {

                userinfos = new UserInfo[InteractionFrame.UserInfoArrayLength];
                interact = new InteractionStream(sensor.Kinect, new DummyInteractionClient());
                interact.InteractionFrameReady += InteractionStreamOnInteractionFrameReady;
                sensor.Kinect.DepthFrameReady += SensorOnDepthFrameReady;



                skeletons = new Skeleton[sensor.Kinect.SkeletonStream.FrameSkeletonArrayLength];
                sensor.Kinect.SkeletonFrameReady += SensorOnSkeletonFrameReady;


                sensor.Kinect.ColorFrameReady += Kinect_ColorFrameReady;

            }
            catch (NullReferenceException) { }
        }

        /// <summary>
        /// Color Stream Frame Ready Event
        /// </summary>
        private static void Kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            if (video == true)
            {
                using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
                {
                    if (colorFrame == null) return;

                    byte[] colorData = new byte[colorFrame.PixelDataLength];

                    colorFrame.CopyPixelDataTo(colorData);

                    var v = BitmapSource.Create(
                                        colorFrame.Width, colorFrame.Height, // image dimensions
                                        96, 96,  // resolution - 96 dpi for video frames
                                        PixelFormats.Bgr32, // video format
                                        null,               // palette - none
                                        colorData,          // video data
                                        colorFrame.Width * colorFrame.BytesPerPixel // stride
                                        );
                    ColorStreamEvent(v);
                }
            }
        }

        /// <summary>
        /// Kinect Requirements Request Ready
        /// </summary>
        static void YouPluginManager_KinectReqReady(YouPluginManager.KinectRequirementsArgs e)
        {
            req = e.req;
        }

        /// <summary>
        /// Use this function only when testing plugin! Insert in mainWindow when navigating to a frame to assure page requirements are set
        /// </summary>
        /// <param name="e">The Kinect Requirements of the page you're about to navigate to</param>
        public static void setKinectRequirements(KinectRequirements e)
        {
            req = e;
        }

        /// <summary>
        /// Returns the KinectSensorChooser currently in use
        /// </summary>
        public static KinectSensorChooser getSensor()
        {
            return sensor;
        }

        /// <summary>
        /// Binds the Kinect Sensor in use to the KinectRegion provided as an argument
        /// </summary>
        /// <param name="region"> The Kinect Region you want to bind a sensor to </param>
        public static void bindRegion(KinectRegion region)
        {
            if (region != activeRegion)
            {
                region.KinectSensor = bind;
                activeRegion = region;
            }
        }

        /// <summary>
        /// Unbinds the Kinect Region in use
        /// </summary>
        public static void unbindRegion(KinectRegion region)
        {
            if (region.KinectSensor != null)
                bind = region.KinectSensor;
            activeRegion = null;
        }

    }
}
