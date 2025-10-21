namespace Celeste.Mod.TyporiumUtilities_DEV.ForCollabUtils2
{


    public class Hooks
    {

        public static void Load()
        {

            if (!TyporiumUtilities_DEVModule.CollabUtils2_Loaded)
            {
                return;
            }

            // Open Journal Anywhere
            OpenJournalAnywhere.Hooks.Load();

        }
        

        public static void Unload()
        {

            if (!TyporiumUtilities_DEVModule.CollabUtils2_Loaded)
            {
                return;
            }

            // Open Journal Anywhere
            OpenJournalAnywhere.Hooks.Unload();
        
        }
    }
}