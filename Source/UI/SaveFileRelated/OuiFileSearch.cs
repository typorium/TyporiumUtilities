// Importation
using System.Collections;
using Monocle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Data;
using System.Linq;


namespace Celeste.Mod.TyporiumUtilities.UI.SaveFile {

    public class TyporiumUtilities_OuiFileSearch : Oui
    {

        // Left : search
        // Right : save files found
        TextMenu menu;
        TextMenu saves_menu;

        // Variables needed to check search related infos
        bool searching;
        string search_text;

        // UI
        private static int margin_offset = Engine.Width / 4;
        private static int savefile_name_length_max = 12;

        // Slots found
        private OuiFileSelectSlot[] slots;

        // Added, Removed
        public override void Added(Scene scene)
        {
            base.Added(scene);
        }

        public override void Removed(Scene scene)
        {
            base.Removed(scene);
        }


        // Constructor
        public TyporiumUtilities_OuiFileSearch() : base()
        {

            // Left and right
            this.menu = new TextMenu();
            this.saves_menu = new TextMenu();

            // Position of UI
            this.Position = Vector2.Zero;

            // Search related
            this.searching = true;
            this.search_text = "";

            // Slots that match the search
            this.slots = this.GetVerifiedSlots(this.search_text);
        }


        // UI creation
        public void CreateMenu()
        {
            this.menu = new TextMenu();
            this.saves_menu = new TextMenu();
            
            this.menu.Add( new TextMenu.Header( Dialog.Clean( "TyporiumUtilities_OuiFileSearchBar_BigTitle" ) ) );
            this.menu.Add( new TextMenu.SubHeader( Dialog.Clean( "TyporiumUtilities_OuiFileSearchBar_Explanation" ) ) );

            this.saves_menu.Add( new TextMenu.Header( Dialog.Clean( "TyporiumUtilities_OuiFileSearchBar_BigTitle" )) );
            this.saves_menu.Add( new TextMenu.SubHeader( Dialog.Clean( "TyporiumUtilities_OuiFileSearchBar_Explanation" )) );
        }


        // Get all slots that were verified, matched the searched text
        public OuiFileSelectSlot[] GetVerifiedSlots(string search_name)
        {
            if (!OuiFileSelect.Loaded){
                return [];
            }

            OuiFileSelectSlot[] iterate_slots = (OuiFileSelectSlot[])Scene.Entities.FindFirst<OuiFileSelect>().Slots.Clone();
            OuiFileSelectSlot[] new_slots = new OuiFileSelectSlot[iterate_slots.Length];

            int current_non_empty = 0;
            for(int i = 0; i < iterate_slots.Length; i++)
            {
                if (iterate_slots[i] is OuiFileSelectSlot && iterate_slots[i].Exists)
                {
                    if (iterate_slots[i].Name.ToLower().Contains(search_name.ToLower()))
                    {
                        new_slots[current_non_empty] = iterate_slots[i];
                        current_non_empty += 1;
                    }
                }
            }

            new_slots = new_slots.Where(slot => slot != null).ToArray();

            for(int i = 0; i < new_slots.Length; i++)
            {
                new_slots[i].Position.Y = i * new_slots[i].Height + 10;
            }

            return new_slots;
        }


        // Event to type in the new character
        public void OnTextInput(char character)
        {

            if (this.searching)
            {
                if (character == '\r')
                {
                    return;
                }

                else if (character == '\b')
                {
                    if (this.search_text.Length > 0)
                    {
                        this.search_text = this.search_text.Substring(0, this.search_text.Length - 1);
                        this.slots = this.GetVerifiedSlots(this.search_text);
                    }
                }

                else if (char.IsControl(character))
                {
                    return;
                }

                else
                {
                    if (this.search_text.Length < savefile_name_length_max)
                    {
                        this.search_text += character.ToString();
                        Audio.Play("event:/ui/main/rename_entry_char");

                        this.slots = this.GetVerifiedSlots(this.search_text);
                    }
                }
            }
        }


