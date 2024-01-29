using System;
using Microsoft.Xna.Framework;

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
#if DEBUG
            // debug builds use verbose logging
            Logger.SetLogLevel(nameof(TyporiumUtilitiesModule), LogLevel.Verbose);
#else
            // release builds use info logging to reduce spam in log files
            Logger.SetLogLevel(nameof(TyporiumUtilitiesModule), LogLevel.Info);
#endif
        }

        public override void Load() {
            // TODO: apply any hooks that should always be active
        }

        public override void Unload() {
            // TODO: unapply any hooks applied in Load()
        }
    }
}
