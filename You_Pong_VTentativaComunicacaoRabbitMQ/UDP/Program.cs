using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UDP
{
    class Program

    {
        const int PORT = 1234;

        static UdpClient UDPReceiver = new UdpClient(PORT);

        static Byte[] receivedData = new Byte[512];
        
        static void Main(string[] args)
        {
            cParser myParser = new cParser();
            
            Console.WriteLine("Introduza o IP:\n");
            String ipadd = Console.ReadLine();
            IPAddress broadcast = IPAddress.Parse(ipadd);           
            //IPAddress broadcast = IPAddress.Parse("192.168.1.64");
            IPEndPoint ep = new IPEndPoint(broadcast, 1234);
            


            IPEndPoint EP = new IPEndPoint(IPAddress.Any, PORT);
            //receivedData = UDPReceiver.Receive(ref EP);
           // myParser.Parse(receivedData);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);



            while (true)
            {

                receivedData = UDPReceiver.Receive(ref EP);
                myParser.Parse(receivedData);


                string g = "ola Joana";
                byte[] sendbuf = Encoding.ASCII.GetBytes(g);
                s.SendTo(sendbuf, ep);
                Console.WriteLine("Message sent to the broadcast address");
            }
        }
    }
}