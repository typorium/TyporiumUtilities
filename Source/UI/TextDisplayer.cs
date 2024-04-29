using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.TyporiumUtilities.UI
{


    // TextDisplayer that lets you render text and image
    public class TextDisplayer : Entity
    {


        // Text
        string text;
        Vector2 text_position;

        
        // Image
        MTexture image;
        Vector2 image_position;
        int image_width;
        int image_height;


        // Other
        float text_amount_margin;
        float scale;
        float text_scale_inverse;


        // Constructor
        public TextDisplayer(string text, MTexture texture, Vector2 position, float scale = 1f, int text_amount_margin = 10, bool scale_text = true) : base()
        {
            
            // HUD tag in order to render it in UIs
            this.AddTag(Tags.HUD);
            this.Depth = -1;

            // Other
            this.scale = scale;
            this.text_amount_margin = text_amount_margin;

            // Image related
            this.image = texture;
            this.image_position = position;
            this.Position = position;
            this.image_width = (int)(this.image.Width * this.scale);
            this.image_height = (int)(this.image.Height * this.scale);

            // text
            this.text = text;
            this.text_scale_inverse = scale_text ? 1f : this.scale;
            this.text_position = new Vector2(
                this.image_position.X + this.image_width + this.text_amount_margin,
                this.image_position.Y + this.image_height / 2 - ActiveFont.Measure("× " + this.text).Y * this.scale / this.text_scale_inverse / 2
            );
            
        }


        // Updates component
        public override void Update()
        {
            base.Update();
        }


        // Renders component
        public override void Render()
        {

            base.Render();

            this.image.Draw(this.image_position, Vector2.Zero, Color.White, this.scale);
            ActiveFont.Draw(this.text.ToString(), this.text_position, Vector2.Zero, Vector2.One * this.scale / this.text_scale_inverse, Color.White);
            ActiveFont.DrawOutline(this.text, this.text_position, Vector2.Zero, Vector2.One * this.scale / this.text_scale_inverse, Color.White, 2f, Color.Black);
        }


        // Modifiers
        public void ChangeText(string text)
        {
            this.text = text;
        }


        // Properties
        public int GetHeight()
        {
            float height = (
                Math.Max(
                    this.image_height,
                    ActiveFont.Measure(this.text).Y
                ) + 1
            );

            return (int)height;
            
        }
    }
}