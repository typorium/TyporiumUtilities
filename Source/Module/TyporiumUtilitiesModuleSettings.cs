namespace Celeste.Mod.TyporiumUtilities {
    public class TyporiumUtilitiesModuleSettings : EverestModuleSettings {


        [SettingName("TyporiumUtilities_Settings_UI_SaveFileRelated_OuiFileSearch_Open")]
        [DefaultButtonBinding(Microsoft.Xna.Framework.Input.Buttons.Start, Microsoft.Xna.Framework.Input.Keys.Escape)]
        public ButtonBinding UI_SaveFileRelated_OuiFileSearch_Open { get; set;}
            
    }
}
