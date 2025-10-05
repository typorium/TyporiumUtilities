using System;
using System.Collections;
using System.IO;
using FMOD.Studio;

namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.RemoveEmptyOnLoad
{


    public static class Hooks
    {


        public static void Load()
        {
            // On.Celeste.OuiFileSelect.Enter += Mod_Enter;
        }

        public static void Unload()
        {
            // On.Celeste.OuiFileSelect.Enter -= Mod_Enter;
        }


        private static IEnumerator Mod_Enter(On.Celeste.OuiFileSelect.orig_Enter orig, OuiFileSelect self, Oui from)
        {
            yield break;
        }

    }
}