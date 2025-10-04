using System.IO;
using MonoMod.Utils;

namespace Celeste.Mod.TyporiumUtilities_DEV.Utilities.Saves
{


    public static class SavesFunctions
    {


        // Get the name file without the save's index
        private static string GetFileNameExceptIndex(string filename)
        {
            int index = filename.IndexOf("-");
            return filename.Substring(index);
        }


        // Swaps two save files
        public static void SwapFile(int slotindex1, int slotindex2, OuiFileSelect OuiInstance, string TemporaryDirectoryname)
        {

            // Creates a temporary directory to swap files
            Directory.CreateDirectory(Everest.PathSettings + "/" + TemporaryDirectoryname);

            // Gets the path of all the saves' files
            DirectoryInfo saves_directory = new DirectoryInfo(Everest.PathSettings);

            // Move and rename all files related to save of index "slotindex1" to the temporary directory
            string oldname;
            string newname;

            foreach (var fileinfo in saves_directory.EnumerateFiles(slotindex1.ToString() + "-*.celeste"))
            {
                string newfilename = slotindex2.ToString() + GetFileNameExceptIndex(fileinfo.Name);
                fileinfo.MoveTo(Everest.PathSettings + "/" + TemporaryDirectoryname + "/" + newfilename);
            }
            oldname = Everest.PathSettings + "/" + slotindex1.ToString() + ".celeste";
            newname = Everest.PathSettings + "/" + TemporaryDirectoryname + "/" + slotindex2.ToString() + ".celeste";
            if (File.Exists(oldname))
            {
                File.Move(oldname, newname);
            }

            // Rename all files related to save of index "slotindex2"
            foreach (var fileinfo in saves_directory.EnumerateFiles(slotindex2.ToString() + "-*.celeste"))
            {
                string newfilename = slotindex1.ToString() + GetFileNameExceptIndex(fileinfo.Name);
                File.Move(fileinfo.FullName, Everest.PathSettings + "/" + newfilename);
            }
            oldname = Everest.PathSettings + "/" + slotindex2.ToString() + ".celeste";
            newname = Everest.PathSettings + "/" + slotindex1.ToString() + ".celeste";
            if (File.Exists(oldname))
            {
                File.Move(oldname, newname);
            }

            // Moves back all files from temporary directory to saves' directory
            DirectoryInfo temporary_directory = new DirectoryInfo(Everest.PathSettings + "/" + TemporaryDirectoryname);
            foreach (var fileinfo in temporary_directory.EnumerateFiles())
            {
                File.Move(fileinfo.FullName, Everest.PathSettings + "/" + fileinfo.Name);
            }

            // Deletes temporary directory
            temporary_directory.Delete(true);


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