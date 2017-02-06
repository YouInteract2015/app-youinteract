using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace You_Pong
{
    public class Ball
    {
        public double r;    // raio
        public double x;    // posicao x
        public double y;    // posicao y
        public double velocity; // velocidade NAO E USADA
        public double speedx, speedy;   // velocidade em x,y

        // construtor
        public Ball(double x, double y, double r)
        {
            this.r = r;
            this.x = x;
            this.y = y;
            velocity = 1;   // usada?

        }
    }
}
