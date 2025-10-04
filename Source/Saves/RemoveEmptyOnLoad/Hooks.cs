using System;

namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.RemoveEmptyOnLoad
{


    public static class Hooks
    {


        public static void Load()
        {
            On.Celeste.OuiFileSelect.LoadThread += Mod_LoadThread;
        }

        public static void Unload()
        {
            On.Celeste.OuiFileSelect.LoadThread -= Mod_LoadThread;
        }


        public static void Mod_LoadThread(On.Celeste.OuiFileSelect.orig_LoadThread orig, OuiFileSelect self)
        {

            // Original actions
            orig(self);

            // For all savefile
            int i = 0;
            while (i < self.Slots.Length)
            {

                // If savefile is empty
                if (!self.Slots[i].Exists)
                {}

            }

        }
        
    }
}