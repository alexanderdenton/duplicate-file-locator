using System;
using System.Security.Cryptography;
using System.Text;

namespace duplicate_file_locator
{
    internal class Program
    {

        static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        static string CreateHashOfFile(string filePath)
        {
            byte[] tmpSource;
            byte[] tmpHash;

            //Create a byte array from source data.
            tmpSource = ASCIIEncoding.ASCII.GetBytes(filePath);

            //Compute hash based on source data.
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            return ByteArrayToString(tmpHash);
        }

        static bool CompareFiles(string file1, string file2)
        {
            string file1Hash = CreateHashOfFile(file1);
            string file2Hash = CreateHashOfFile(file2);

            return file1Hash == file2Hash;
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
