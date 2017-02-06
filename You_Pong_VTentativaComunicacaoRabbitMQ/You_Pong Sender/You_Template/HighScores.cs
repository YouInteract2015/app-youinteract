using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You_Pong
{
    public class HighScores
    {
        public int pos;
        public double value;

        public HighScores(int i, double s)
        {
            pos = i;
            value = s;
        }
        public override string ToString()
        {
            return "Lugar: " + pos + " Tempo: " + value;
        }
    }
}
