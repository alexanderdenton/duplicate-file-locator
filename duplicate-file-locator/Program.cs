using System;
using System.Diagnostics;
using System.Drawing;

using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;


namespace duplicate_file_locator
{

    internal class Program
    {
        // Function below usies code from the MD5 Comparison.

        const string DUPLICATED_IMAGES_TXT = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\duplicated-images.txt";

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
            Console.WriteLine(file2);
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

        static List<string> GetAllFilesInDirectory(string dir)
        {
            List<string> files = new List<string>();
            if (Directory.Exists(dir))
            {
                List<string> subdirs = Directory.GetDirectories(dir).ToList<string>();
                foreach (string subdir in subdirs)
                {
                    files.AddRange(GetAllFilesInDirectory(subdir));
                }

                // Add the files in the current directory
                files.AddRange(Directory.GetFiles(dir).ToList<string>());
            }
            return files;
        }

        static DuplicatedImage FindDuplicateImage(string ogFile, List<string> files)
        {
            DuplicatedImage duplicatedImage = null;
            foreach (string file in files)
            {
                if(CompareFiles(ogFile, file))
                {
                    if (duplicatedImage == null)
                    {
                        duplicatedImage = new DuplicatedImage(ogFile, file);
                    }
                    else
                    {
                        duplicatedImage.AddDuplicate(file);
                    }
                }
            }
            return duplicatedImage;
        }

        static void RemoveCheckedFilesFromList(DuplicatedImage image, List<String> files)
        {
            files.Remove(image.GetOriginal());
            foreach (string duplicate in image.GetDuplicates())
            {
                files.Remove(duplicate);
            }
        }

        static void StoreDuplicatedImages(DuplicatedImage duplicatedImage)
        {
            using (StreamWriter sw = File.AppendText(DUPLICATED_IMAGES_TXT))
            {
                sw.WriteLine(duplicatedImage);
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("(S)earch Folder,\n(D)isplay duplicates\n(C)lear duplicate file\n(Q)uit?");
                string operation = Console.ReadLine();
                {
                    if (operation[0] == 'S' || operation[0] == 's')
                    {
                        Console.Write("Enter Folder to search: ");
                        string path = Console.ReadLine();
                        if (Path.Exists(path))
                        {
                            List<string> files = GetAllFilesInDirectory(path);
                            if (files.Count > 0)
                            {
                                //foreach (string file in files)
                                //{
                                //    Console.WriteLine(file);
                                //}

                                // Count -1 because the last file will have already been compared to all the others.
                                //ConsoleUtility.WriteProgressBar(0);
                                for (int i = 0; i < files.Count - 1; i++)
                                {
                                    int progress = (i * 100) / files.Count - 1;
                                    //ConsoleUtility.WriteProgressBar(progress, true);

                                    DuplicatedImage image = FindDuplicateImage(files[i], files.GetRange(i + 1, files.Count - (i + 1)));
                                    if (image != null)
                                    {
                                        StoreDuplicatedImages(image);
                                        RemoveCheckedFilesFromList(image, files);
                                        i--; // Removing current index from list changes what is stored at next image. Without this a file would be skipped each time.
                                    }
                                }
                                //ConsoleUtility.WriteProgressBar(100, true);
                                Console.WriteLine();

                            }
                            else
                            {
                                Console.WriteLine("There are no files in this directory and it's subdirectories, please try again.\n");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Path does not exist, please try again.\n");
                        }

                        
                    }
                    else if (operation[0] == 'D' || operation[0] == 'd')
                    {
                        string text = File.ReadAllText(DUPLICATED_IMAGES_TXT);
                        Console.WriteLine(text);
                    }
                    else if (operation[0] == 'C' || operation[0] == 'c')
                    {
                        using (StreamWriter sw = File.CreateText(DUPLICATED_IMAGES_TXT))
                        {
                            sw.WriteLine("No duplicates found.");
                        }
                        Console.WriteLine("Duplicate file cleared.\n");
                    }
                    else if (operation[0] == 'S' || operation[0] == 's')
                    {
                        Console.WriteLine("Thank you, good bye!");
                        return; // Quit's program
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please try again.\n");
                    }
                }

                


            }
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
