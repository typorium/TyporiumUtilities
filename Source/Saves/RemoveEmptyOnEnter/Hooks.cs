using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;
using FMOD.Studio;

namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.RemoveEmptyOnEnter
{


    public static class Hooks
    {


        public static void Load()
        {
            On.Celeste.OuiFileSelect.Enter += Mod_Enter;
        }

        public static void Unload()
        {
            On.Celeste.OuiFileSelect.Enter -= Mod_Enter;
        }


        private static IEnumerator Mod_Enter(On.Celeste.OuiFileSelect.orig_Enter orig, OuiFileSelect self, Oui from)
        {

            bool have_files_been_deleted = false;

            // The highest saveslot
            if (TyporiumUtilities_DEVModule.Settings.RemoveEmptySavesOnEnter)
            {
                int maxslot = Utilities.SavesFunctions.GetHighestSlot();

                // Pointers
                int empty_index = 0;
                int nonempty_index = 0;

                // While none of the pointers went above the max savefile
                while (empty_index <= maxslot)
                {

                    // If an empty savefile is found
                    if (!Utilities.SavesFunctions.FilesExist(empty_index))
                    {

                        // Move forward the nonempty pointer if below empty
                        if (nonempty_index < empty_index)
                        {
                            nonempty_index = empty_index;
                        }

                        // While the nonempty pointer doesn't go out of range
                        while (nonempty_index <= maxslot)
                        {

                            // If a non-empty file is found, swap the empty one with the non-empty one 
                            if (Utilities.SavesFunctions.FilesExist(nonempty_index))
                            {
                                have_files_been_deleted = true;
                                Utilities.SavesFunctions.SwapFile(nonempty_index, empty_index, null);
                                break;
                            }

                            // If not, go to next save
                            nonempty_index += 1;
                        }
                    }

                    // If not empty, go to next save
                    empty_index += 1;
                }
            }

            // If at least one file has been deleted, reload the UI slots
            if (have_files_been_deleted)
            {

                // Removes each UI slots
                for (int i = 0; i < self.Slots.Length; i++)
                {
                    if (self.Slots[i] == null)
                    {
                        continue;
                    }

                    self.Slots[i].RemoveSelf();
                    self.Slots[i] = null;
                }

                // Forces the OUI to reload
                OuiFileSelect.Loaded = false;
                self.SlotSelected = false;
                self.SlotIndex = 0;
                self.HasSlots = false;
                self.loadedSuccess = false;

            }
            
            // Do the original method's purpose
            IEnumerator origEnum = orig(self, from);
            while (origEnum.MoveNext()) yield return origEnum.Current;

            yield break;
        }

    }
}