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

        static string CreateHashOfImage(string path)
        {
            //Ref: https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/compute-hash-values
            //Ref: https://www.sitepoint.com/community/t/bitmap-to-byte-array-in-c/1877
            try
            {
                Bitmap img = new Bitmap(path);

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
            catch
            {
                return string.Empty;
            }
            
        }

        //static bool CompareFiles(string file1, string file2)
        //{
        //    Console.WriteLine(file2);
        //    Bitmap image1 = new Bitmap(file1);
        //    Bitmap image2 = new Bitmap(file2);

        //    if (image1.Width != image2.Width || image1.Height != image2.Height)
        //    {
        //        return false;
        //    }

        //    //for (int i=0; i < image1.Width; i++)
        //    //{
        //    //    for (int j = 0; j < image1.Height; j++)
        //    //    {
        //    //        var img1_ref = image1.GetPixel(i, j);
        //    //        var img2_ref = image2.GetPixel(i, j);

        //    //        if (img1_ref != img2_ref)
        //    //        {
        //    //            return false;
        //    //        }
        //    //    }
        //    //}

        //    string image1Hash = CreateHashOfImage(image1);
        //    string image2Hash = CreateHashOfImage(image2);

        //    return image1Hash == image2Hash;
        //}

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

        //static DuplicatedImage FindDuplicateImage(string ogFile, List<string> files)
        //{
        //    DuplicatedImage duplicatedImage = null;
        //    foreach (string file in files)
        //    {
        //        if(CompareFiles(ogFile, file))
        //        {
        //            if (duplicatedImage == null)
        //            {
        //                duplicatedImage = new DuplicatedImage(ogFile, file);
        //            }
        //            else
        //            {
        //                duplicatedImage.AddDuplicate(file);
        //            }
        //        }
        //    }
        //    return duplicatedImage;
        //}

        static void RemoveCheckedFileFromList(string file, List<String> files)
        {
            files.Remove(file);
        }

        //static void StoreDuplicatedImages(DuplicatedImage duplicatedImage)
        //{
            
        //}

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("(S)earch Folder\n(D)isplay Duplicate Files\n(C)lear Duplicate File Log\n(V)erify Duplicated Files\n(H)ash Individual File\n(Q)uit?");
                string operation = Console.ReadLine();
                {
                    if (operation[0] == 'S' || operation[0] == 's')
                    {
                        Console.Write("Enter Folder to search: ");
                        string dirPath = Console.ReadLine();
                        if (Path.Exists(dirPath))
                        {
                            List<string> filePaths = GetAllFilesInDirectory(dirPath);
                            int totalImages = filePaths.Count;
                            if (totalImages > 0)
                            {
                                int imagesHashed = 0;
                                // Create a list of every hash found
                                List<string> hashesFound = new List<string>();
                                for (int i = 0; i<filePaths.Count; i++)
                                {
                                    int progress = (imagesHashed*100) / totalImages;
                                    ConsoleUtility.WriteProgressBar(progress, true);

                                    string hash = CreateHashOfImage(filePaths[i]);
                                    imagesHashed++;
                                    if (hash != string.Empty)
                                    {
                                        if (hashesFound.Contains(hash))
                                        {
                                            DuplicatedImageFinder.AddHash(hash, filePaths[i]);
                                            RemoveCheckedFileFromList(filePaths[i], filePaths);
                                            i--;
                                        }
                                        else
                                        {
                                            hashesFound.Add(hash);
                                        }
                                    }
                                    //int progress = (i * 100) / files.Count - 1;
                                    //ConsoleUtility.WriteProgressBar(progress, true);

                                    //DuplicatedImage image = FindDuplicateImage(files[i], files.GetRange(i + 1, files.Count - (i + 1)));
                                    //if (image != null)
                                    //{
                                    //    StoreDuplicatedImages(image);
                                        
                                    //}
                                }

                                ConsoleUtility.WriteProgressBar(100, true);
                                Console.WriteLine("\nTotal Images checked : {0}\nCollating Data now...", imagesHashed);

                                DuplicatedImageFinder.FindOriginals(filePaths, hashesFound);

                                DuplicatedImageFinder.SaveData(DUPLICATED_IMAGES_TXT);

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
                        Console.WriteLine();
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
                    else if (operation[0] == 'V' || operation[0] == 'v')
                    {
                        Console.WriteLine("Feature not implemented yet...\n");
                    }
                    else if (operation[0] == 'H' || operation[0] == 'h')
                    {
                        Console.Write("Enter path of file to hash: ");
                        string filePath = Console.ReadLine();
                        if (Path.Exists(filePath))
                        {
                            string hash = CreateHashOfImage(filePath);
                            Console.WriteLine("Hash of image : {0}\n", hash);
                        }
                        else
                        {
                            Console.WriteLine("Path does not exist, please try again.\n");
                        }
                    }
                    else if (operation[0] == 'Q' || operation[0] == 'q')
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
            //string originalFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\P1040133.JPG";
            //string diffNameFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\diff_name.JPG";
            //string sameNameFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name\\P1040133.JPG";
            //string sameNameDiffDateFile = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name_diff_date\\P1040133.JPG";

            //Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, originalFile);
            //Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, originalFile));

            //Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, diffNameFile);
            //Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, diffNameFile));

            //Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, sameNameFile);
            //Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, sameNameFile));

            //Console.WriteLine("Comparing \n{0} \nwith \n{1}", originalFile, sameNameDiffDateFile);
            //Console.WriteLine("Result: {0}\n", CompareFiles(originalFile, sameNameDiffDateFile));


        }
    }
}
