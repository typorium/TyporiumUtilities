using System;
using On.Celeste;

namespace Celeste.Mod.TyporiumUtilities_DEV;

public class TyporiumUtilities_DEVModule : EverestModule {
    public static TyporiumUtilities_DEVModule Instance { get; private set; }

    public override Type SettingsType => typeof(TyporiumUtilities_DEVModuleSettings);
    public static TyporiumUtilities_DEVModuleSettings Settings => (TyporiumUtilities_DEVModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(TyporiumUtilities_DEVModuleSession);
    public static TyporiumUtilities_DEVModuleSession Session => (TyporiumUtilities_DEVModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(TyporiumUtilities_DEVModuleSaveData);
    public static TyporiumUtilities_DEVModuleSaveData SaveData => (TyporiumUtilities_DEVModuleSaveData) Instance._SaveData;

    public TyporiumUtilities_DEVModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(TyporiumUtilities_DEVModule), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(TyporiumUtilities_DEVModule), LogLevel.Info);
#endif
    }

    public override void Load()
    {

        // SaveFile Searcher
        Saves.SaveFileSearcher.Hooks.Load();

        // SaveFile Stats
        Saves.SaveFileStats.Hooks.Load();

        // SaveFile Swapper
        Saves.SaveFileSwapper.Hooks.Load();

        // SaveFile Others
        Saves.RemoveEmptyOnLoad.Hooks.Load();
    }

    public override void Unload()
    {

        // SaveFile Searcher
        Saves.SaveFileSearcher.Hooks.Unload();

        // SaveFile Stats
        Saves.SaveFileStats.Hooks.Unload();

        // SaveFile Swapper
        Saves.SaveFileSwapper.Hooks.Unload();

        // SaveFile Others
        Saves.RemoveEmptyOnLoad.Hooks.Unload();

    }
}