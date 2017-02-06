using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * @author: Rui Oliveira 68779 - 2015
 */
namespace You_Videos
{
    static class AllVideos
    {
        private static Video[] allVideos;
        private static int activeVideo;

        public static void instantate(int nVideos)
        {
            allVideos = new Video[nVideos];
        }

        public static void setVideos(Video[] vd)
        {
            int i;
            for (i = 0; i < allVideos.Length; i++)
            {
                allVideos[i] = vd[i];
            }
        }

        public static void setActiveVideos(int number)
        {
            Console.WriteLine("VIM AQUI");
            activeVideo = number;
            cantinActivation(number);
        }

        public static int getActiveVideos()
        {
            return activeVideo;
        }

        public static Video getVideo(int n)
        {
            return allVideos[n-1];
        }

        public delegate void EventHandler<String>(int e);
        public static event EventHandler<int> cantinActivation = delegate { };
    }
}
