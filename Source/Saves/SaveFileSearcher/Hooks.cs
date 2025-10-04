namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.SaveFileSearcher
{


    public class Hooks
    {


        public static void Load()
        {

            // On Hook on File selection UI to make the savefile searcher UI accessible
            On.Celeste.OuiFileSelect.Update += FileSelect_Update;

        }


        public static void Unload()
        {

            // On Hook on File selection UI to make the savefile searcher UI accessible
            On.Celeste.OuiFileSelect.Update -= FileSelect_Update;
            
        }


        public static void FileSelect_Update(On.Celeste.OuiFileSelect.orig_Update orig, OuiFileSelect self)
        {

            // If appropriate menu button is pressed, go to menu
            ButtonBinding bind = TyporiumUtilities_DEVModule.Settings.SaveFileSearcher_OpenOui_Bind;
            if (bind.Pressed && self.Selected)
            {
                bind.ConsumePress();
                self.Overworld.Goto<SaveFileSearcherOui>();
            }

            // Original method
            orig(self);
        }

    }
}