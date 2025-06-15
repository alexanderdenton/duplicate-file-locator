using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            using (StreamWriter sw = File.AppendText(saveFilePath))
            {
                foreach (var img in _duplicatedImages)
                {
                    sw.WriteLine(img);
                }
            }
        }
    }
}
