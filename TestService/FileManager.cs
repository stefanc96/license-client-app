using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestService
{
    class FileManager
    {
        public static string CheckOrCreateFolder(String folder)
        {
            string path = Constants.ServiceFolder +folder;
            if (!Directory.Exists(path))
            { 
                DirectoryInfo directory = System.IO.Directory.CreateDirectory(path);
                directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            else
            {
                EmptyFolder(path);
            }

            return path;
        }

        public static void EmptyFolder(String path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public bool CheckIfConfigFileExists(string path)
        {
            return File.Exists(path);
        }

        public static void CreateConfigFile(string path, string PK)
        {
            File.WriteAllText(path,PK);
        }

    }
}
