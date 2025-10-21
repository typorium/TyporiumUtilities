using System;

namespace Celeste.Mod.TyporiumUtilities_DEV;

public class TyporiumUtilities_DEVModule : EverestModule {
    public static TyporiumUtilities_DEVModule Instance { get; private set; }

    public override Type SettingsType => typeof(TyporiumUtilities_DEVModuleSettings);
    public static TyporiumUtilities_DEVModuleSettings Settings => (TyporiumUtilities_DEVModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(TyporiumUtilities_DEVModuleSession);
    public static TyporiumUtilities_DEVModuleSession Session => (TyporiumUtilities_DEVModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(TyporiumUtilities_DEVModuleSaveData);
    public static TyporiumUtilities_DEVModuleSaveData SaveData => (TyporiumUtilities_DEVModuleSaveData)Instance._SaveData;

    public static bool CollabUtils2_Loaded { get; set; }

    public TyporiumUtilities_DEVModule()
    {
        Instance = this;

        #if DEBUG
            Logger.SetLogLevel(nameof(TyporiumUtilities_DEVModule), LogLevel.Verbose);
        #else
            Logger.SetLogLevel(nameof(TyporiumUtilities_DEVModule), LogLevel.Info);
        #endif
        
    }
    

    private void CheckOptionalDependencies()
    {

        // CollabUtils2
        EverestModuleMetadata CollabUtils2Dependency = new()
        {
            Name = "CollabUtils2",
            Version = new Version(1, 10, 14)
        };
        CollabUtils2_Loaded = Everest.Loader.DependencyLoaded(CollabUtils2Dependency);

    }

    public override void Load()
    {

        CheckOptionalDependencies();

        // CollabUtils2 Related
        ForCollabUtils2.OpenJournalAnywhere.Hooks.Load();

        // Saves Related
        Saves.Hooks.Load();

    }

    public override void Unload()
    {

        // CollabUtils2 Related
        ForCollabUtils2.OpenJournalAnywhere.Hooks.Unload();

        // Saves Related
        Saves.Hooks.Load();
    }
}