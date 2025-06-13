using System;
using System.Drawing;

using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace duplicate_file_locator
{

    internal class Program
    {
        // Function below usies code from the MD5 Comparison.

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

        static string CreateHashOfImage(Bitmap img)
        {
            byte[] tmpSource;
            byte[] tmpHash;

            //Create a byte array from source data.
            MemoryStream stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            tmpSource = stream.ToArray();

            //Compute hash based on source data.
            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

            return ByteArrayToString(tmpHash);
        }

        static bool CompareFiles(string file1, string file2)
        {
            Bitmap image1 = new Bitmap(file1);
            Bitmap image2 = new Bitmap(file2);

            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                return false;
            }

            //for (int i=0; i < image1.Width; i++)
            //{
            //    for (int j = 0; j < image1.Height; j++)
            //    {
            //        var img1_ref = image1.GetPixel(i, j);
            //        var img2_ref = image2.GetPixel(i, j);

            //        if (img1_ref != img2_ref)
            //        {
            //            return false;
            //        }
            //    }
            //}

            string image1Hash = CreateHashOfImage(image1);
            string image2Hash = CreateHashOfImage(image2);

            return image1Hash == image2Hash;


           
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