        // Render the menus
        public override void Render()
        {
            base.Render();

            string search_text_render = this.search_text + "_";

            Draw.Rect(
                this.menu.Position.X - this.menu.Width,
                this.menu.Position.Y + this.menu.Height,
                this.menu.Width * 2,
                Calc.Min(50, ActiveFont.HeightOf( search_text_render) ),
                Color.Black * 0.4f
            );

            ActiveFont.DrawOutline(
                this.search_text + "_",
                new Vector2(
                    this.menu.Position.X - this.menu.Width + 10,
                    this.menu.Position.Y + this.menu.Height - 2
                ),
                Vector2.Zero,
                Vector2.One * 0.75f,
                Color.White,
                2f,
                Color.Black
            );
        }


        // Update menus
        public override void Update()
        {

            if (this.searching && Input.MenuRight.Pressed)
            {
                if ( !(this.slots.Length <= 0) )
                {
                    this.searching = false;
                    this.menu.Focused = false;
                    this.saves_menu.Focused = true;
                    this.saves_menu.Selection = 0;
                }
            }
            else if (!this.searching && Input.MenuLeft.Pressed)
            {
                this.searching = true;
                this.menu.Focused = true;
                this.saves_menu.Focused = false;
            }

            base.Update();

            this.menu.Position.X = (Engine.Width / 2) + this.Position.X - margin_offset;
            this.menu.Position.Y = this.Position.Y - 50;

            this.saves_menu.Position.X = (Engine.Width / 2) + this.Position.X + margin_offset;

            this.saves_menu.Clear();

            for(int i = 0; i < this.slots.Length; i++)
            {
                TextMenu.Button button = new TextMenu.Button(this.slots[i].Name);
                button.OnPressed = delegate()
                {
                    if (this.Selected && this.Focused)
                    {
                        int slot_index = this.saves_menu.Selection;
                        OuiFileSelect oui_next = base.Overworld.Goto<OuiFileSelect>();
                        oui_next.SlotIndex = this.slots[slot_index].FileSlot;
                        oui_next.SelectSlot(false);
                    }
                };
                this.saves_menu.Add( button );
            }

            if (this.searching)
            {
                this.saves_menu.Selection = -1;
            }

            if(!this.searching && Input.MenuCancel.Pressed)
            {
                if (this.Selected && this.Focused)
                {
                    this.Overworld.Goto<OuiFileSelect>();
                }
            }
        }


        // Enter, Leave
        public override IEnumerator Leave(Oui next)
        {
            this.Focused = false;
            this.Visible = false;

            this.searching = false;
            this.menu.Focused = false;
            this.saves_menu.Focused = false;

            base.Scene.Remove( this.menu );
            base.Scene.Remove( this.saves_menu );

            TextInput.OnInput -= this.OnTextInput;

            yield return null;
            yield break;
        }


        public override IEnumerator Enter(Oui from)
        {
            this.Focused = true;
            this.Visible = true;

            this.searching = true;
            this.search_text = "";

            this.searching = true;
            this.menu.Focused = true;
            this.saves_menu.Focused = false;

            this.CreateMenu();

            base.Scene.Add( this.menu );
            base.Scene.Add( this.saves_menu );

            TextInput.OnInput += this.OnTextInput;

            yield return null;
            yield break;
        }


        // Load, Unload methods
        public static void Load()
        {
            On.Celeste.OuiFileSelect.Update += ModOuiFileSelectUpdate;
        }

        public static void Unload()
        {
            On.Celeste.OuiFileSelect.Update -= ModOuiFileSelectUpdate;
        }


        // Hook needed to make access to Oui possible
        private static void ModOuiFileSelectUpdate(On.Celeste.OuiFileSelect.orig_Update orig, OuiFileSelect self)
        {
            orig(self);

            if (TyporiumUtilitiesModule.Settings.UI_SaveFileRelated_OuiFileSearch_Open)
            {
                if (self.Selected && self.Focused) {
                    Audio.Play("event:/ui/main/whoosh_large_in");
                    
                    self.Overworld.Goto<TyporiumUtilities_OuiFileSearch>();
                }
            }
        }


    }
}