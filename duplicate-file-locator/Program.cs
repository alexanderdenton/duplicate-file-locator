using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace duplicate_file_locator
{

    internal class Program
    {
        // Function below usies code from the MD5 Comparison.
        
        static bool CompareFiles(string file1, string file2)
        {
            FileInfo FileInfo1 = new FileInfo(file1);
            FileInfo FileInfo2 = new FileInfo(file2);

            var fileStream1 = FileInfo1.OpenRead();
            var fileStream2 = FileInfo2.OpenRead();

            var md5Creator = MD5.Create();

            var fileStream1Hash = md5Creator.ComputeHash(fileStream1);
            var fileStream2Hash = md5Creator.ComputeHash(fileStream2);

            for (var i = 0; i < fileStream1Hash.Length; i++)
            {
                if (fileStream1Hash[i] != fileStream2Hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {

            string originalFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\P1040133.JPG";
            string diffNameFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\diff_name.JPG";
            string sameNameFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name\\P1040133.JPG";
            string sameNameDiffDateFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name_diff_date\\P1040133.JPG";

            Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, originalFile);
            Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, originalFile));

            Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, diffNameFile);
            Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, diffNameFile));

            Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, sameNameFile);
            Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, sameNameFile));

            Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, sameNameDiffDateFile);
            Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, sameNameDiffDateFile));


        }
    }
}
