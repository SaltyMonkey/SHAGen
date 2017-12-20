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
        static void Main(string[] args)
        {
            var formattedOutput = new List<string>();
            foreach (var file in GetFiles(AppDomain.CurrentDomain.BaseDirectory))
            {
                formattedOutput.Add($"\"{Path.GetFileName(file)}\":{FileHash(file)}");
            }
            File.WriteAllLines($"{AppDomain.CurrentDomain.BaseDirectory}\\{output}",formattedOutput);
        }

        public static List<string> GetFiles(string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
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
            return hashString;
        }
    }
}
