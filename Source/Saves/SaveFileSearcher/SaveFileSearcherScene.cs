using Microsoft.Xna.Framework;
using Celeste.Mod.TyporiumUtilities_DEV.Utilities.UIElements;
using Monocle;
using System.Collections.Generic;
using System;
using System.Collections;


namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.SaveFileSearcher
{


    public enum SelectionableElements
    {
        SearchBar = 1,
        SaveFiles = 2
    }


    public class SaveFileSearcherScene : TextMenu
    {

        // Global
        private float xoffset;
        public SelectionableElements selected;

        // Title Menu
        private UIHeader TitleMenu_Header;

        // Search Menu
        private string search;

        private UIRectangle SearchMenu_SearchBox;
        private UITextArea SearchMenu_SearchTextArea;

        // Saves Menu
        private MTexture SavesMenu_CardWhite;
        private MTexture SavesMenu_TicketWhite;

        private MTexture SavesMenu_CardGolden;
        private MTexture SavesMenu_TicketGolden;

        private MTexture SavesMenu_ClockImage;
        private MTexture SavesMenu_DeathsImage;

        public int SelectedSave;

        private float BumpAnimation_YOffset;
        private bool CanSwitch;

        public List<OuiFileSelectSlot> SavesMenu_Slots;

        public bool SaveChosen { get; set; }


        public SaveFileSearcherScene() : base()
        {

            // Global
            this.AutoScroll = false;
            this.selected = SelectionableElements.SearchBar;

            // Title Menu
            this.TitleMenu_Header = new UIHeader( new Vector2(Engine.Width / 2, 80), Dialog.Clean("Typorium_TyporiumUtilities_Saves_SaveFileSearcher_MenuTitle_Header"), 2);

            // Search Menu
            this.search = "";
            this.SearchMenu_SearchBox = new UIRectangle( new Vector2(150, Engine.Height / 2), new Vector2(Engine.Width / 2 - 200, 100), Color.Black, alpha: 0.4f);
            this.SearchMenu_SearchTextArea = new UITextArea(this.SearchMenu_SearchBox.Position + new Vector2(10, 0), this.search, 1.2f, Color.White, false, 1f, true);

            // Saves Menu
            this.SavesMenu_CardWhite = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSearcher/card"];
            this.SavesMenu_TicketWhite = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSearcher/ticket"];

            this.SavesMenu_CardGolden = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSearcher/card_golden"];
            this.SavesMenu_TicketGolden = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSearcher/ticket_golden"];

            this.SavesMenu_ClockImage = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSearcher/time"];
            this.SavesMenu_DeathsImage = GFX.Gui["Typorium/TyporiumUtilities/SaveFileSearcher/deaths"];

            this.SelectedSave = 0;

            this.BumpAnimation_YOffset = 0;
            this.CanSwitch = true;
            this.SaveChosen = false;

            this.SavesMenu_Slots = new();
            this.UpdateSlots();

        }


        public void Initialize()
        {

            // Search Menu
            TextInput.OnInput += this.OnTextInput;
            this.search = "";

            // Saves Menu
            this.SavesMenu_Slots.Clear();
            this.SelectedSave = 0;
            this.selected = SelectionableElements.SearchBar;
            this.SaveChosen = false;
            this.UpdateSlots();

        }


        public void Uninitialize()
        {

            // Search Menu
            TextInput.OnInput -= this.OnTextInput;
            
            // Saves Menu
            this.SavesMenu_Slots.Clear();
            this.SelectedSave = 0;
            this.SaveChosen = false;
            this.selected = SelectionableElements.SearchBar;

        }


        public void SetXOffset(float xoffset)
        {

            // Global
            this.xoffset = xoffset;

            // Menu Title
            this.TitleMenu_Header.SetXOffset(this.xoffset);

            // Search Menu
            this.SearchMenu_SearchBox.SetXOffset(this.xoffset);
            this.SearchMenu_SearchTextArea.SetXOffset(this.xoffset);

        }


        private void UpdateSlots()
        {

            // If saves haven't loaded yet, don't search anything
            if (!OuiFileSelect.Loaded)
            {
                return;
            }

            // If they loaded, search for saves with same name
            OuiFileSelect ouifileselect = this.Scene.Entities.FindFirst<OuiFileSelect>();
            if (ouifileselect == null)
            {
                return;
            }

            this.SavesMenu_Slots.Clear();

            OuiFileSelectSlot[] ouifileselect_slots = ouifileselect.Slots;
            for(int i = 0; i < ouifileselect_slots.Length; i++)
            {
                OuiFileSelectSlot current = ouifileselect_slots[i];

                // If current save isn't created (empty save)
                if (!current.Exists)
                {
                    continue;
                }

                // If current save doens't contains search, don't accept
                if (!current.Name.ToLower().Contains(this.search.ToLower()))
                {
                    continue;
                }

                this.SavesMenu_Slots.Add(current);
            }

            // Reset scroll
            this.selected = SelectionableElements.SearchBar;
            this.SelectedSave = 0;
            this.BumpAnimation_YOffset = 0f;
        }


        public void OnTextInput(char c)
        {

            // If searchbar is selected or menu isn't fully shown
            if (this.selected != SelectionableElements.SearchBar || this.xoffset != 0)
            {
                return;
            }

            // Stores search for comparaison later
            string old_search = this.search;

            // Deleting character
            if ( (c == '\b' || c == '\u007f') && this.search.Length > 0)
            {
                this.search = this.search.Substring(0, this.search.Length - 1);
                Audio.Play("event:/ui/main/rename_entry_backspace");
            }
            
            // If it's a control key
            else if (char.IsControl(c))
            {
                Audio.Play("event:/ui/main/button_invalid");
            }

            // Else, add it to search
            else if (this.search.Length < 12 && ActiveFont.FontSize.Characters.ContainsKey((int)c))
            {
                if (c == ' ')
                {
                    Audio.Play("event:/ui/main/rename_entry_space");
                } else {
                    Audio.Play("event:/ui/main/rename_entry_char");
                }
                this.search += c;
            }
            else {
                Audio.Play("event:/ui/main/button_invalid");
            }

            // Compare old and new search
            if (this.search != old_search)
            {
                this.UpdateSlots();
            }
            
        }


        // Switch save in the saves menu
        private IEnumerator SwitchSave(int direction)
        {

            // Switch save
            this.CanSwitch = false;
            this.SelectedSave += direction;
            yield return null;

            // Bump Animation
            OuiFileSelectSlot current = this.SavesMenu_Slots[this.SelectedSave];
            for (float p = 0; p < 0.05f; p += Engine.DeltaTime)
            {
                this.BumpAnimation_YOffset = direction * 6f * Ease.SineIn(20*p);
                yield return null;
            }
            for (float p = 0.05f; p < 0.1f; p += Engine.DeltaTime)
            {
                this.BumpAnimation_YOffset = direction * 6f * Ease.SineOut(20*p);
                yield return null;
            }
            this.BumpAnimation_YOffset = 0f;
            this.CanSwitch = true;
            yield break;

        }


        public override void Update()
        {
            // base.Update();
            this.Components.Update();

            // Go from the searchbar to the save selection menu
            if (Input.MenuRight.Pressed && this.selected == SelectionableElements.SearchBar && this.SavesMenu_Slots.Count != 0)
            {
                if (this.SavesMenu_Slots.Count != 0)
                {
                    this.selected = SelectionableElements.SaveFiles;
                    Input.MenuRight.ConsumePress();
                }
            }

            // Go from the saves menu to the searchbar
            else if (Input.MenuLeft.Pressed && this.selected == SelectionableElements.SaveFiles)
            {
                this.selected = SelectionableElements.SearchBar;
                Input.MenuLeft.ConsumePress();
            }

            // Select save
            else if (Input.MenuConfirm.Pressed && this.selected == SelectionableElements.SaveFiles)
            {
                this.SaveChosen = true;
                return;
            }

            // Switch between saves
            if (this.selected == SelectionableElements.SaveFiles && this.CanSwitch)
            {

                // Switch down
                if (Input.MenuDown.Pressed && this.SelectedSave < this.SavesMenu_Slots.Count - 1)
                {
                    this.Add(new Coroutine(this.SwitchSave(1), true));
                    Input.MenuDown.ConsumePress();
                    Audio.Play("event:/ui/main/savefile_rollover_down");
                }
                else if (Input.MenuUp.Pressed && this.SelectedSave > 0)
                {
                    this.Add(new Coroutine(this.SwitchSave(-1), true));
                    Input.MenuUp.ConsumePress();
                    Audio.Play("event:/ui/main/savefile_rollover_up");
                }
            }

            // Search Menu
            this.SearchMenu_SearchTextArea.Y = this.SearchMenu_SearchBox.Y + this.SearchMenu_SearchBox.Height - ActiveFont.HeightOf(this.search + "_") * this.SearchMenu_SearchTextArea.scale;
        }


        public override void Render()
        {
            base.Render();

            // Title Menu
            this.TitleMenu_Header.Render();

            // Search Menu
            this.SearchMenu_SearchBox.Render();

            this.SearchMenu_SearchTextArea.text = this.search + (this.selected == SelectionableElements.SearchBar ? "_" : "");
            this.SearchMenu_SearchTextArea.Render();

            // Saves Menu
            int MarginBetweenSaves = 50;
            int PadX = 50;
            int MarginBetweenNameAndMapTitle = 60;
            int MarginForSelectedSave = -60;
            
            int MarginBetweenCardAndTicket = 30;
            int FullMarginBetweenCardAndTicket = 350 - MarginBetweenCardAndTicket;

            Vector2 MarginBetweenTicketAndClock = new Vector2(-120, -30);
            int MarginBetweenClockAndTime = -10;
            int MarginBetweenClockAndSkull = 10;
            int MarginBetweenSkullAndDeaths = -10;

            float savetitle_scale = 0.7f;
            float savecurrentmap_scale = 0.5f;
            float savetime_scale = 0.5f;
            float savedeaths_scale = 0.5f;

            // Create the anchor for all the saves
            Vector2 anchor = new Vector2(
                Engine.Width / 4 * 3 - this.SavesMenu_CardWhite.Width / 2 + PadX + this.xoffset,
                this.SearchMenu_SearchBox.Y + this.SavesMenu_CardWhite.Height / 2 - this.SelectedSave * (this.SavesMenu_CardWhite.Height + MarginBetweenSaves)
            );

            // For each saves
            for (int i = Math.Max(0, (int)this.SelectedSave - 2); i < Math.Min(this.SelectedSave + 3, this.SavesMenu_Slots.Count); i++ )
            {

                // Calculate the current save's position
                OuiFileSelectSlot current = this.SavesMenu_Slots[i];

                // Relevant informations about the save
                MTexture card = current.Golden ? this.SavesMenu_CardGolden : this.SavesMenu_CardWhite;
                MTexture ticket = current.Golden ? this.SavesMenu_TicketGolden : this.SavesMenu_TicketWhite;
                string save_lastmap = Dialog.Clean(AreaData.Areas[current.FurthestArea].Name);

                // Calculates card's position
                Vector2 current_position = new Vector2(anchor.X, anchor.Y);

                        // Add offset based off which save it is
                current_position.Y += i * (this.SavesMenu_CardWhite.Height + MarginBetweenSaves);

                        // If the current save is the selected save
                if (i == this.SelectedSave)
                {
                    current_position.Y += this.BumpAnimation_YOffset;
                    if (this.selected == SelectionableElements.SaveFiles)
                    {
                        current_position.X += MarginForSelectedSave;
                    }
                }

                // Calculate ticket's position
                Vector2 current_ticket_position = current_position;
                current_ticket_position.X += MarginBetweenCardAndTicket;
                if (i == this.SelectedSave && this.selected == SelectionableElements.SaveFiles)
                {
                    current_ticket_position.X += FullMarginBetweenCardAndTicket;
                }

                // Draws the ticket
                ticket.DrawCentered(current_ticket_position);

                // Draws information if the ticket is currently selected
                if (i == this.SelectedSave && this.selected == SelectionableElements.SaveFiles)
                {

                    // Draws clock
                    Vector2 clock_position = current_ticket_position;
                    clock_position += MarginBetweenTicketAndClock;
                    this.SavesMenu_ClockImage.DrawCentered(clock_position);

                    // Draws savefile time
                    Vector2 time_position = clock_position;
                    time_position.X += this.SavesMenu_ClockImage.Width;
                    time_position.X += MarginBetweenClockAndTime;
                    time_position.Y -= ActiveFont.HeightOf(current.Time) / 2 * savetime_scale;
                    ActiveFont.Draw(current.Time, time_position, Vector2.Zero, Vector2.One * savetime_scale, new Color(0.3f, 0.3f, 0.3f));

                    // Draws skull
                    Vector2 skull_position = clock_position;
                    skull_position.Y += this.SavesMenu_ClockImage.Height;
                    skull_position.Y += MarginBetweenClockAndSkull;
                    this.SavesMenu_DeathsImage.DrawCentered(skull_position);

                    // Draws deaths
                    Vector2 deathcounter_position = skull_position;
                    deathcounter_position.X += this.SavesMenu_DeathsImage.Width;
                    deathcounter_position.X += MarginBetweenSkullAndDeaths;
                    deathcounter_position.Y -= ActiveFont.HeightOf(current.Deaths.amount.ToString()) / 2 * savedeaths_scale;
                    ActiveFont.DrawOutline("" + current.Deaths.amount.ToString(), deathcounter_position, Vector2.Zero, Vector2.One * savedeaths_scale, Color.White, 1f, Color.Black);

                }

                // Draws the card
                card.DrawCentered(current_position);
                
                // Draws the savefile's name and current map's title
                Vector2 savename_margin = ActiveFont.Measure(current.Name) / 2;
                savename_margin *= savetitle_scale;
                savename_margin.Y += MarginBetweenNameAndMapTitle / 2;
                ActiveFont.Draw(
                    current.Name,
                    current_position - savename_margin,
                    Vector2.Zero,
                    Vector2.One * savetitle_scale,
                    Color.Black
                );

                Vector2 savemap_margin = ActiveFont.Measure(save_lastmap) / 2;
                savemap_margin *= savecurrentmap_scale;
                savemap_margin.Y -= MarginBetweenNameAndMapTitle / 2;
                ActiveFont.Draw(
                    save_lastmap,
                    current_position - savemap_margin,
                    Vector2.Zero,
                    Vector2.One * savecurrentmap_scale,
                    new Color(0.2f, 0.2f, 0.2f)
                );
            }

        }
    }
}