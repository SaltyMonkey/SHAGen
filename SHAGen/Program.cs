using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SHAGen
{
    class Program
    {
        private static string output = "hashes.txt";
        private static string mainDir = AppDomain.CurrentDomain.BaseDirectory;

        static void Main(string[] args)
        {
            var formattedOutput = new List<string>();
            foreach (var file in GetFiles(mainDir))
            {
                formattedOutput.Add($"\"{GetRelativePath(file, mainDir)}\":\"{FileHash(file)}\",");
            }
            File.WriteAllLines($"{mainDir}\\{output}",formattedOutput);
        }

        static string GetRelativePath(string filespec, string folder)
        {
            var pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            var folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static List<string> GetFiles(string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Select(Path.GetFullPath).Where(p=> !p.Contains(".git") && !p.Contains("exe")).ToList();
        }

        public static string FileHash(string file)
        {
            string hashString;
            using (var stream = File.OpenRead(file))
            {
                var sha = SHA1.Create();
                var hash = sha.ComputeHash(stream);
                hashString = BitConverter.ToString(hash);
                hashString = hashString.Replace("-", "");
            }
            return hashString.ToLowerInvariant();
        }
    }
}
