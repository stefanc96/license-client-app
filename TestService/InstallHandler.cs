using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    class InstallHandler
    {
        private static string DefaultPath = Constants.ServiceFolder + Constants.DefaultFolder;
        public static void ProcessZipFile(String path)
        {
            ZipFile.ExtractToDirectory(path, DefaultPath);
            File.Delete(path);
            List<String> filesNames = InstallHandler.GetFileNames(DefaultPath);
            String scriptName = InstallHandler.CreateBatchInstaller(filesNames);
            Console.WriteLine(scriptName);
            InstallHandler.ExecuteScript(scriptName);
        }
        public static List<String> GetFileNames(String path)
        {
            var installFiles = new DirectoryInfo(Constants.ServiceFolder + Constants.DefaultFolder);
            List<String> fileNames = new List<string>();
            foreach (var file in installFiles.GetFiles())
            {
                fileNames.Add(file.Name);
            }

            return fileNames;
        }
        public static String CreateBatchInstaller(List<String> fileNames)
        {
            FileInfo file;
            String command;
            FileStream script = File.Create(Constants.ServiceFolder + Constants.DefaultFolder + "//script.bat");
            foreach (var fileName in fileNames)
            {
                file = new FileInfo(fileName);
                command = GetCommandForExtension(file.Extension, "\""+Constants.ServiceFolder + Constants.DefaultFolder + "\\" + fileName + "\"");
                script.Write(Encoding.ASCII.GetBytes(command),0,command.Length);
                byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                script.Write(newline, 0, newline.Length);
            }
            script.Close();
            return script.Name;
        }

        private static String GetCommandForExtension(String extension, String path)
        {
            if (extension == ".exe")
            {
                return path + " /S /norestart";
            }
            return "msiexec /i " + path + " /q";
        }

        public static void ExecuteScript(String path)
        {
            UACBypass.Execute(path);
        }
    }
}
