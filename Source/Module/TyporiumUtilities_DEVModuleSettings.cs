namespace Celeste.Mod.TyporiumUtilities_DEV;


public class TyporiumUtilities_DEVModuleSettings : EverestModuleSettings
{

    /*
    ##########################################
    # COLLAB UTILS 2 RELATED
    ##########################################
    */

    // Whether or not the game deletes empty saves when loading saves
    [SettingSubHeader("Typorium_TyporiumUtilities_Settings_CollabUtils2_OpenJournalAnywhere_SubMenu")]
    [SettingName("Typorium_TyporiumUtilities_Settings_CollabUtils2_OpenJournalAnywhere_Bind")]
    public ButtonBinding CollabUtils2_OpenJournalAnywhereInLobby { get; set; }

    /*
    ##########################################
    # SAVES RELATED
    ##########################################
    */

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
    [SettingName("Typorium_TyporiumUtilities_Settings_Saves_Others_RemoveEmptySavesOnEnter")]
    public bool RemoveEmptySavesOnEnter { get; set; } = false;

}