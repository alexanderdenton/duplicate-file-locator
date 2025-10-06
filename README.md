Duplicate File Locator
======================

The Purpose of this project is to help find duplicate images within your directories. This is done recursively through your directories.

The algorithm gets a list of all the file paths for your images, gets the bitmap data then hashes it. These hashes are then compared to each other. Each time you hash a new file, it is checked to see if it's contained within the known hashes array. If it is found then you have a duplicated hash. If not, it's added to the known hashes array.

At the end you'll have an array of hashes with no duplicates and a structured array of file paths to duplicate files.

Project Structure
-----------------

 - **DuplicateFileLocatorConsole:** This is the command line interface for the application.
 - **DuplicateFileLocatorLibrary:** Contains the class structures and algorithms for locating the duplicate files.
 - **DuplicateFileLocatorTests:** Unit tests created for the Library.
 - **ProofOfConcept:** First draft of the application. In main branch for reference and proof of how the application has evolved.
