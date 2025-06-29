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
                //Console.WriteLine(args[0]);
                if (args[0] == "-f" || args[0] == "--find")
                {
                    //Console.WriteLine("Option Find");
                    if (args.Length > 1)
                    {
                        //Console.WriteLine(args[1]);
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
                    //Console.WriteLine("Option Display");
                    duplicateFileLocator.DisplayDuplicateFiles();
                }
                else if (args[0] == "-c" || args[0] == "--clear-duplicate-log")
                {
                    //Console.WriteLine("Option Clear");
                    duplicateFileLocator.ClearDuplicateFiles();
                }
                else if (args[0] == "-e" || args[0] == "--export-duplicate-log")
                {
                    //Console.WriteLine("Option Export");
                    if (args.Length > 1)
                    {
                        //Console.WriteLine(args[1]);
                        string exportPath = args[1];
                        duplicateFileLocator.ExportDuplicateFiles(exportPath);
                    }
                    else
                    {
                        //Console.WriteLine("No Path");
                        duplicateFileLocator.ExportDuplicateFiles();
                    }
                }
                else if (args[0] == "-v" || args[0] == "--verify-duplicate-log")
                {
                    //Console.WriteLine("Option Verify");
                    duplicateFileLocator.VerifyDuplicateFiles();
                }
                else if (args[0] == "-h" || args[0] == "--hash-file")
                {
                    Console.WriteLine("Option Hash");
                    if (args.Length > 1)
                    {
                        //Console.WriteLine(args[1]);
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
