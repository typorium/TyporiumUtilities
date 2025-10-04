using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Celeste.Mod.TyporiumUtilities_DEV.Utilities.UIElements;
using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities_DEV.Saves.SaveFileStats
{


    public class SaveFileStatsScene : TextMenu
    {


        // Instance
        public static SaveFileStatsScene Instance { get; set; }
        public static OuiFileSelect OuiInstance { get; set;}

        // Global
        private float YOffset;
        private float margin;

        // Border
        Vector2 BorderRectangle_DefaultPosition;
        UIRectangle BorderRectangle;

        // Time Counter
        private MTexture TimeCounter_Icon;
        private Vector2 TimeCounter_Position;
        private UITextArea TimeCounter_Text;

        // Deaths Counter
        private MTexture DeathCounter_Icon;
        private Vector2 DeathCounter_Position;
        private UITextArea DeathCounter_Text;

        // Strawberry Counter
        private MTexture StrawberryCounter_Icon;
        private Vector2 StrawberryCounter_Position;
        private UITextArea StrawberryCounter_Text;

        // Gold Strawberry Counter
        private MTexture GoldStrawberryCounter_Icon;
        private Vector2 GoldStrawberryCounter_Position;
        private UITextArea GoldStrawberryCounter_Text;

        // Clear Counter
        private MTexture ClearCounter_Icon;
        private Vector2 ClearCounter_Position;
        private UITextArea ClearCounter_Text;


        // Constructor
        public SaveFileStatsScene() : base()
        {

            // Global
            this.Position = Vector2.Zero;
            this.YOffset = 0;
            this.margin = 200;

            // Border
            this.BorderRectangle_DefaultPosition = Vector2.Zero;
            this.BorderRectangle = new UIRectangle(new Vector2(0, 0), new Vector2(Engine.Width, 100), Color.Black, 0.8f);

            // Time Counter
            this.TimeCounter_Icon = GFX.Gui["Typorium/TyporiumUtilities/SaveFileStats/time"];

            float margin = this.BorderRectangle.Height / 2;
            this.TimeCounter_Position = new Vector2(margin, margin);

            this.TimeCounter_Text = new UITextArea(
                Vector2.Zero,
                "",
                0.7f,
                Color.White,
                false,
                1,
                true
            );

            // Death Counter
            this.DeathCounter_Icon = GFX.Gui["Typorium/TyporiumUtilities/SaveFileStats/deaths"];
            this.DeathCounter_Position = Vector2.Zero;
            this.DeathCounter_Text = new UITextArea(
                Vector2.Zero,
                "",
                0.7f,
                Color.White,
                false,
                1,
                true
            );

            // Strawberry Counter
            this.StrawberryCounter_Icon = GFX.Gui["Typorium/TyporiumUtilities/SaveFileStats/strawberry"];
            this.StrawberryCounter_Position = Vector2.Zero;
            this.StrawberryCounter_Text = new UITextArea(
                Vector2.Zero,
                "",
                0.7f,
                Color.White,
                false,
                1,
                true
            );

            // Gold Strawberry Counter
            this.GoldStrawberryCounter_Icon = GFX.Gui["Typorium/TyporiumUtilities/SaveFileStats/gold_strawberry"];
            this.GoldStrawberryCounter_Position = Vector2.Zero;
            this.GoldStrawberryCounter_Text = new UITextArea(
                Vector2.Zero,
                "",
                0.7f,
                Color.White,
                false,
                1,
                true
            );

            // Clear Counter
            this.ClearCounter_Icon = GFX.Gui["Typorium/TyporiumUtilities/SaveFileStats/clear"];
            this.ClearCounter_Position = Vector2.Zero;
            this.ClearCounter_Text = new UITextArea(
                Vector2.Zero,
                "",
                0.7f,
                Color.White,
                false,
                1,
                true
            );

        }


        // Recalculates all necessary values
        private void Initialize()
        {

            // Gets all saves
            if (!OuiFileSelect.Loaded)
            {
                return;
            }
            OuiFileSelectSlot[] slots = OuiInstance.Slots;

            // Needed informations
            long TimeCounter_ticks = 0;
            int DeathCounter_deaths = 0;
            int StrawberryCounter_strawberries = 0;
            int GoldStrawberryCounter_goldstrawberries = 0;
            int ClearCounter_clear = 0;
            
            // Loops over all files
            for (int i = 0; i < slots.Count(); i++)
            {

                // If save doesn't exist, skip
                if (!UserIO.Exists(SaveData.GetFilename(i)))
                {
                    continue;
                }

                // Get data
                SaveData data = UserIO.Load<SaveData>(SaveData.GetFilename(i), false);
                if (data == null)
                {
                    continue;
                }

                // Get save's time
                TimeCounter_ticks += data.Time;

                // Get deaths
                DeathCounter_deaths += data.TotalDeaths;

                // Get strawberries and clears
                List<LevelSetStats> levelsets = data.LevelSets;
                levelsets.AddRange(data.LevelSetRecycleBin);

                levelsets.ForEach(levelsetstats => {
                    levelsetstats.Areas.ForEach(areastats => {
                        for (int j = 0; j < areastats.Modes.Length; j++)
                        {
                            StrawberryCounter_strawberries += areastats.Modes[j].TotalStrawberries;
                            ClearCounter_clear += areastats.Modes[j].Completed ? 1 : 0;
                        }
                        
                    });
                });

                // Get gold strawberries
                GoldStrawberryCounter_goldstrawberries += data.TotalGoldenStrawberries;

            }

            // Time Counter
            this.TimeCounter_Text.text = Dialog.FileTime(TimeCounter_ticks);
            this.TimeCounter_Text.Position = new Vector2(
                this.TimeCounter_Position.X + this.TimeCounter_Icon.Width / 2 + 10,
                this.TimeCounter_Position.Y
            );
            this.TimeCounter_Text.Position.Y -= ActiveFont.HeightOf(this.TimeCounter_Text.text) * this.TimeCounter_Text.scale / 2;

            // Deaths Counter
            this.DeathCounter_Position = new Vector2(
                this.TimeCounter_Text.Position.X + ActiveFont.Measure(this.TimeCounter_Text.text).X * this.TimeCounter_Text.scale + this.DeathCounter_Icon.Width / 2 + this.margin,
                this.TimeCounter_Position.Y
            );
            this.DeathCounter_Text.text = DeathCounter_deaths.ToString();
            this.DeathCounter_Text.Position = new Vector2(
                this.DeathCounter_Position.X + this.DeathCounter_Icon.Width / 2 + 10,
                this.DeathCounter_Position.Y
            );
            this.DeathCounter_Text.Position.Y -= ActiveFont.HeightOf(this.DeathCounter_Text.text) * this.DeathCounter_Text.scale / 2;

            // Strawberry Counter
            this.StrawberryCounter_Position = new Vector2(
                this.DeathCounter_Text.Position.X + ActiveFont.Measure(this.DeathCounter_Text.text).X * this.DeathCounter_Text.scale + this.StrawberryCounter_Icon.Width / 2 + this.margin,
                this.DeathCounter_Position.Y
            );
            this.StrawberryCounter_Text.text = StrawberryCounter_strawberries.ToString();
            this.StrawberryCounter_Text.Position = new Vector2(
                this.StrawberryCounter_Position.X + this.StrawberryCounter_Icon.Width / 2 + 10,
                this.StrawberryCounter_Position.Y
            );
            this.StrawberryCounter_Text.Position.Y -= ActiveFont.HeightOf(this.StrawberryCounter_Text.text) * this.StrawberryCounter_Text.scale / 2;

            // Gold Strawberry Counter
            this.GoldStrawberryCounter_Position = new Vector2(
                this.StrawberryCounter_Text.Position.X + ActiveFont.Measure(this.StrawberryCounter_Text.text).X * this.StrawberryCounter_Text.scale + this.GoldStrawberryCounter_Icon.Width / 2 + this.margin,
                this.StrawberryCounter_Position.Y
            );
            this.GoldStrawberryCounter_Text.text = GoldStrawberryCounter_goldstrawberries.ToString();
            this.GoldStrawberryCounter_Text.Position = new Vector2(
                this.GoldStrawberryCounter_Position.X + this.GoldStrawberryCounter_Icon.Width / 2 + 10,
                this.GoldStrawberryCounter_Position.Y
            );
            this.GoldStrawberryCounter_Text.Position.Y -= ActiveFont.HeightOf(this.GoldStrawberryCounter_Text.text) * this.GoldStrawberryCounter_Text.scale / 2;

            // Clear Counter
            this.ClearCounter_Position = new Vector2(
                this.GoldStrawberryCounter_Text.Position.X + ActiveFont.Measure(this.GoldStrawberryCounter_Text.text).X * this.GoldStrawberryCounter_Text.scale + this.ClearCounter_Icon.Width / 2 + this.margin,
                this.GoldStrawberryCounter_Position.Y
            );
            this.ClearCounter_Text.text = ClearCounter_clear.ToString();
            this.ClearCounter_Text.Position = new Vector2(
                this.ClearCounter_Position.X + this.ClearCounter_Icon.Width / 2 + 10,
                this.ClearCounter_Position.Y
            );
            this.ClearCounter_Text.Position.Y -= ActiveFont.HeightOf(this.ClearCounter_Text.text) * this.ClearCounter_Text.scale / 2;

        }


        // Enter coroutine
        private IEnumerator EnterCoroutine()
        {

            // Reset everything
            this.YOffset = -this.BorderRectangle.Collider.Height;
            yield return null;

            // Ease in for 1 second
            float time = 0.5f;
            for (float p = time; p > 0; p -= Engine.DeltaTime)
            {
                this.YOffset = -this.BorderRectangle.Collider.Height * Ease.SineIn(1 / time * p);
                yield return null;
            }

            // Setup everything
            this.YOffset = 0;

        }

        public void StartEnterCoroutine()
        {

            // Recalculate all values
            this.Initialize();

            // Entering coroutine
            this.Add(new Coroutine(this.EnterCoroutine()));

        }


        // Leave coroutine
        private IEnumerator LeaveCoroutine()
        {

            // Reset everything
            this.YOffset = 0;
            yield return null;

            // Ease in for 1 second
            float time = 0.15f;
            for (float p = 0; p < time; p += Engine.DeltaTime)
            {
                this.YOffset = -this.BorderRectangle.Collider.Height * Ease.SineIn(1 / time * p);
                yield return null;
            }

            // Setup everything
            this.YOffset = -this.BorderRectangle.Collider.Height;

        }
        
        public void StartLeaveCoroutine()
        {
            this.Add(new Coroutine(this.LeaveCoroutine()));
        }


        // Updating
        public override void Update()
        {
            
            // base.Update();
            this.Components.Update();

        }


        // Rendering
        public override void Render()
        {

            // Original rendering
            base.Render();

            // Stats Border
            this.BorderRectangle.Position = this.BorderRectangle_DefaultPosition;
            this.BorderRectangle.Y += this.YOffset;
            this.BorderRectangle.Render();

            // Render time counter
            Vector2 New_TimeCounter_Position = this.TimeCounter_Position;
            New_TimeCounter_Position.Y += this.YOffset;
            this.TimeCounter_Icon.DrawCentered(New_TimeCounter_Position);

            this.TimeCounter_Text.Y += this.YOffset;
            this.TimeCounter_Text.Render();
            this.TimeCounter_Text.Y -= this.YOffset;

            // Render death counter
            Vector2 New_DeathCounter_Position = this.DeathCounter_Position;
            New_DeathCounter_Position.Y += this.YOffset;
            this.DeathCounter_Icon.DrawCentered(New_DeathCounter_Position);

            this.DeathCounter_Text.Y += this.YOffset;
            this.DeathCounter_Text.Render();
            this.DeathCounter_Text.Y -= this.YOffset;

            // Render strawberry counter
            Vector2 New_StrawberryCounter_Position = this.StrawberryCounter_Position;
            New_StrawberryCounter_Position.Y += this.YOffset;
            this.StrawberryCounter_Icon.DrawCentered(New_StrawberryCounter_Position);

            this.StrawberryCounter_Text.Y += this.YOffset;
            this.StrawberryCounter_Text.Render();
            this.StrawberryCounter_Text.Y -= this.YOffset;

            // Render gold strawberry counter
            Vector2 New_GoldStrawberryCounter_Position = this.GoldStrawberryCounter_Position;
            New_GoldStrawberryCounter_Position.Y += this.YOffset;
            this.GoldStrawberryCounter_Icon.DrawCentered(New_GoldStrawberryCounter_Position);

            this.GoldStrawberryCounter_Text.Y += this.YOffset;
            this.GoldStrawberryCounter_Text.Render();
            this.GoldStrawberryCounter_Text.Y -= this.YOffset;

            // Render clear counter
            Vector2 New_ClearCounter_Position = this.ClearCounter_Position;
            New_ClearCounter_Position.Y += this.YOffset;
            this.ClearCounter_Icon.DrawCentered(New_ClearCounter_Position);

            this.ClearCounter_Text.Y += this.YOffset;
            this.ClearCounter_Text.Render();
            this.ClearCounter_Text.Y -= this.YOffset;

        }

    }
}