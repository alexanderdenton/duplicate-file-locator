using DuplicateFileLocatorLibrary.Interfaces;
using DuplicateFileLocatorLibrary.Utilities;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace DuplicateFileLocatorLibrary.Classes
{
    public class DuplicateFileLocator : IDuplicateFileLocator
    {
        #region Private Attributes

        private const string DUPLICATED_IMAGES_JSON = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\duplicated-images.json";
        private const string DEFAULT_TXT_OUTPUT_PATH = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\duplicated-images.txt";

        private List<IDuplicatedFile> _duplicatedFiles;

        #endregion

        #region Constructor

        public DuplicateFileLocator()
        {
            _duplicatedFiles = new List<IDuplicatedFile>();

            LoadData();
        }

        #endregion

        #region Public Methods

        public void FindDuplicateFiles(string dir)
        {
            // Check if path exists.
            if (Path.Exists(dir))
            {
                // List contains all file paths but by the end of the method will only contatin
                // non duplicate file paths (paths to files which are not duplicates)
                List<string> filePaths = GetAllFilesInDirectory(dir);
                int totalFiles = filePaths.Count;

                // Check folder contains files
                if (totalFiles > 0)
                {
                    int filesHashed = 0;
                    // hashesFound stores all the different hashes calculated.
                    List<string> hashesFound = new List<string>();

                    for (int i = 0; i < filePaths.Count; i++)
                    {
                        // Calculate and display progress
                        int progress = (filesHashed * 100) / totalFiles;
                        ConsoleUtility.WriteProgressBar(progress, true);

                        string hash = CreateHash(filePaths[i]);
                        filesHashed++;

                        if (hash != string.Empty)
                        {
                            if (hashesFound.Contains(hash))
                            {
                                AddDuplicateFile(hash, filePaths[i]);
                                // Remove the duplicate path from list
                                filePaths.Remove(filePaths[i]);
                                i--;
                            }
                            else
                            {
                                // As this hash has not beed calculated yet, add to list.
                                hashesFound.Add(hash);
                            }
                        }
                    }

                    ConsoleUtility.WriteProgressBar(100, true);
                    Console.WriteLine("\nTotal Images checked : {0}\nCollating Data now...", filesHashed);

                    // When the duplicate files are added only the hash and duplicate path is known.
                    // Using the filesPaths and hashesFound lists you can locate the original file path
                    // because each index is related.
                    FindOriginalPaths(filePaths, hashesFound);

                    SaveData();
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

        public void DisplayDuplicateFiles()
        {
            throw new NotImplementedException();
        }

        public void VerifyDuplicateFiles()
        {
            throw new NotImplementedException();
        }

        public void ClearDuplicateFiles()
        {
            throw new NotImplementedException();
        }

        public void ExportDuplicateFiles()
        {
            throw new NotImplementedException();
        }

        public void ExportDuplicateFiles(string filePath)
        {
            throw new NotImplementedException();
        }

        public void HashIndividualFile(string filePath)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method recursively gets all the files in the directory.
        /// </summary>
        /// <param name="dir">
        /// Directory Path to be searched.
        /// </param>
        /// <returns>
        /// List of file paths in the directory.
        /// </returns>
        private List<string> GetAllFilesInDirectory(string dir)
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

        /// <summary>
        /// Method converts a byte array to a string.
        /// </summary>
        /// <param name="arrInput">
        /// Byte array to be converted.
        /// </param>
        /// <returns>
        /// String
        /// </returns>
        private string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        /// <summary>
        /// Method creates MD5 hash of file.
        /// </summary>
        /// <param name="filePath">
        /// Path of file to be hashed.
        /// </param>
        /// <returns>
        /// String containing hash value.
        /// </returns>
        private string CreateHash(string filePath)
        {
            try
            {
                Bitmap img = new Bitmap(filePath);

                byte[] tmpSource;
                byte[] tmpHash;

                // Create a byte array from source data.
                MemoryStream stream = new MemoryStream();
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                tmpSource = stream.ToArray();

                // Compute hash based on source data.
                tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);

                return ByteArrayToString(tmpHash);
            }
            catch
            {
                // TODO Add error.
                return string.Empty;
            }
        }

        /// <summary>
        /// Method adds the duplicate file path to the object with the same hash value.
        /// </summary>
        /// <param name="hash">
        /// Hash value to add duplicate path to.
        /// </param>
        /// <param name="filePath">
        /// Path to the duplicate file.
        /// </param>
        private void AddDuplicateFile(string hash, string filePath)
        {
            if (_duplicatedFiles.Any(file => file.Hash == hash))
            {
                // Get the DuplicatedFile object with the same hash value and add duplicate path.
                _duplicatedFiles.FirstOrDefault(file => file.Hash == hash).AddDuplicatePath(filePath);
            }
            else
            {
                // Create a new object with the hash and duplicate path.
                _duplicatedFiles.Add(new DuplicatedFile(hash, filePath));
            }
        }

        /// <summary>
        /// Method loops through duplicate files, uses each file's hash to get an index.
        /// then adds the original file path with than index to the DuplicatedFile object.
        /// </summary>
        /// <param name="filePaths">
        /// List of original file paths, with no duplicates.
        /// </param>
        /// <param name="hashesFound">
        /// List of all the hashes found.
        /// </param>
        private void FindOriginalPaths(List<string> filePaths, List<string> hashesFound)
        {
            if (filePaths.Count == hashesFound.Count)
            {
                foreach (var file in _duplicatedFiles)
                {
                    int i = hashesFound.IndexOf(file.Hash);
                    string ogPath = filePaths[i];
                    file.OriginalPath = ogPath;
                }
            }
            else
            {
                Console.WriteLine("Note: Can't find original files as filePaths and hashesFound are different in length");
            }
        }

        /// <summary>
        /// Method loads data from json file.
        /// </summary>
        private void LoadData()
        {
            string json = File.ReadAllText(DUPLICATED_IMAGES_JSON);
            _duplicatedFiles = JsonConvert.DeserializeObject<List<IDuplicatedFile>>(json);
        }

        /// <summary>
        /// Method saves data to json file.
        /// </summary>
        private void SaveData()
        {
            string json = JsonConvert.SerializeObject(_duplicatedFiles);
            File.WriteAllText(DUPLICATED_IMAGES_JSON, json);
        }

        #endregion
    }
}
