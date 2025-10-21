using System;

namespace Celeste.Mod.TyporiumUtilities_DEV.ForCollabUtils2.OpenJournalAnywhere
{


    public class Hooks
    {

        public static void Load()
        {
            On.Celeste.Player.Update += Mod_PlayerUpdate;
        }

        public static void Unload()
        {
            On.Celeste.Player.Update -= Mod_PlayerUpdate;
        }

        private static void Mod_PlayerUpdate(On.Celeste.Player.orig_Update orig, Player self)
        {

            // Original
            orig(self);

            // Check bind
            ButtonBinding bind = TyporiumUtilities_DEVModule.Settings.CollabUtils2_OpenJournalAnywhereInLobby;
            if (bind != null && bind.Pressed)
            {
                bind.ConsumeBuffer();
                Functions.OpenJournal(self.Scene, self);
            }
        }
    }
}