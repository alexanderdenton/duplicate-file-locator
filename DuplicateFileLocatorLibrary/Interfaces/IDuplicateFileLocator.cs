using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileLocatorLibrary.Interfaces
{
    public interface IDuplicateFileLocator
    {
        #region Methods

        /// <summary>
        /// Method searches recursively to find duplicated files.
        /// </summary>
        /// <param name="pathToFolder">
        /// Path to the folder to be searched.
        /// </param>
        void FindDuplicateFiles(string pathToFolder);

        /// <summary>
        /// Methods displays the duplicate files found including the original path.
        /// </summary>
        void DisplayDuplicateFiles();

        /// <summary>
        /// Method verifies that the files are in fact duplicates.
        /// This is needed as different files might create the same hash.
        /// </summary>
        void VerifyDuplicateFiles();

        /// <summary>
        /// Method deletes all the data relating to duplicate files found.
        /// </summary>
        void ClearDuplicateFiles();

        /// <summary>
        /// Method exports the duplicate data in the same format as <see cref="DisplayDuplicateFiles">DisplayDuplicateFiles</see>.
        /// </summary>
        void ExportDuplicateFiles();

        /// <summary>
        /// Method exports the duplicate data in the same format as <see cref="DisplayDuplicateFiles">DisplayDuplicateFiles</see>.
        /// </summary>
        /// <param name="pathToFile">
        /// Path to the file to export data to.
        /// </param>
        void ExportDuplicateFiles(string pathToFile);

        /// <summary>
        /// Method hashes an individual file.
        /// </summary>
        /// <param name="pathToFile">
        /// Path to file to be hashed.
        /// </param>
        void HashIndividualFile(string pathToFile);

        #endregion
    }
}
