using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class Response
    {
        public bool isSucces;
        public string ip;
        public Response(bool isSucces, string ip)
        {
            this.isSucces = isSucces;
            this.ip = ip;
        }
    }
}
