using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{

    public class RDPClient
    {
        public string connectionString;
        public string ip;

        public RDPClient(string connectionString, string ip)
        {
            this.connectionString = connectionString;
            this.ip = ip;
        }
    }
}

