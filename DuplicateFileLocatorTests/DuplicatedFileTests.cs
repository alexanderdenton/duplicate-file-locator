using DuplicateFileLocatorLibrary.Classes;
using DuplicateFileLocatorLibrary.Interfaces;
using NUnit.Framework;

namespace DuplicateFileLocatorTests
{
    [TestFixture]
    public class DuplicatedFileTests
    {
        #region Constructor Tests

        [Test]
        [Category("Constructor Tests")]
        public void DuplicatedFile_BasicConstructor()
        {
            // Arrange
            IDuplicatedFile duplicatedFile = new DuplicatedFile();

            // Act
            string hash = duplicatedFile.Hash;
            string originalPath = duplicatedFile.OriginalPath;
            List<string> duplicatedPaths = duplicatedFile.DuplicatePaths;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(hash, Is.EqualTo(string.Empty));
                Assert.That(originalPath, Is.EqualTo(string.Empty));
                Assert.That(duplicatedPaths, Is.Empty);
            });
        }

        #endregion

        #region Setter Tests

        #endregion

        #region Public Method Tests

        #endregion
    }
}
