using System;
using System.IO;
using System.Linq;

namespace BIOS_Filter_for_ASUS
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the current directory where the program is located
            string currentDir = Directory.GetCurrentDirectory();

            // Get the parent directory where the BIOS files are located
            string parentDir = Directory.GetParent(currentDir).FullName;

            // Get the subdirectories for new and old BIOS files
            string newDir = Path.Combine(parentDir, "NEW_BIOS_FILES");
            string oldDir = Path.Combine(parentDir, "OLD_BIOS_FILES");

            // Create the subdirectories if they don't exist
            Directory.CreateDirectory(newDir);
            Directory.CreateDirectory(oldDir);

            // Get all the BIOS files in the parent directory
            string[] biosFiles = Directory.GetFiles(parentDir, "*.*");

            // Group the BIOS files by model name
            var groups = biosFiles.GroupBy(f => Path.GetFileNameWithoutExtension(f).Split('.')[0]);

            // Loop through each group of BIOS files
            foreach (var group in groups)
            {
                // Get the model name
                string model = group.Key;

                // Check if the group has more than one file
                if (group.Count() > 1)
{
                    // Get the latest BIOS file by version number
                    string latestFile = group.OrderByDescending(f => int.Parse(Path.GetExtension(f).TrimStart('.'))).First();

                    // Copy the latest BIOS file to the new directory
                    File.Copy(latestFile, Path.Combine(newDir, Path.GetFileName(latestFile)), true);

                    // Move the other BIOS files to the old directory
                    foreach (string file in group.Except(new[] { latestFile }))
                    {
                        File.Move(file, Path.Combine(oldDir, Path.GetFileName(file)), true);
                    }
                }
            }

            // Print a message to indicate the program is done
            Console.WriteLine("The program has finished updating the BIOS files. Hit Enter to close the window.");
            Console.ReadLine();
        }
    }
}