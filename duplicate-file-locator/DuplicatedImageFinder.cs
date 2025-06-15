using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace duplicate_file_locator
{
    public static class DuplicatedImageFinder
    {
        private static List<DuplicatedImage> _duplicatedImages = new List<DuplicatedImage>();

        public static void AddHash(string hash, string pathToDuplicate)
        {
            if (_duplicatedImages.Any(img => img.GetHash() == hash))
            {
                // Get the DuplicatedImage object with the same hash value and add the path to the duplicate
                _duplicatedImages.FirstOrDefault(img => img.GetHash() == hash).AddDuplicate(pathToDuplicate);
            }
            else
            {
                // Create a new object with the hash and path to the duplicate
                _duplicatedImages.Add(new DuplicatedImage(hash, pathToDuplicate));
            }
        }

        public static void SaveData(string saveFilePath)
        {
            if(File.ReadLines(saveFilePath).First() == "No duplicates found.")
            {
                File.WriteAllText(saveFilePath, "");
            }

            using (StreamWriter sw = File.AppendText(saveFilePath))
            {
                foreach (var img in _duplicatedImages)
                {
                    sw.WriteLine(img);
                }
            }
        }

        public static void FindOriginals(List<string> filePaths, List<string> hashesFound)
        {
            if (filePaths.Count == hashesFound.Count)
            {
                foreach(var img in _duplicatedImages)
                {
                    int i = hashesFound.IndexOf(img.GetHash());
                    string ogPath = filePaths[i];
                    img.AddOriginalPath(ogPath);
                }
            }
            else
            {
                Console.WriteLine("Note: Can't find original files as filePaths and hashesFound are different in length");
            }
        }
    }
}
