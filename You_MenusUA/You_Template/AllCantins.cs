using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 *  @author: Pedro Abade 59385
 *          Tomás Rodrigues 68129
 * 
 * Class for saving all canteens available
 */
namespace You_MenusUA
{
    static class AllCantins
    {
        // canteens
        private static Cantin[] allCantins;
        // active canteen
        private static int activeCantin;

        /**
         * Instantiate 
         */
        public static void instantate(int nCantins)
        {
            allCantins = new Cantin[nCantins];
        }

        /**
         * Save canteens
         */
        public static void setCantins(Cantin[] cantins)
        {
            int i;
            for (i=0 ; i < allCantins.Length ; i++)
            {
                allCantins[i] = cantins[i];
            }
        }

        /**
         * Select active canteen
         */
        public static void setActiveCantin(int number)
        {
            activeCantin = number;
            cantinActivation(number);
        }

        /**
         * Return active canteen
         */
        public static int getActiveCantin()
        {
            return activeCantin;
        }

        /**
         * Return specific canteen
         */
        public static Cantin getCantin(int n)
        {
            return allCantins[n-1];
        }

        public delegate void EventHandler<String>(int e);
        // Handler that is used when selecting a canteen
        public static event EventHandler<int> cantinActivation = delegate { };
    }
}