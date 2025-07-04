using DuplicateFileLocatorLibrary.Classes;
using DuplicateFileLocatorLibrary.Interfaces;
using NUnit.Framework;

namespace DuplicateFileLocatorTests
{
    [TestFixture]
    public class DuplicateFileLocatorTests
    {
        [Test]
        public void DuplicateFileLocator_Constuctor()
        {
            string testJson = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test.json";
            using (StreamWriter sw = File.CreateText(testJson))
            {
                sw.WriteLine();
            }

            IDuplicateFileLocator duplicateFileLocator = new DuplicateFileLocator(testJson);
            
            Assert.That(duplicateFileLocator, Is.Not.Null);
        }

        [Test]
        public void FindDuplicateFiles()
        {
            string testJson = "C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test.json";
            using (StreamWriter sw = File.CreateText(testJson))
            {
                sw.WriteLine();
            }

            IDuplicateFileLocator duplicateFileLocator = new DuplicateFileLocator(testJson);

            duplicateFileLocator.FindDuplicateFiles("C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test_files");

            string jsonResults = string.Empty;
            using (StreamReader sr = new StreamReader("C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test.json"))
            {
                jsonResults = sr.ReadToEnd();
            }

            string jsonExpected = string.Empty;
            using (StreamReader sr = new StreamReader("C:\\Users\\Alexa\\repos\\duplicate-file-locator\\test-expected.json"))
            {
                jsonExpected = sr.ReadToEnd();
            }

            Assert.That(jsonResults, Is.EqualTo(jsonExpected));
        }
    }
}
