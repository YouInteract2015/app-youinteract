using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Gallery
{
    static class AllPhotos
    {
        private static Photo[] allCantins;
        private static int activeCantin;

        public static void instantate(int nCantins)
        {
            allCantins = new Photo[nCantins];
        }

        public static void setCantins(Photo[] cantins)
        {
            int i;
            for (i = 0; i < allCantins.Length; i++)
            {
                allCantins[i] = cantins[i];
            }
        }

        public static void setActiveCantin(int number)
        {
            Console.WriteLine("VIM AQUI");
            activeCantin = number;
            cantinActivation(number);
        }

        public static int getActiveCantin()
        {
            return activeCantin;
        }

        public static Photo getCantin(int n)
        {
            return allCantins[n - 1];
        }

        public delegate void EventHandler<String>(int e);
        public static event EventHandler<int> cantinActivation = delegate { };
    }
}
