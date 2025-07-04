using DuplicateFileLocatorLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileLocatorLibrary.Classes
{
    public class DuplicatedFile : IDuplicatedFile
    {
        #region Private Attributes

        private string _hash;
        private string _originalPath;
        private List<string> _duplicatePaths;

        #endregion

        #region Public Attributes

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

        [JsonProperty("DuplicatePaths")]
        public List<string> DuplicatePaths
        {
            get { return _duplicatePaths; }
            private set { _duplicatePaths = value; }
        }

        #endregion

        #region Constructors

        public DuplicatedFile()
        {
            Hash = string.Empty;
            OriginalPath = string.Empty;
            DuplicatePaths = new List<string>();
        }

        public DuplicatedFile(string hash) : this()
        {
            Hash = hash;
        }

        public DuplicatedFile(string hash, string duplicatePath) : this(hash)
        {
            DuplicatePaths.Add(duplicatePath);
        }

        [JsonConstructor]
        public DuplicatedFile(string hash, string originalPath, List<string> duplicatePaths) : this(hash)
        {
            OriginalPath = originalPath;
            if(duplicatePaths != null)
            {
                foreach (var path in duplicatePaths)
                {
                    DuplicatePaths.Add(path);
                }
            }
        }

        #endregion

        #region Public Methods

        public void AddDuplicatePath(string path)
        {
            if (!DuplicatePaths.Contains(path))
                DuplicatePaths.Add(path);
        }

        public override string ToString()
        {
            string output = "Hash : " + Hash + "\nOriginal : " + OriginalPath + "\nDuplicates : \n";
            foreach (string image in DuplicatePaths)
            {
                output += "\t\t" + image + "\n";
            }
            return output;
        }

        #endregion
    }
}
