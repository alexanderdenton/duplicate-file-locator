using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duplicate_file_locator
{
    public class DuplicatedImage
    {
        private readonly string _path;
        private List<string> _duplicateImages;

        public DuplicatedImage() 
        {
            _path = new string("");
            _duplicateImages = new List<string>();
        }

        public DuplicatedImage(string path) : this()
        {
            _path = path;
        }

        public DuplicatedImage(string path, string duplicatePath) : this(path)
        {
            _duplicateImages.Add(duplicatePath);
        }

        public void AddDuplicate(string path)
        {
            if (!_duplicateImages.Contains(path))
                _duplicateImages.Add(path);
        }

        public string GetOriginal()
        {
            return _path;
        }

        public List<string> GetDuplicates()
        {
            return _duplicateImages;
        }

        public override string ToString()
        {
            string output = "Original : " + _path + "\nDuplicates : \n";
            foreach (string image in _duplicateImages)
            {
                output += "\t\t" + image + "\n";
            }
            return output;
        }
    }
}
