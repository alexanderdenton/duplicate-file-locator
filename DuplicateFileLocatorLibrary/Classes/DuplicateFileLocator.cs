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

        private const string DUPLICATED_FILES_JSON = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\duplicated-images.json";
        private const string DEFAULT_TXT_OUTPUT_PATH = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\duplicated-images.txt";

        // This is used to replace DUPLICATED_FILES_JSON
        private string _duplicatedFilesJson;

        private List<IDuplicatedFile> _duplicatedFiles;

        #endregion

        #region Constructor

        /// <summary>
        /// Method initalises attributes and loads data.
        /// </summary>
        public DuplicateFileLocator()
        {
            _duplicatedFiles = new List<IDuplicatedFile>();

            LoadData();
        }

        public DuplicateFileLocator(string jsonPath)
        {
            _duplicatedFilesJson = jsonPath;

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

                        if (CheckHash(hash, hashesFound))
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

                    ConsoleUtility.WriteProgressBar(100, true);
                    Console.WriteLine("\nTotal Images checked : {0}\nCollating Data now...\n", filesHashed);

                    // When the duplicate files are added only the hash and duplicate path is known.
                    // Using the filesPaths and hashesFound lists you can locate the original file path
                    // because each index is related.
                    // filePaths only contains paths of files which aren't duplicates and the original paths of the duplicates.
                    // hashesFound only contains hashes never found before at the time of creation of the hash
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

        public void VerifyDuplicateFiles()
        {
            Console.WriteLine("Verifying duplicate files found...\n");

            // List of paths that are not duplicates
            List<string> notDuplicatePaths = new List<string>();

            for (int i = 0; i < _duplicatedFiles.Count; i++)
            {
                string ogPath = _duplicatedFiles[i].OriginalPath;
                Bitmap img1 = new Bitmap(ogPath);
                foreach (var duplicatedPath in _duplicatedFiles[i].DuplicatePaths)
                {
                    Bitmap img2 = new Bitmap(duplicatedPath);

                    if (!AreImagesEqual(img1, img2))
                    {
                        // Images are not duplicates.
                        notDuplicatePaths.Add(duplicatedPath);
                    }
                }

                // If the "duplicate files" were not actually duplicates
                // If there is just one added, then no other duplicates
                if (notDuplicatePaths.Count > 1)
                {
                    DuplicatedFile temp = new DuplicatedFile(_duplicatedFiles[i].Hash);
                    temp.OriginalPath = notDuplicatePaths[0];
                    notDuplicatePaths.RemoveAt(0);
                    foreach (var path in notDuplicatePaths)
                    {
                        temp.AddDuplicatePath(path);
                    }

                    _duplicatedFiles.Add(temp);
                    // Clear array otherwise you'll keep adding to it for the new file.
                    // Only file paths with the same hash should be stored in the array 
                    // at a given time.
                    notDuplicatePaths = new List<string>();
                }
            }
        }

        public void DisplayDuplicateFiles()
        {
            Console.WriteLine(ToString());
        }

        public void ClearDuplicateFiles()
        {
            if (_duplicatedFilesJson != null)
            {
                using (StreamWriter sw = File.CreateText(_duplicatedFilesJson))
                {
                    sw.WriteLine();
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(DUPLICATED_FILES_JSON))
                {
                    sw.WriteLine();
                }
            }
                
            Console.WriteLine("Duplicate file cleared.\n");
        }

        public void ExportDuplicateFiles()
        {
            using (StreamWriter sw = File.CreateText(DEFAULT_TXT_OUTPUT_PATH))
            {
                sw.WriteLine(ToString());
            }
            Console.WriteLine("Duplicate file exported.\n");
        }

        public void ExportDuplicateFiles(string filePath)
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.WriteLine(ToString());
            }
            Console.WriteLine("Duplicate file exported.\n");
        }

        public void HashIndividualFile(string filePath)
        {
            if (Path.Exists(filePath))
            {
                string hash = CreateHash(filePath);
                Console.WriteLine("Hash : {0}\n", hash);
            }
            else
            {
                Console.WriteLine("Path does not exist, please try again.\n");
            }
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
                IDuplicatedFile duplicateFile = _duplicatedFiles.FirstOrDefault(file => file.Hash == hash);

                // Only adds file path if it's different to what's already be found.
                if (duplicateFile.OriginalPath != filePath && !duplicateFile.DuplicatePaths.Contains(filePath))
                {
                    duplicateFile.AddDuplicatePath(filePath);
                }
                
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
                    // only find original path if the duplicate file object doesn't have one
                    if (file.OriginalPath == string.Empty)
                    {
                        int i = hashesFound.IndexOf(file.Hash);

                        // Only add the ogPath if hash exists
                        if (i >= 0)
                        {
                            string ogPath = filePaths[i];
                            file.OriginalPath = ogPath;
                        }
                    }
                    
                }
            }
            else
            {
                Console.WriteLine("Note: Can't find original files as filePaths and hashesFound are different in length.\n");
            }
        }

        /// <summary>
        /// Method loads data from json file.
        /// </summary>
        private void LoadData()
        {
            string json = string.Empty;
            if (_duplicatedFilesJson != null)
            {
                json = File.ReadAllText(_duplicatedFilesJson);
            }
            else
            {
                json = File.ReadAllText(DUPLICATED_FILES_JSON);
            }

            try
            {
                var deserialized = JsonConvert.DeserializeObject<List<DuplicatedFile>>(json);
                _duplicatedFiles = deserialized.Cast<IDuplicatedFile>().ToList();
            }
            catch
            {

            }
        }

        /// <summary>
        /// Method saves data to json file.
        /// </summary>
        private void SaveData()
        {
            string json = JsonConvert.SerializeObject(_duplicatedFiles);

            if (_duplicatedFilesJson != null)
            {
                File.WriteAllText(_duplicatedFilesJson, json);
            }
            else
            {
                File.WriteAllText(DUPLICATED_FILES_JSON, json);
            }
        }

        /// <summary>
        /// Method converts list of duplicated files to a single string.
        /// </summary>
        /// <returns></returns>
        private string ToString()
        {
            string output = String.Empty;
            foreach (var file in _duplicatedFiles)
            {
                output += file + "\n";
            }

            if (output == String.Empty)
            {
                output = "No duplicate files found.";
            }

            return output;
        }

        /// <summary>
        /// Method checks size of bitmap then checks each pixel.
        /// </summary>
        /// <param name="bmp1">
        /// Bitmap image.
        /// </param>
        /// <param name="bmp2">
        /// Second bitmap image to compare against.
        /// </param>
        /// <returns>
        /// True if images are the same.
        /// </returns>
        private bool AreImagesEqual(Bitmap bmp1, Bitmap bmp2)
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

        /// <summary>
        /// Methods checks if hash has been found in known duplicates, or within folder being searched.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="hashesFound"></param>
        /// <returns></returns>
        private bool CheckHash(string hash, List<string> hashesFound)
        {
            if (hash == string.Empty)
                return false;
            else
                // returns true if hash is already a known duplicate or if the hash is contained within
                // the temporary list of known hashes.
                return _duplicatedFiles.Any(file => file.Hash == hash) || hashesFound.Contains(hash);
        }

        #endregion
    }
}
