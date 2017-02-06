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
using Microsoft.Kinect.Toolkit.Interaction;
using Microsoft.Kinect.Toolkit.Controls;

namespace YouInteract.YouInteractAPI
{
    /// <summary>
    /// Framework element to be inserted as a Button
    /// </summary>
    public class YouButton : KinectTileButton
    {
        
        private HandPointer capturedHand;

        /// <summary>
        /// Constructor for MyButton
        /// </summary>
        public YouButton()
        {
            Initialise();

        }

        /// <summary>
        /// Event of Hand Entering
        /// </summary>
        public  event onHandEnterHandler EnterEvent = delegate { };
        /// <summary>
        /// Event of Hand Leaving
        /// </summary>
        public  event onhandLeaveHandler LeaveEvent = delegate { };
        /// <summary>
        /// Event of Grip Release
        /// </summary>
        public  event onGripReleaseHandler ReleaseEvent = delegate { };
        /// <summary>
        /// Event of Grip 
        /// </summary>
        public  event onGripHandler GripEvent = delegate { };

        //Initialization Routine
        private void Initialise()
        {
            KinectRegion.AddHandPointerEnterHandler(this, this.OnHandPointerEnter);
            KinectRegion.AddHandPointerLeaveHandler(this, this.OnHandPointerLeave);
            KinectRegion.AddHandPointerGripHandler(this, this.OnHandPointerGripCaptured);
            KinectRegion.AddHandPointerGripReleaseHandler(this, this.onHandPointerGripReleaseCaptured);
        }

        //Checks if Grip Release Captured
        private void onHandPointerGripReleaseCaptured(object sender, HandPointerEventArgs e)
        {


            if (e.HandPointer.HandEventType == HandEventType.GripRelease)
            {
                ReleaseEvent(sender, e);
            }
            e.Handled = true;

        }

        //Checks if Hand Left a Kinect Button
        private void OnHandPointerLeave(object sender, HandPointerEventArgs e)
        {
            if (!KinectRegion.GetIsPrimaryHandPointerOver(this))
            {
                capturedHand = null;
                LeaveEvent(sender, e);
            }
            e.Handled = true;

        }

        //Checks if Hand Entered a Kinect Button
        private void OnHandPointerEnter(object sender, HandPointerEventArgs e)
        {
            if (KinectRegion.GetIsPrimaryHandPointerOver(this))
            {
                if (capturedHand == null)
                {
                    capturedHand = e.HandPointer;
                    EnterEvent(sender, e);
                }
            }
            e.Handled = true;


        }

        //Checks if Grip Captured
        private void OnHandPointerGripCaptured(object sender, HandPointerEventArgs e)
        {

            if (e.HandPointer.HandEventType == HandEventType.Grip)
            {
                GripEvent(sender, e);
            }
            e.Handled = true;
        }

        /// <summary>
        /// Function that returns the Hand that is hovering the YouButton
        /// </summary>
        /// <returns>The hand that entered/hovers the YouBUtton</returns>
        public HandPointer getActiveHand()
        {
            return capturedHand;
        }

    }
}
