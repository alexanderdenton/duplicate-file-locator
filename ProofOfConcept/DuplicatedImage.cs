using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ProofOfConcept
{
    public class DuplicatedImage
    {
        private string _hash;
        private string _originalPath;
        private List<string> _duplicateImages;

        [JsonProperty("Hash")]
        public string Hash
        {
            get { return _hash; }
            private set { _hash = value; }
        }

        [JsonProperty("OriginalPath")]
        public string OriginalPath
        {
            get { return _originalPath; }
            set { _originalPath = value; }
        }

        [JsonProperty("DuplicateImages")]
        public List<string> DuplicateImages
        {
            get { return _duplicateImages; }
        }

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
            DuplicateImages.Add(duplicatePath);
        }

        [JsonConstructor]
        public DuplicatedImage(string hash, string originalPath, List<string> duplicateImages) : this(hash)
        {
            _originalPath = originalPath;
            _duplicateImages = duplicateImages;
        }

        public void AddDuplicate(string path)
        {
            if (!_duplicateImages.Contains(path))
                _duplicateImages.Add(path);
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
