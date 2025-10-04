using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities_DEV.Utilities.UIElements
{


    public class UITextArea : Entity
    {


        // Dialog, Text
        public string text { get; set; }
        public float scale;
        public float alpha;
        public bool outline;

        // Positioning
        private float xoffset;
        private bool centered;


        public UITextArea(Vector2 position, string text, float scale, Color color, bool centered = true, float alpha = 1f, bool outline = false) : base(position)
        {

            // Dialog, Text
            this.text = text;
            this.scale = scale;
            this.alpha = alpha;
            this.outline = outline;

            // Positioning
            this.centered = centered;

        }


        public void SetXOffset(float xoffset)
        {
            this.xoffset = xoffset;
        }


        public override void Update()
        {
            base.Update();
        }


        public override void Render()
        {
            base.Render();

            // If centered, center text temporarily
            if (this.centered) {this.Position -= ActiveFont.Measure(this.text) * this.scale / 2; }

            // Draw text
            if (this.outline)
            {
                ActiveFont.DrawEdgeOutline(this.text, this.Position + new Vector2(this.xoffset, 0), Vector2.Zero, Vector2.One * this.scale, Color.White, 1f, Color.Black, 2f, Color.Black);
            } else {
                ActiveFont.Draw(this.text, this.Position + new Vector2(this.xoffset, 0), Color.White);
            }

            // If centered, return to normal position
            if (centered) {this.Position += ActiveFont.Measure(this.text) * this.scale / 2; }
        }
    }
}