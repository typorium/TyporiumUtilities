using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MonoMod.Utils;

namespace Celeste.Mod.TyporiumUtilities_DEV.Utilities
{


    public class SavesFunctions
    {


        // DirectoryInfo object referencing the Saves folder
        private static DirectoryInfo SavesDirectoryInfo { get; set; } = new DirectoryInfo(Everest.PathSettings);



        // Deletes all the files of a save
        public static void DeleteFile(int slotindex)
        {

            // Delete the vanilla save of slot index (1.celeste if slotindex equals 1)
            File.Delete(SavesDirectoryInfo.FullName + slotindex.ToString() + ".celeste");

            // Delete all files of format slotindex-*.celeste (1-*.celeste if slotindex equals 1)
            foreach (var file in SavesDirectoryInfo.EnumerateFiles(slotindex.ToString() + "-*.celeste"))
            {
                file.Delete();
            }

        }

        // Checks if a save at index "slotindex" exists in the folder
        public static bool FilesExist(int slotindex)
        {
            string regex = GetRegexForSlot(slotindex);
            return SavesDirectoryInfo.EnumerateFiles().Where(file => Regex.IsMatch(file.Name, regex)).ToList().Count != 0;
        }

        // Get a file's name without the index of the save
        private static string GetFileNameWithoutSlotIndex(string filename)
        {
            int index = filename.IndexOf("-");
            if (index == -1)
            {
                index = filename.IndexOf(".");
            }
            return filename.Substring(index);
        }

        // Get the slot index contained in the file's name
        private static int GetSlotIndexWithoutFileName(string filename)
        {
            int index = filename.IndexOf("-");
            if (index == -1)
            {
                index = filename.IndexOf(".");
            }

            int slotindex = -1;
            int.TryParse(filename.Substring(0, index), out slotindex);

            return slotindex;
        }

        // Get the index of the highest save referenced in the files
        public static int GetHighestSlot()
        {
            string regex = GetRegexForSlot(null);
            int maxslot = -1;

            foreach (var file in SavesDirectoryInfo.EnumerateFiles().Where(file => Regex.IsMatch(file.Name, regex)))
            {
                maxslot = Math.Max(maxslot, GetSlotIndexWithoutFileName(file.Name));
            }

            return maxslot;
        }

        // Gets the RegEx pattern used to get all files related to save of index "slotindex". If slotindex is null, all files of all saves will be taken
        private static string GetRegexForSlot(int? slotindex)
        {
            if (slotindex == null)
            {
                return @"^(\d+)(-mod(session|savedata|settings|save)(-([^.]+))?)?.celeste";
            }
            return "^(" + slotindex.ToString() + @")(-mod(session|savedata|settings|save)(-([^.]+))?)?.celeste";
        }

        // Swaps all files that reference slotindex1 and slotindex2
        public static void SwapFile(int slotindex1, int slotindex2, OuiFileSelect OuiInstance)
        {

            // Get regexs for slots
            string regex_slot1 = GetRegexForSlot(slotindex1);
            string regex_slot2 = GetRegexForSlot(slotindex2);

            // Creates a temporary directory to swap files
            string temporary_folder = "TyporiumUtilities_SavesFunction_SwapFiles";
            SavesDirectoryInfo.CreateSubdirectory(temporary_folder);

            // Move and rename all files related to save of index "slotindex1" to the temporary directory
            foreach (var fileinfo in SavesDirectoryInfo.EnumerateFiles().Where(file => Regex.IsMatch(file.Name, regex_slot1)))
            {
                string newfilename = $"{slotindex2}{GetFileNameWithoutSlotIndex(fileinfo.Name)}";
                fileinfo.MoveTo($"{SavesDirectoryInfo.FullName}/{temporary_folder}/{newfilename}");
            }

            // Rename all files related to save of index "slotindex2"
            foreach (var fileinfo in SavesDirectoryInfo.EnumerateFiles().Where(file => Regex.IsMatch(file.Name, regex_slot2)))
            {
                string newfilename = $"{slotindex1}{GetFileNameWithoutSlotIndex(fileinfo.Name)}";
                File.Move(fileinfo.FullName, $"{SavesDirectoryInfo.FullName}/{newfilename}");
            }

            // Moves back all files from temporary directory to saves' directory
            DirectoryInfo temporary_folder_directoryinfo = new DirectoryInfo($"{SavesDirectoryInfo.FullName}/{temporary_folder}");
            foreach (var fileinfo in temporary_folder_directoryinfo.EnumerateFiles())
            {
                File.Move(fileinfo.FullName, $"{SavesDirectoryInfo.FullName}/{fileinfo.Name}");
            }

            // Deletes temporary directory
            temporary_folder_directoryinfo.Delete(true);


            // Swaps slots (UI related)
            if (OuiInstance == null)
            {
                return;
            }

            OuiInstance.Slots[slotindex1].FileSlot = slotindex2;
            OuiInstance.Slots[slotindex2].FileSlot = slotindex1;

            OuiFileSelectSlot tmp = OuiInstance.Slots[slotindex1];
            OuiInstance.Slots[slotindex1] = OuiInstance.Slots[slotindex2];
            OuiInstance.Slots[slotindex2] = tmp;

            OuiInstance.SlotIndex = slotindex2;
            foreach (OuiFileSelectSlot ouiFileSelectSlot in OuiInstance.Slots)
            {
                DynamicData dyndata = DynamicData.For(ouiFileSelectSlot);
                dyndata.Invoke("ScrollTo", ouiFileSelectSlot.IdlePosition.X, ouiFileSelectSlot.IdlePosition.Y);
            }
        }
    }
}