using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileLocatorLibrary.Interfaces
{
    public interface IDuplicatedFile
    {
        #region Attributes

        /// <summary>
        /// Hash value of file.
        /// </summary>
        string Hash { get; }
        
        /// <summary>
        /// Path to first file found.
        /// </summary>
        string OriginalPath { get; set; }

        /// <summary>
        /// List of paths to duplicate files.
        /// </summary>
        List<string> DuplicatePaths { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Method adds path to <c>DuplicatePaths</c>.
        /// </summary>
        /// <param name="path"></param>
        void AddDuplicatePath(string path);

        #endregion
    }
}
