using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class ClientInfo
    {
        public string ip;
        public string PK;

        public ClientInfo(string ip, string PK)
        {
            this.ip = ip;
            this.PK = PK;
        }
    }
}
