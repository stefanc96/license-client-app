using Amazon.S3;
using Amazon.S3.Transfer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestService
{

    class UDPer
    {
        const string MethodInstall = "INSTALL";
        const string MethodShare = "SHARE";
        const string MethodDiscover = "DISCOVER";
        string adminIp;
        RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
        byte[] key;
        byte[] IV;

        const string MethodSendInfo = "INFO";
        const int PORT_NUMBER = 15000;
        readonly string connectionString;
        static Aes myAes = Aes.Create();
        public UDPer(string connectionString)
        {
            this.connectionString = connectionString;
            string configFileName = "./" + GetLocalIp() + ".config";
            if (!File.Exists(configFileName))
            {
                FileManager.CreateConfigFile(configFileName, rsaProvider.ToXmlString(true));
            }
            else
            {
                string PK = File.ReadAllText(configFileName);
                rsaProvider.FromXmlString(PK);
            }
          
        }



        readonly Thread t = null;
        public void Start()
        {
            if (t != null)
            {
                throw new Exception("Already started, stop first");
            }
            Console.WriteLine("Started listening");
            StartListening();
        }
        public void Stop()
        {
            try
            {
                udp.Close();
                Console.WriteLine("Stopped listening");
            }
            catch {
            }
        }

        private readonly UdpClient udp = new UdpClient(PORT_NUMBER);

        private void StartListening()
        {
            IAsyncResult asyncResult = udp.BeginReceive(Receive, new object());
        }

        List<string> GetTargetsList(JToken[] tokens)
        {
            List<string> targets = new List<string>();
            foreach(JToken token in tokens)
            {
                string target = token.ToObject<string>();
                targets.Add(target);
            }
            return targets;
        }


        public void ProcessInstall(byte[] info)
        {
            if(key.Length == 0)
            {
                return;
            }
            Response response = new Response(false, GetLocalIp());
            string message = DecryptStringFromBytes_Aes(info, this.key, this.IV);
            JObject jObject = JObject.Parse(message);
            string link = jObject["info"].ToString();
            JToken[] tokens = jObject["targets"].ToArray();
            List<string> targets = GetTargetsList(tokens);
            if (targets.Contains(GetLocalIp()))
            {
                FileManager.EmptyFolder(Constants.ServiceFolder + Constants.DefaultFolder);
                string path = Constants.ServiceFolder + Constants.DefaultFolder + "/test.zip";
                using (var client = new WebClient())
                {
                    client.DownloadFile(link, path);
                }
                InstallHandler.ProcessZipFile(path);

                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), Constants.DefaultBroadcastUdpPort);
                string method = "Method " + MethodInstall + " ";
                response.isSucces = true;
                byte[] encyptedBytes = EncryptStringToBytes_Aes(JsonConvert.SerializeObject(response), key, IV);
                byte[] bytes = Combine(Encoding.ASCII.GetBytes(method), encyptedBytes);
                udp.Send(bytes, bytes.Length, ip);
            }
        }

        public void ProcessShare(byte[] info)
        {
            if (key == null)
            {
                return;
            }
            Response response = new Response(false, GetLocalIp());
            string message = DecryptStringFromBytes_Aes(info, this.key, this.IV);
            Console.WriteLine(message);
            JObject jObject = JObject.Parse(message);
            string link = jObject["info"].ToString();
            JToken[] tokens = jObject["targets"].ToArray();
            List<string> targets = GetTargetsList(tokens);
            if (targets.Contains(GetLocalIp()))
            {
                string path = Constants.ShareFolder + "/share.zip";

               
                try
                {
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(link, path);
                    }
                    ZipFile.ExtractToDirectory(path, Constants.ShareFolder);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                File.Delete(path);
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), Constants.DefaultBroadcastUdpPort);
                string method = "Method " + MethodShare + " ";
                response.isSucces = true;
                byte[] encyptedBytes = EncryptStringToBytes_Aes(JsonConvert.SerializeObject(response), key, IV);
                byte[] bytes = Combine(Encoding.ASCII.GetBytes(method), encyptedBytes);
                udp.Send(bytes, bytes.Length, ip);
            }
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
               
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
             
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        private byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }
        private void ProcessSendInfo(byte[] info)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(adminIp), Constants.DefaultBroadcastUdpPort);
            string message = JsonConvert.SerializeObject(new RDPClient(connectionString, GetLocalIp()));
            string decryptedInfo = Encoding.ASCII.GetString(rsaProvider.Decrypt(info,false));

            AesInfo jObject = JsonConvert.DeserializeObject<AesInfo>(decryptedInfo);
            this.key = jObject.key;
            this.IV = jObject.IV;
            string method = "Method " + MethodSendInfo + " ";
            byte[] encyptedBytes = EncryptStringToBytes_Aes(message, key, IV);
            byte[] bytes = Combine(Encoding.ASCII.GetBytes(method), encyptedBytes);
            udp.Send(bytes, bytes.Length, ip);
        }

        public void ProcessDiscover(byte[] info)
        {
            adminIp = Encoding.ASCII.GetString(info);
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(adminIp), Constants.DefaultBroadcastUdpPort);
            string message = JsonConvert.SerializeObject(new ClientInfo(GetLocalIp(), rsaProvider.ToXmlString(false)));
            string method = "Method " + MethodDiscover + " ";
            byte[] bytes = Encoding.ASCII.GetBytes(method + message);
            udp.Send(bytes, bytes.Length, ip);
        }

        private void ProcessMethod(string method, byte[] info)
        {
            switch (method)
            {
                case MethodInstall:
                    ProcessInstall(info);
                    break;
                case MethodShare:
                    ProcessShare(info);
                    break;
                case MethodDiscover:
                    ProcessDiscover(info);
                    break;
                case MethodSendInfo:
                    ProcessSendInfo(info);
                    break;

            }
        }

        static string GetLocalIp()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
        private void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, PORT_NUMBER);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            string message = Encoding.ASCII.GetString(bytes);
            Console.WriteLine(message);
            string[] messageArguments = message.Split(' ');
            this.ProcessMethod(messageArguments[1], GetData(bytes));
            StartListening();

        }

        private static byte[] GetData(byte[] info)
        {
            int spaceCounter = 0;
            List<byte> array = new List<byte>();
            for(int i = 0; i < info.Length; i++)
            {
                char c = Convert.ToChar(info[i]);
                if (c == ' ')
                {
                    spaceCounter++;
                    if (spaceCounter == 2) continue;
                }
                if (spaceCounter >= 2)
                {
                    array.Add(info[i]);
                }
            }
            return array.ToArray();
        }
    }


}
