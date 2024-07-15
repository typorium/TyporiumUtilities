namespace Celeste.Mod.TyporiumUtilities {
    public class TyporiumUtilitiesModuleSettings : EverestModuleSettings {


        /*
        SaveFile OUI Related
        */
        [SettingSubHeader("TyporiumUtilities_Settings_UI_SaveFileRelated_OuiFileSearch_SubHeader")]

        // Open FileSearch
        [SettingName("TyporiumUtilities_Settings_UI_SaveFileRelated_OuiFileSearch_Open")]
        [DefaultButtonBinding(0, Microsoft.Xna.Framework.Input.Keys.Escape)]
        public ButtonBinding UI_SaveFileRelated_OuiFileSearch_Open { get; set;}




        /*
        Gameplay States Related
        */
        [SettingSubHeader("TyporiumUtilities_Settings_Gameplay_States_SubHeader")]

        // Change State Binding
        [SettingName("TyporiumUtilities_Settings_Gameplay_States_ChangeState")]
        [DefaultButtonBinding(0, 0)]
        public ButtonBinding Gameplay_States_ChangeState { get; set;}

        // States (Viewer) Related
        [SettingInGame(false)]
        [SettingName("TyporiumUtilities_Settings_Gameplay_States_Viewer_HitboxTrailInSecond")]
        [SettingRange(0, 10)]
        public int Gameplay_States_Viewer_HitboxTrailInSecond { get; set; }
            
    }
}
