using System.Linq;
using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities_DEV.Utilities.UIElements
{


    public class UIRectangle : Entity
    {


        // Dialog, Text
        public float alpha;
        public Color color;

        // Positioning
        private float xoffset;


        public UIRectangle(Vector2 position, Vector2 size, Color color, float alpha = 1f) : base(position)
        {

            // Rectangle
            this.Collider = new Hitbox(size.X, size.Y);
            this.color = color;
            this.alpha = alpha;
            

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

            this.Position.X += this.xoffset;
            Draw.Rect(this.Collider, this.color * this.alpha);
            this.Position.X -= this.xoffset;
        }
    }
}