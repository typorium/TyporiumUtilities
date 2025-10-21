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


            // Do the original method's purpose
            IEnumerator origEnum = orig(self, from);
            while (origEnum.MoveNext()) yield return origEnum.Current;

            yield break;
        }

    }
}