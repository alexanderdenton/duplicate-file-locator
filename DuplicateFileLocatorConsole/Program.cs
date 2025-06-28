using DuplicateFileLocatorLibrary.Classes;
using DuplicateFileLocatorLibrary.Interfaces;

namespace DuplicateFileLocatorConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDuplicateFileLocator duplicateFileLocator = new DuplicateFileLocator();

            if (args.Length != 0)
            {
                if (args[0] == "-f" || args[0] == "--find")
                {
                    if (args.Length > 1)
                    {
                        string pathToFolder = args[1];
                        duplicateFileLocator.FindDuplicateFiles(pathToFolder);
                    }
                    else
                    {
                        Console.WriteLine("Path to folder is required");
                    }
                }
                else if (args[0] == "-d" || args[0] == "--display-duplicate-log")
                {
                    duplicateFileLocator.DisplayDuplicateFiles();
                }
                else if (args[0] == "-c" || args[0] == "--clear-duplicate-log")
                {
                    duplicateFileLocator.ClearDuplicateFiles();
                }
                else if (args[0] == "-e" || args[0] == "--export-duplicate-log")
                {
                    if (args.Length > 1)
                    {
                        string exportPath = args[1];
                        duplicateFileLocator.ExportDuplicateFiles(exportPath);
                    }
                    else
                    {
                        duplicateFileLocator.ExportDuplicateFiles();
                    }
                }
                else if (args[0] == "-v" || args[0] == "--verify-duplicate-log")
                {
                    duplicateFileLocator.VerifyDuplicateFiles();
                }
                else if (args[0] == "-h" || args[0] == "--hash-file")
                {
                    if (args.Length > 1)
                    {
                        string pathToFile = args[1];
                        duplicateFileLocator.HashIndividualFile(pathToFile);
                    }
                    else
                    {
                        Console.WriteLine("Path to file is required");
                    }
                }
            }
            else
            {
                // TODO: Add Help info
                Console.WriteLine("No arguments Inputted");
            }

        }
    }
}
