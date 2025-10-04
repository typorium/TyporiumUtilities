using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities_DEV.Utilities.UIElements
{


    public class UIHeader : Entity
    {


        // Dialog, Text
        public string text { get; set; }
        public float scale;
        public float alpha;

        // Positioning
        private float xoffset;


        public UIHeader(Vector2 position, string text, float scale, bool centered = true, float alpha = 1f) : base(position)
        {

            // Dialog, Text
            this.text = text;
            this.scale = scale;
            this.alpha = alpha;

            // If centered, center text
            if (centered) {this.Position -= ActiveFont.Measure(this.text) * this.scale / 2; }

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

            ActiveFont.DrawEdgeOutline(this.text, this.Position + new Vector2(this.xoffset, 0), Vector2.Zero, Vector2.One * this.scale, Color.Gray * this.alpha, 4f, Color.DarkSlateBlue * alpha, 2f, Color.Black * (this.alpha * this.alpha * this.alpha) );
        }
    }
}