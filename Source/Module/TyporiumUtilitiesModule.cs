using System;
using Celeste.Mod.TyporiumUtilities.States;

namespace Celeste.Mod.TyporiumUtilities {
    public class TyporiumUtilitiesModule : EverestModule {
        public static TyporiumUtilitiesModule Instance { get; private set; }

        public override Type SettingsType => typeof(TyporiumUtilitiesModuleSettings);
        public static TyporiumUtilitiesModuleSettings Settings => (TyporiumUtilitiesModuleSettings) Instance._Settings;

        public override Type SessionType => typeof(TyporiumUtilitiesModuleSession);
        public static TyporiumUtilitiesModuleSession Session => (TyporiumUtilitiesModuleSession) Instance._Session;

        public override Type SaveDataType => typeof(TyporiumUtilitiesModuleSaveData);
        public static TyporiumUtilitiesModuleSaveData SaveData => (TyporiumUtilitiesModuleSaveData) Instance._SaveData;

        public TyporiumUtilitiesModule() {
            Instance = this;
            Logger.SetLogLevel(nameof(TyporiumUtilitiesModule), LogLevel.Info);
        }


        public override void Load() {
            TyporiumUtilities.UI.SaveFile.TyporiumUtilities_OuiFileSearch.Load();
            TyporiumUtilities.UI.SaveFile.TyporiumUtilities_OuiSaveFileStatsViewer.Load();

            Everest.Events.Level.OnLoadLevel += StateManager.OnLoadLevel;
            Everest.Events.Level.OnExit += StateManager.OnExit;
        }


        public override void Unload() {
            TyporiumUtilities.UI.SaveFile.TyporiumUtilities_OuiFileSearch.Unload();
            TyporiumUtilities.UI.SaveFile.TyporiumUtilities_OuiSaveFileStatsViewer.Unload();

            Everest.Events.Level.OnLoadLevel -= StateManager.OnLoadLevel;
            Everest.Events.Level.OnExit -= StateManager.OnExit;
        }
    }
}
