using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using Microsoft.Kinect.Toolkit.Interaction;

namespace YouInteract.YouInteractAPI
{
    /// <summary>
    /// Contains the UserInfo Array
    /// </summary>
    public class InteractionStreamArgs
    {
        /// <summary>
        /// Constructor for InteractionStreamArgs
        /// </summary>
        /// <param name="user">An Array of User Infos</param>
        public InteractionStreamArgs(UserInfo[] user){
            userinfo = user;
        }

        /// <summary>
        /// UserInfo Array
        /// </summary>
        public UserInfo[] userinfo { get; private set;}

    }

    /// <summary>
    /// Contains the Skeleton Array
    /// </summary>
    public class SkeletonStreamArgs
    {
        /// <summary>
        /// A Constructor for SkeletonStreamArgs
        /// </summary>
        /// <param name="skeleton">A Skeleton Array</param>
        public SkeletonStreamArgs(Skeleton[] skeleton)
        {
            skeletons = skeleton;
        }

        /// <summary>
        /// Skeletons Array
        /// </summary>
        public Skeleton[] skeletons { get; private set; }

    }
    /// <summary>
    /// This sends the Skeleton Array data of the tracked Skeletons
    /// 
    /// </summary>
 
    public delegate void SkeletonEventHandler(SkeletonStreamArgs e);

    /// <summary>
    /// This sends the UserInfo Array data of the Valid UserInfos
    /// </summary>
 
    
    public delegate void InteractionEventHandler(InteractionStreamArgs e);
    /// <summary>
    /// Raises an event for Hand Entering (Hovering) a Mybutton
    /// </summary>
    public delegate void onHandEnterHandler(object sender, HandPointerEventArgs e);
    /// <summary>
    /// Raises an event for Hand Leaving a Mybutton
    /// </summary>
    public delegate void onhandLeaveHandler(object sender, HandPointerEventArgs e);
    /// <summary>
    /// Raises an event for a Grip on a Mybutton
    /// </summary>
    public delegate void onGripHandler(object sender, HandPointerEventArgs e);
    /// <summary>
    /// Raises an event for a grip release on a MyButtonRelease
    /// </summary>
    public delegate void onGripReleaseHandler(object sender, HandPointerEventArgs e);


}
