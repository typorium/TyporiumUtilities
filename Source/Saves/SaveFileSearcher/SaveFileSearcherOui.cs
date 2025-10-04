using System.Collections;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.SaveFileSearcher
{


    public class SaveFileSearcherOui : Oui
    {


        // Scene
        public SaveFileSearcherScene oui_scene;


        public SaveFileSearcherOui() : base()
        {

            // Scene
            this.oui_scene = new SaveFileSearcherScene();
            this.oui_scene.SetXOffset(-1920);

        }


        public override IEnumerator Enter(Oui from)
        {

            // Slides into the screen on enter
            this.oui_scene.SetXOffset(-1920);
            this.Overworld.Add(this.oui_scene);
            yield return null;

            // Load necessary things
            Audio.Play("event:/ui/main/whoosh_savefile_in");
            this.oui_scene.Initialize();

            float time = 0.5f;
            for (float p = time; p > 0; p -= Engine.DeltaTime)
            {
                this.oui_scene.SetXOffset(-1920 / time * Ease.SineIn(2*p) );
                yield return null;
            }
            this.oui_scene.SetXOffset(0);

            // End
            yield return null;
        }


        public override IEnumerator Leave(Oui next)
        {

            // Slides outside the screen on leave
            this.oui_scene.SetXOffset(0);
            Audio.Play("event:/ui/main/whoosh_savefile_out");

            float time = 0.5f;
            for (float p = 0; p < time; p += Engine.DeltaTime)
            {
                this.oui_scene.SetXOffset(-1920 / time * Ease.SineIn(2*p) );
                yield return null;
            }
            this.oui_scene.SetXOffset(-1920);

            // Unload necessary things
            this.oui_scene.Uninitialize();

            // ENd
            this.Overworld.Remove(this.oui_scene);
            yield return null;
        }


        public override void Update()
        {
            base.Update();

            if (Input.MenuCancel.Pressed && this.Selected && this.oui_scene.selected != SelectionableElements.SearchBar || Input.ESC.Pressed && this.Selected)
            {
                Input.MenuCancel.ConsumePress();
                Input.ESC.ConsumePress();
                this.Overworld.Goto<OuiFileSelect>();
                return;
            }

            if (this.oui_scene.SaveChosen && this.Selected)
            {
                OuiFileSelect ouifileselect = this.Overworld.GetUI<OuiFileSelect>();
                ouifileselect.SlotIndex = this.oui_scene.SavesMenu_Slots[this.oui_scene.SelectedSave].FileSlot;
                this.Overworld.Goto<OuiFileSelect>();
                return;
            }
        }
    }
}