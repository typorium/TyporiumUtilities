using System.Collections;

namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.SaveFileSwapper
{


    public class Hooks
    {


        public static void Load()
        {

            // Loading Hooks
            On.Celeste.OuiFileSelect.Enter += FileSelect_Enter;
            On.Celeste.OuiFileSelect.Leave += FileSelect_Leave;
            On.Celeste.OuiFileSelect.SelectSlot += FileSelect_SelectSlot;

        }


        public static void Unload()
        {

            // Loading Hooks
            On.Celeste.OuiFileSelect.Enter -= FileSelect_Enter;
            On.Celeste.OuiFileSelect.Leave -= FileSelect_Leave;
            On.Celeste.OuiFileSelect.SelectSlot -= FileSelect_SelectSlot;

        }


        public static IEnumerator FileSelect_Enter(On.Celeste.OuiFileSelect.orig_Enter orig, OuiFileSelect self, Oui from)
        {

            // Original coroutine
            IEnumerator origEnum = orig(self, from);
            while (origEnum.MoveNext()) yield return origEnum.Current;

            // Creates the new UI scene
            if (SaveFileSwapperScene.Instance == null)
            {
                SaveFileSwapperScene.Instance = new SaveFileSwapperScene();
                SaveFileSwapperScene.OuiInstance = self;
            }


            // Add the new UI scene to overworld
            self.Overworld.Add(SaveFileSwapperScene.Instance);
            SaveFileSwapperScene.Instance.StartEnterCoroutine();

        }


        public static IEnumerator FileSelect_Leave(On.Celeste.OuiFileSelect.orig_Leave orig, OuiFileSelect self, Oui next)
        {

            // Routine for stats viewer to leave
            if (SaveFileSwapperScene.Instance != null)
            {
                SaveFileSwapperScene.Instance.StartLeaveCoroutine();
            }

            // Original coroutine
            yield return new SwapImmediately(orig(self, next));

            // Remove stats viewer
            if (SaveFileSwapperScene.Instance != null)
            {
                self.Overworld.Remove(SaveFileSwapperScene.Instance);
            }

        }


        public static void FileSelect_SelectSlot(On.Celeste.OuiFileSelect.orig_SelectSlot orig, OuiFileSelect self, bool reset)
        {

            // Check if scene is created
            if (SaveFileSwapperScene.Instance == null)
            {
                return;
            }

            // Check if scene is in swap mode
            if (SaveFileSwapperScene.Instance.swapping_state != SaveFileSwapperScene.SwappingState.NotSwapping)
            {
                return;
            }

            // If not, then behave normally
            orig(self, reset);
        }

    }
}