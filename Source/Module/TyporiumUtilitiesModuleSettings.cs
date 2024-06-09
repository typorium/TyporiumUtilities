namespace Celeste.Mod.TyporiumUtilities {
    public class TyporiumUtilitiesModuleSettings : EverestModuleSettings {


        // SaveFile OUI Related
        [SettingName("TyporiumUtilities_Settings_UI_SaveFileRelated_OuiFileSearch_Open")]
        [DefaultButtonBinding(0, Microsoft.Xna.Framework.Input.Keys.Escape)]
        public ButtonBinding UI_SaveFileRelated_OuiFileSearch_Open { get; set;}


        // States Related
        [SettingName("TyporiumUtilities_Settings_Gameplay_States_ChangeState")]
        [DefaultButtonBinding(0, 0)]
        public ButtonBinding Gameplay_States_ChangeState { get; set;}

        // States (Viewer) Related
        [SettingInGame(false)]
        [SettingName("TyporiumUtilities_Settings_Gameplay_States_Viewer_HitboxTrailInSecond")]
        [SettingRange(1, 10)]
        public int Gameplay_States_Viewer_HitboxTrailInSecond { get; set; }
            
    }
}
