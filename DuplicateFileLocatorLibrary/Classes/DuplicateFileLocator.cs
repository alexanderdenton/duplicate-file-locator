using DuplicateFileLocatorLibrary.Interfaces;
using DuplicateFileLocatorLibrary.Utilities;

namespace DuplicateFileLocatorLibrary.Classes
{
    public class DuplicateFileLocator : IDuplicateFileLocator
    {
        #region Private Attributes

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

        public void FindDuplicateFiles(string pathToFolder)
        {
            // Check if path exists.
            if (Path.Exists(pathToFolder))
            {
                // List contains all file paths but by the end of the method will only contatin
                // non duplicate file paths (paths to files which are not duplicates)
                List<string> filePaths = GetAllFilesInDirectory(pathToFolder);
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

        public void ExportDuplicateFiles(string pathToFile)
        {
            throw new NotImplementedException();
        }

        public void HashIndividualFile(string pathToFile)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private List<string> GetAllFilesInDirectory(string pathToFolder)
        {
            return new List<string>();
        }

        private string CreateHash(string filePath)
        {
            return string.Empty;
        }

        private void AddDuplicateFile(string hash, string filePath)
        {

        }

        private void FindOriginalPaths(List<string> filePaths, List<string> hashesFound)
        {

        }

        private void SaveData()
        {

        }

        private void LoadData()
        {

        }

        #endregion
    }
}
