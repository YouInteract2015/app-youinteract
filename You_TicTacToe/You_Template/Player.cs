using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace You_TicTacToe
{
    public class Player
    {
        public int ID;  //ID do player NAO ESTA A SER USADA
        public double yactual; // posicao atual da mao
        public double x;    //coordenada x do player NAO ESTA A SER USADA
        public double yimage;  //posicao atual da barra (na frame anterior)
        public int score;   //pontuacao do player
        public bool closed;

        // construtor
        public Player()
        {
            ID = -1;
            yactual = -1;
            x = -1;
            yimage = -1;
            score = 0;
            closed = false;
        }
    }
    
}
