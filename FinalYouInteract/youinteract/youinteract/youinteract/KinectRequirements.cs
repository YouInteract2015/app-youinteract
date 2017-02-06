using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YouInteract.YouBasic
{
    /// <summary>
    /// A class used to identify the KinectRequirements of a certain page, 
    /// used to optimize application performance
    /// </summary>
    public class KinectRequirements
    {
        private bool SkeletonStream;
        private bool InteractionStream;
        private bool KinectRegion;
        /// <summary>
        /// A class that determines if the SkeletonStream and the InteractionStream will be activated
        /// </summary>
        /// <param name="kinectRegion">Whether the page will have a KinectRegion or not (<b><i>true</i></b> or <b><i>false</i></b>)</param>
        /// <param name="skeleton">Wheter the SkeletonStream will be activated or not (<b><i>true</i></b> or <b><i>false</i></b>)</param>
        /// <param name="interaction">Wheter the InteractionStream will be activated or not (<b><i>true</i></b> or <b><i>false</i></b>)</param>
        public KinectRequirements(bool kinectRegion, bool skeleton, bool interaction)
        {
            KinectRegion = kinectRegion;
            SkeletonStream = skeleton;
            InteractionStream = interaction;
        }
        /// <summary>
        /// Function for getting the SkeletonStream requirement value
        /// </summary>
        /// <returns>SkeletonStream Requirement Value</returns>
        public bool getSkeletonStreamReq()
        {
            return SkeletonStream;
        }
        /// <summary>
        /// Function for getting the InteractionStream requirement value
        /// </summary>
        /// <returns>SkeletonStream Requirement Value</returns>
        public bool getInteractionStreamReq()
        {
            return InteractionStream;
        }
        /// <summary>
        /// Function for getting the KinectRegion requirement value
        /// </summary>
        /// <returns>SkeletonStream Requirement Value</returns>
        public bool getKinectRegionReq()
        {
            return KinectRegion;
        }
    }
}
