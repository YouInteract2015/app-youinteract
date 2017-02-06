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

namespace YouInteract.YouBasic
{

    /// <summary>
    /// The Kinect API for developing in <c>YouInteractAPI</c>.
    /// </summary>
    public static class KinectApi
    {
        private static KinectSensorChooser sensor = new KinectSensorChooser();
        private static KinectSensor bind;
        private static Skeleton[] skeletons;
        private static UserInfo[] userinfos;
        private static InteractionStream interact;
        private static KinectRegion activeRegion = null;
        private static KinectRequirements req;

        /// <summary>
        /// This is Interaction Event that contains the UserInfo Array
        /// </summary>
        public static event InteractionEventHandler InteractionEvent = delegate { };

        /// <summary>
        /// This is Skeleton Event that contains the Skeleton Array
        /// </summary>
        public static event SkeletonEventHandler SkeletonEvent = delegate { };


        private static void sensor_KinectChanged(object sender, KinectChangedEventArgs args)
        {
            bool error = false;
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.DepthStream.Range = DepthRange.Default;
                    args.OldSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();
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
                    args.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                    args.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Default;
                    args.NewSensor.SkeletonStream.Enable();
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
                    interact.ProcessSkeleton(skeletons, acc, skeletonFrame.Timestamp);


                }
                catch (InvalidOperationException) { }
            }
            SkeletonEvent(new SkeletonStreamArgs(skeletons));


        }

        private static void SensorOnDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
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
        private static void InteractionStreamOnInteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            using (var isf = e.OpenInteractionFrame())
            {

                if (isf == null)
                    return;
                isf.CopyInteractionDataTo(userinfos);

            }
            UserInfo[] users = (from u in userinfos
                                where u.SkeletonTrackingId>0
                                select u).ToArray();
            if(users.Count() >0)
            InteractionEvent(new InteractionStreamArgs(users));



        }
        /// <summary>
        /// The onLoaded method that should be called on the project's MainWindow
        /// <b>WARNING:</b> Should not be called in a Page
        /// </summary>

        public static void onLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            YouPluginManager.KinectReqReady += YouPluginManager_KinectReqReady;
            sensor.KinectChanged += sensor_KinectChanged;
            sensor.Start();

            try
            {
                //if (req.getKinectRegionReq())
                //{
                //    activeRegion.KinectSensor = sensor.Kinect;

                //}
                if (req.getSkeletonStreamReq())
                {
                    skeletons = new Skeleton[sensor.Kinect.SkeletonStream.FrameSkeletonArrayLength];
                    sensor.Kinect.SkeletonFrameReady += SensorOnSkeletonFrameReady;
                }
                if (req.getInteractionStreamReq())
                {
                    userinfos = new UserInfo[InteractionFrame.UserInfoArrayLength];
                    interact = new InteractionStream(sensor.Kinect, new DummyInteractionClient());
                    interact.InteractionFrameReady += InteractionStreamOnInteractionFrameReady;


                }
                sensor.Kinect.DepthFrameReady += SensorOnDepthFrameReady;
            }
            catch (NullReferenceException) { }
        }

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
