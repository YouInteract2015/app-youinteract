using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_TimeTables
{
    class MyCurso
    {
        private static int curso;

        public static void setActiveCurso(int newcurso)
        {
            curso = newcurso;
            cursoActivation(newcurso);
        }



        public delegate void EventHandler (int e);

        public static event EventHandler cursoActivation = delegate { };

        public static event EventHandler cursoDesactivation = delegate { };
    }
}
