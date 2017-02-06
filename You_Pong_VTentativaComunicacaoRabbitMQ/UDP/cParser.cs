using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDP
{
    class cParser
    {
        public void Parse(Byte[] data)
        {
            string received = Encoding.ASCII.GetString(data);
            
            switch(received){
                case "coisas":
                    //Descrever cada caso com o que deve fazer
                    break;
                default:
                    Console.WriteLine(received);
                    break;
            }
        }
    }
}
