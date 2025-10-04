namespace Celeste.Mod.TyporiumUtilities_DEV;


public class TyporiumUtilities_DEVModuleSettings : EverestModuleSettings
{

    // Binding which lets you open the SaveFile Searcher's UI
    [SettingSubHeader("Typorium_TyporiumUtilities_Settings_Saves_SaveFileSearcher_SubMenu")]
    [SettingInGame(false)]
    [SettingName("Typorium_TyporiumUtilities_Settings_Saves_SaveFileSearcher_OuiOpen")]
    public ButtonBinding SaveFileSearcher_OpenOui_Bind { get; set; }


    // Binding which lets you open the SaveFile Swapper's UI
    [SettingSubHeader("Typorium_TyporiumUtilities_Settings_Saves_SaveFileSwapper_SubMenu")]
    [SettingInGame(false)]
    [SettingName("Typorium_TyporiumUtilities_Settings_Saves_SaveFileSwapper_OuiOpen")]
    public ButtonBinding SaveFileSwapper_OpenOui_Bind { get; set; }


    // Whether or not the game deletes empty saves when loading saves
    [SettingSubHeader("Typorium_TyporiumUtilities_Settings_Saves_Others_SubMenu")]
    [SettingInGame(false)]
    [SettingName("Typorium_TyporiumUtilities_Settings_Saves_Others_DeleteEmptyOnStartup")]
    public bool DeleteEmptySavesOnStartup { get; set; } = false;

}