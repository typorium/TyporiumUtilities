using System.Collections;
using System.Linq;


namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.SaveFileStats
{


    public class Hooks
    {


        public static void Load()
        {
            On.Celeste.OuiFileSelect.Enter += FileSelect_Enter;
            On.Celeste.OuiFileSelect.Leave += FileSelect_Leave;
        }


        public static void Unload()
        {
            On.Celeste.OuiFileSelect.Enter -= FileSelect_Enter;
            On.Celeste.OuiFileSelect.Leave -= FileSelect_Leave;
        }


        public static IEnumerator FileSelect_Enter(On.Celeste.OuiFileSelect.orig_Enter orig, OuiFileSelect self, Oui from)
        {

            // Original coroutine
            IEnumerator origEnum = orig(self, from);
            while (origEnum.MoveNext()) yield return origEnum.Current;

            // Creates the new UI scene
            if (SaveFileStatsScene.Instance == null)
            {
                SaveFileStatsScene.Instance = new SaveFileStatsScene();
                SaveFileStatsScene.OuiInstance = self;
            }

            // Check to see if all slots are added on the scene
            if (self.Scene.Entities.FindAll<OuiFileSelectSlot>().Count() == self.Slots.Count())
            {

                // Adds it to scene
                if (SaveFileStatsScene.Instance != null)
                {
                    SaveFileStatsScene.Instance.Depth = self.Scene.Entities.FindFirst<OuiFileSelectSlot>().Depth - 1;
                    self.Overworld.Add(SaveFileStatsScene.Instance);
                    SaveFileStatsScene.Instance.StartEnterCoroutine();
                }

            }

        }

    
        public static IEnumerator FileSelect_Leave(On.Celeste.OuiFileSelect.orig_Leave orig, OuiFileSelect self, Oui next)
        {

            // Routine for stats viewer to leave
            if (SaveFileStatsScene.Instance != null)
            {
                SaveFileStatsScene.Instance.StartLeaveCoroutine();
            }

            // Original coroutine
            yield return new SwapImmediately(orig(self, next));

            // Remove stats viewer
            if (SaveFileStatsScene.Instance != null)
            {
                self.Overworld.Remove(SaveFileStatsScene.Instance);
            }
            
        }
    }
}