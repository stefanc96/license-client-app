using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class AesInfo
    {

        public byte[] key { get; set; }
        public byte[] IV { get; set; }
        public AesInfo(byte[] key, byte[] IV)
        {
            this.key = key;
            this.IV = IV;
        }
    }
}
