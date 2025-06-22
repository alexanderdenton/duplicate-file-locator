﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

using System.Drawing;
using Newtonsoft.Json;

namespace duplicate_file_locator
{
    public static class DuplicatedImageFinder
    {
        private static List<DuplicatedImage> _duplicatedImages = new List<DuplicatedImage>();

        public static void AddHash(string hash, string pathToDuplicate)
        {
            if (_duplicatedImages.Any(img => img.Hash == hash))
            {
                // Get the DuplicatedImage object with the same hash value and add the path to the duplicate
                _duplicatedImages.FirstOrDefault(img => img.Hash == hash).AddDuplicate(pathToDuplicate);
            }
            else
            {
                // Create a new object with the hash and path to the duplicate
                _duplicatedImages.Add(new DuplicatedImage(hash, pathToDuplicate));
            }
        }

        public static void SaveData(string saveDataFilePath)
        {
            string json = JsonConvert.SerializeObject(_duplicatedImages);
            File.WriteAllText(saveDataFilePath, json);
        }

        public static void LoadData(string saveDataFilePath)
        {
            string json = File.ReadAllText(saveDataFilePath);
            _duplicatedImages = JsonConvert.DeserializeObject<List<DuplicatedImage>>(json);

        }

        public static void OutputData(string outputFilePath)
        {
            if(File.ReadLines(outputFilePath).First() == "No duplicates found.")
            {
                File.WriteAllText(outputFilePath, "");
            }

            using (StreamWriter sw = File.AppendText(outputFilePath))
            {
                foreach (var img in _duplicatedImages)
                {
                    sw.WriteLine(img);
                }
            }
        }

        public static string DisplayData()
        {
            string displayOutput = String.Empty;
            foreach (var img in _duplicatedImages)
            {
                displayOutput += img;
            }

            if (displayOutput == String.Empty)
            {
                displayOutput = "No duplicates found.";
            }

            return displayOutput;
        }

        public static void FindOriginals(List<string> filePaths, List<string> hashesFound)
        {
            if (filePaths.Count == hashesFound.Count)
            {
                foreach(var img in _duplicatedImages)
                {
                    int i = hashesFound.IndexOf(img.Hash);
                    string ogPath = filePaths[i];
                    img.OriginalPath = ogPath;
                }
            }
            else
            {
                Console.WriteLine("Note: Can't find original files as filePaths and hashesFound are different in length");
            }
        }

        public static void VerifyDuplicates()
        {
            // List of paths that are not duplicates
            List<string> NotDuplicatePaths = new List<string>();

            for (int i = 0; i < _duplicatedImages.Count; i++)
            {
                string ogPath = _duplicatedImages[i].OriginalPath;
                Bitmap img1 = new Bitmap(ogPath);
                foreach (var duplicatedPath in _duplicatedImages[i].DuplicateImages)
                {
                    Bitmap img2 = new Bitmap(duplicatedPath);

                    if (!AreImagesEqual(img1 , img2))
                    {
                        NotDuplicatePaths.Add(duplicatedPath);
                    }
                }

                // If the "duplicate images" were not actually duplicates
                // If there is just one added, then no possible duplicates
                if (NotDuplicatePaths.Count > 1)
                {
                    DuplicatedImage temp = new DuplicatedImage(_duplicatedImages[i].Hash);
                    temp.OriginalPath = NotDuplicatePaths[0];
                    NotDuplicatePaths.RemoveAt(0);
                    foreach (var path in NotDuplicatePaths)
                    {
                        temp.AddDuplicate(path);
                    }

                    _duplicatedImages.Add(temp);
                }
            }
        }

        private static bool AreImagesEqual(Bitmap bmp1, Bitmap bmp2)
        {
            if (bmp1.Width != bmp2.Width || bmp1.Height != bmp2.Height)
                return false;

            for (int y = 0; y < bmp1.Height; y++)
            {
                for (int x = 0; x < bmp1.Width; x++)
                {
                    if (bmp1.GetPixel(x, y) != bmp2.GetPixel(x, y))
                        return false;
                }
            }
        
            return true;
        }
    }
}
