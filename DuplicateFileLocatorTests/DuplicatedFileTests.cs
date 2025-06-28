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

        [TestCase("BC1173AE1C119128CCCA7190BDD16FAF")]
        [TestCase("38357FEEAA7392183DD16A34FE681100")]
        [TestCase("769F0CACE7B957755C9B770CAE24D88F")]
        [Category("Constructor Tests")]
        public void DuplicatedFile_InitialisesHash(string fileHash)
        {
            IDuplicatedFile duplicatedFile = new DuplicatedFile(fileHash);

            string hash = duplicatedFile.Hash;

            Assert.That(hash, Is.EqualTo(fileHash));

        }

        [TestCase("BC1173AE1C119128CCCA7190BDD16FAF", "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\og\\P1040133.JPG")]
        [TestCase("38357FEEAA7392183DD16A34FE681100", "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name\\20240809_152410.jpg")]
        [TestCase("769F0CACE7B957755C9B770CAE24D88F", "C:\\Users\\Alexa\\OneDrive\\Denton Family Photos\\2004\\2004-07\\ALEX 03.07.04 026.jpg")]
        [Category("Constructor Tests")]
        public void DuplicatedFile_InitialisesHashAndDuplicatePath(string fileHash, string filePath)
        {
            IDuplicatedFile duplicatedFile = new DuplicatedFile(fileHash, filePath);

            string hash = duplicatedFile.Hash;
            int numDuplicatePaths = duplicatedFile.DuplicatePaths.Count;
            string duplicateFilePath = duplicatedFile.DuplicatePaths.First();

            Assert.Multiple(() =>
            {
                Assert.That(hash, Is.EqualTo(fileHash));
                Assert.That(numDuplicatePaths, Is.EqualTo(1));
                Assert.That(duplicateFilePath, Is.EqualTo(filePath));
            });
        }

        [TestCase("BC1173AE1C119128CCCA7190BDD16FAF", "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\og\\P1040133 - Copy.JPG", 
            new string[] 
            { 
                "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\og\\P1040133.JPG",
                "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name\\P1040133.JPG",
                "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name_diff_date\\P1040133.JPG" 
            })]
        [TestCase("38357FEEAA7392183DD16A34FE681100", "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\og\\20240809_152410.jpg", 
            new string[]
            {
                "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files\\same_name\\20240809_152410.jpg"
            })]
        [TestCase("769F0CACE7B957755C9B770CAE24D88F", "C:\\Users\\Alexa\\OneDrive\\Denton Family Photos\\2004\\2004-07\\ALEX 03.07.04 017.jpg", 
            new string[]
            {
                "C:\\Users\\Alexa\\OneDrive\\Denton Family Photos\\2004\\2004-07\\ALEX 03.07.04 026.jpg"
            })]
        [Category("Constructor Tests")]
        public void DuplicatedFile_InitialisesHashOriginalPathAndDuplicatePath(string fileHash, string filePath, string[] duplicatePaths)
        {
            List<string> duplicates = new List<string>(duplicatePaths);
            IDuplicatedFile duplicatedFile = new DuplicatedFile(fileHash, filePath, duplicates);

            string hash = duplicatedFile.Hash;
            string originalPath = duplicatedFile.OriginalPath;
            int numDuplicatePaths = duplicatedFile.DuplicatePaths.Count;

            Assert.Multiple(() =>
            {
                Assert.That(hash, Is.EqualTo(fileHash));
                Assert.That(originalPath, Is.EqualTo(filePath));
                Assert.That(numDuplicatePaths, Is.EqualTo(duplicatePaths.Length));
            });
        }

        #endregion

        #region Setter Tests

        #endregion

        #region Public Method Tests

        #endregion
    }
}
