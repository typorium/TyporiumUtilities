namespace Celeste.Mod.TyporiumUtilities {
    public class TyporiumUtilitiesModuleSettings : EverestModuleSettings {


        [SettingName("TyporiumUtilities_Settings_UI_SaveFileRelated_OuiFileSearch_Open")]
        [DefaultButtonBinding(0, Microsoft.Xna.Framework.Input.Keys.Escape)]
        public ButtonBinding UI_SaveFileRelated_OuiFileSearch_Open { get; set;}


        [SettingName("TyporiumUtilities_Settings_Gameplay_States_ChangeState")]
        [DefaultButtonBinding(0, 0)]
        public ButtonBinding Gameplay_States_ChangeState { get; set;}
            
    }
}
