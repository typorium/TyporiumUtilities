using System.Collections;
using Celeste.Mod.TyporiumUtilities_DEV.Utilities.UIElements;
using Microsoft.Xna.Framework;
using Monocle;
using Celeste.Mod.TyporiumUtilities_DEV.Utilities;


namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.SaveFileSwapper
{


    public class SaveFileSwapperScene : TextMenu
    {


        // Static fields
        public static SaveFileSwapperScene Instance { get; set; }
        public static OuiFileSelect OuiInstance { get; set; }

        private static string TemporaryDirectoryname { get; set; } = "TyporiumUtilities_TemporaryDir";

        private static float arrow_x_position = 50;

        // Images
        MTexture arrow;
        MTexture arrow_confirm;

        // Binding labels
        public UIBindingLabel UI_SwapperModeToggler;
        private float UI_SwapperModeToggler_OffsetY;

        public UIBindingLabel UI_ChooseSave;

        // Swapping State
        public enum SwappingState
        {
            NotSwapping = 0,
            ChoosingFirstSlot = 1,
            ChoosingSecondSlot = 2
        }

        public SwappingState swapping_state;


        // Chosen saves
        int index_slot1;
        int index_slot2;

        public SaveFileSwapperScene() : base()
        {

            // Global
            this.AutoScroll = false;

            // Makes it so only one instance can exist at a time
            Instance = this;

            // Setup position for scene
            this.Position = new Vector2(0, 0);

            // Images
            this.arrow = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSwapper/arrow"];
            this.arrow_confirm = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSwapper/arrow_confirm"];

            // Binding labels
            this.UI_SwapperModeToggler = new UIBindingLabel(new Vector2(20, Engine.Height), "Toggle file swapper mode", TyporiumUtilities_DEVModule.Settings.SaveFileSwapper_OpenOui_Bind.Button, 0.5f);
            this.UI_SwapperModeToggler_OffsetY = Engine.Height - 1012f;

            this.UI_ChooseSave = new UIBindingLabel(new Vector2(20, 970), "Select save to swap", Input.MenuConfirm, 0.5f);

            // Swapping state
            this.swapping_state = SwappingState.NotSwapping;

            // Chosen saves
            this.index_slot1 = 0;
            this.index_slot2 = 0;

        }


        // Calls coroutine for entering the Oui
        public void StartEnterCoroutine()
        {
            this.Add(new Coroutine(this.Enter()));
        }


        // Coroutine for entering the Oui
        private IEnumerator Enter()
        {

            // Reset everything
            this.UI_SwapperModeToggler.Y = Engine.Height;
            yield return null;

            // Ease in for 1 second
            float time = 0.5f;
            for (float p = 0; p < time; p += Engine.DeltaTime)
            {
                this.UI_SwapperModeToggler.Y = Engine.Height - this.UI_SwapperModeToggler_OffsetY * Ease.SineOut(1 / time * p);
                yield return null;
            }

            // Setup everything
            this.UI_SwapperModeToggler.Y = Engine.Height - this.UI_SwapperModeToggler_OffsetY;
            yield break;

        }



        // Calls coroutine for leaving the Oui
        public void StartLeaveCoroutine()
        {
            this.Add(new Coroutine(this.Leave()));
        }


        // Coroutine for leaving the Oui
        private IEnumerator Leave()
        {

            // Reset everything
            this.swapping_state = SwappingState.NotSwapping;
            this.index_slot1 = 0;
            this.index_slot2 = 0;

            this.UI_SwapperModeToggler.Y = Engine.Height - this.UI_SwapperModeToggler_OffsetY;

            yield return null;

            // Ease in for 1 second
            float time = 0.25f;
            for (float p = time; p > 0; p -= Engine.DeltaTime)
            {
                this.UI_SwapperModeToggler.Y = Engine.Height - this.UI_SwapperModeToggler_OffsetY * Ease.SineIn(1 / time * p);
                yield return null;
            }

            // Setup everything
            this.UI_SwapperModeToggler.Y = Engine.Height;
            yield break;

        }


        // Update
        public override void Update()
        {

            // base.Update()
            this.Components.Update();

            // Leaving swapping state
            if (this.swapping_state != SwappingState.NotSwapping && TyporiumUtilities_DEVModule.Settings.SaveFileSwapper_OpenOui_Bind.Pressed)
            {
                TyporiumUtilities_DEVModule.Settings.SaveFileSwapper_OpenOui_Bind.ConsumePress();

                // Changes state
                this.swapping_state = SwappingState.NotSwapping;

            }

            // Enter swapping state
            if (this.swapping_state == SwappingState.NotSwapping && TyporiumUtilities_DEVModule.Settings.SaveFileSwapper_OpenOui_Bind.Pressed && !OuiInstance.SlotSelected)
            {
                TyporiumUtilities_DEVModule.Settings.SaveFileSwapper_OpenOui_Bind.ConsumePress();

                // Changes state
                this.swapping_state = SwappingState.ChoosingFirstSlot;

                // Reset saves chosen
                this.index_slot1 = this.index_slot2 = 0;
            }


            // If Oui is off, don't update anything else
            if (OuiInstance == null)
            {
                return;
            }

            // Updates everything supposed to be updated in state "choosingfirstslot"
            if (this.swapping_state == SwappingState.ChoosingFirstSlot)
            {

                // Update slot of first chosen save
                this.index_slot1 = OuiInstance.SlotIndex;

                // If save selected, move on to the second save to choose
                if (Input.MenuConfirm.Pressed)
                {
                    Input.MenuConfirm.ConsumePress();

                    // Switch to choosing second slot
                    this.swapping_state = SwappingState.ChoosingSecondSlot;

                    // Play audio
                    Audio.Play("event:/ui/main/button_select");
                    
                }
            }

            // Update everything supposed to be updated in state "choosingsecondslot"
            if (this.swapping_state == SwappingState.ChoosingSecondSlot)
            {

                // Update slot of second chosen save
                this.index_slot2 = OuiInstance.SlotIndex;

                // If save selected
                if (Input.MenuConfirm.Pressed && this.index_slot1 != this.index_slot2)
                {
                    Input.MenuConfirm.ConsumePress();

                    // Switch to not swapping mode
                    this.swapping_state = SwappingState.ChoosingFirstSlot;

                    // Play audio
                    Audio.Play("event:/ui/main/button_select");

                    // Swap files
                    SavesFunctions.SwapFile(this.index_slot1, this.index_slot2, OuiInstance);
                }
            }

        }


        // Renders everything
        public override void Render()
        {

            // Original render
            base.Render();

            // If a save is selected, don't render anything
            if (OuiInstance.SlotSelected)
            {
                return;
            }

            // Renders binds to toggle swap mode
            this.UI_SwapperModeToggler.Render();

            // If Oui is off, don't render anything else
            if (OuiInstance == null)
            {
                return;
            }

            // If swap mode is on
            if (this.swapping_state != SwappingState.NotSwapping)
            {

                // Renders bind telling player to press confirm to choose save
                this.UI_ChooseSave.Render();

            }


            // If choosing first save
            if (this.swapping_state == SwappingState.ChoosingFirstSlot)
            {

                // Renders arrow 1
                Vector2 arrow1_position = new Vector2(arrow_x_position, OuiInstance.Slots[this.index_slot1].CenterY);
                this.arrow.DrawCentered(arrow1_position);

            }

            // If choosing second save
            if (this.swapping_state == SwappingState.ChoosingSecondSlot)
            {

                // Renders arrow 1
                Vector2 arrow1_position = new Vector2(arrow_x_position, OuiInstance.Slots[this.index_slot1].CenterY);
                this.arrow_confirm.DrawCentered(arrow1_position);

                // Renders arrow 2
                Vector2 arrow2_position = new Vector2(arrow_x_position, OuiInstance.Slots[this.index_slot2].CenterY);
                this.arrow.DrawCentered(arrow2_position);

            }
        }
        
    }
}