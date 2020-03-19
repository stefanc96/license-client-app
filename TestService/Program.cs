using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestService
{
    class Program
    {

        
        static void Main()
        {
            RDPServer rdpServer = new RDPServer();
            UDPer udp = new UDPer(rdpServer.GetConnectionString("Licenta", "Licenta", "", 1));
            FileManager.CheckOrCreateFolder(Constants.DefaultFolder);
            udp.Start();
            while (true) ;
        }
    }
}
