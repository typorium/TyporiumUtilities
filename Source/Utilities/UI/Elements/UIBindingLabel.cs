using System;
using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities_DEV.Utilities.UIElements
{


    public class UIBindingLabel : Entity
    {


        // Name of label and bind's informations
        string label;
        VirtualButton bind;

        // Visuals
        float scale;
        float margin;

        // Constructor
        public UIBindingLabel(Vector2 position, string display_text, VirtualButton button, float scale, float margin = 5f) : base(position)
        {

            // Name of label and bind's informations
            this.label = display_text;
            this.bind = button;

            // Visuals
            this.scale = scale;
            this.margin = margin;

        }


        public void SetLabel(string label)
        {
            this.label = label;
        }


        public override void Render()
        {

            // Original render
            base.Render();

            // Renders button bindings on screen
            MTexture mTexture = Input.GuiButton(this.bind, Input.PrefixMode.Latest, "controls/keyboard/oemquestion");
            mTexture.Draw(this.Position, Vector2.Zero, Color.White, this.scale);

            // Renders label on screen
            float texty = this.Position.Y + (mTexture.Height - ActiveFont.Measure(this.label).Y) / 2 * this.scale;
            ActiveFont.DrawOutline(
                this.label,
                new Vector2(this.Position.X + (this.margin + mTexture.Width) * this.scale, texty),
                Vector2.Zero,
                Vector2.One * this.scale,
                Color.White,
                2f,
                Color.Black
            );

        }
    }
}