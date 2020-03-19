using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class Constants
    {
        public static String ServiceFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static String ShareFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string DefaultFolder =@"\AdminApp";
        public static string PublicKey = "<RSAKeyValue><Modulus>vAunSIAEMNOJ8LmggMvir/gYDzedj3+KIfyhGvO8ij0DywzT40Wid0hZ87g/eb3hPsubfqCBmwd0HcY5cD60+I9QOBcEV/8f/r2+6icCvg7DBKWAIceYx4bwTB7pvoNLz6e3x+UHfLPjHMfIE1pbrTpJmJK/8Wp6jIYpLRZ85jE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        public static string PrivateKey = "<RSAKeyValue><Modulus>0jIvMUA6vO9kyn57l/kOnQTCKviyWUvtSc1lsDq1zaTxKUE4cs+QXZc7kOhpxbwtEhF47DzDmB49tQ2pCmtjQotP16lFnBEH+CqMZ9DeSnyrz4S39p1+JXWKkyPgFqAQbhHOGeLZ6uK8z1+x9E0ykw8hRNvUuWMN5hg1eU8ZHVk=</Modulus><Exponent>AQAB</Exponent><P>4DCOAUip5v5Yaq4apMO3FZR7rt+y80DM/dSVIrrihxIvT/eIkhurSpu1TXaSQH367NMWTRoJ4kV26BzpZEZnuw==</P><Q>8AVTfRKmkFc40esbGoC4P3yozSK1thOH80W+2RkZNcOQJKQorE8V/FtAa8EeCNZ3VK4DMAljvHJDS9gLoR8r+w==</Q><DP>RME/W89wI+KPNTBuBWfsh1bBU9FRLV8LPzFqB3uvK2N9VRYCMWUA9GGqibY6hkLcqLLYHQ9GRrmtDOSJA3LALw==</DP><DQ>oef12XirQtKTUYb2UXpizvCYLUgsqxWhr0hs5KU5jDrPEOhrR1BR1Fj5q7YpPGzvMf/vxoeO759kJdUmgxkz4Q==</DQ><InverseQ>e9cOP7PFBKnbBnr6y24PxLPw2OQqbECs7f5qhypBCZ+8Fky9Gi3n5DSmbvsQHP2KlRD+8Zaeht4oEMd/uWehcQ==</InverseQ><D>LRNB94fUkMuti5cQHgj0z0tD8D8gz/FuU1NOqToN3qmuZbBs0IIebVOPHIYamKK/7437pn1pXGevx/HD/E4HHMyCxr1xPQEatuP0MRqBKfQWQPM/K8/eX67m+0Q1RFbSMLvZIYORqxSSmxgORCOoMNSnsmU+FAdoi0UJaNT9QVk=</D></RSAKeyValue>";
        public static int DefaultBroadcastUdpPort = 3500;

    }
}
