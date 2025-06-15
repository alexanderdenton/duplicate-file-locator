using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duplicate_file_locator
{
    public class DuplicatedImage
    {
        private string _hash;
        private string _originalPath;
        private List<string> _duplicateImages;

        public DuplicatedImage() 
        {
            _hash = string.Empty;
            _originalPath = string.Empty;
            _duplicateImages = new List<string>();
        }

        public DuplicatedImage(string hash) : this()
        {
            _hash = hash;
        }

        public DuplicatedImage(string hash, string duplicatePath) : this(hash)
        {
            _duplicateImages.Add(duplicatePath);
        }

        public void AddOriginalPath(string path)
        {
            _originalPath = path;
        }

        public void AddDuplicate(string path)
        {
            if (!_duplicateImages.Contains(path))
                _duplicateImages.Add(path);
        }

        public string GetHash() 
        { 
            return _hash; 
        }

        public string GetOriginalPath()
        {
            return _originalPath;
        }

        public List<string> GetDuplicates()
        {
            return _duplicateImages;
        }

        public override string ToString()
        {
            string output = "Hash : " + _hash + "\nOriginal : " + _originalPath + "\nDuplicates : \n";
            foreach (string image in _duplicateImages)
            {
                output += "\t\t" + image + "\n";
            }
            return output;
        }
    }
}
