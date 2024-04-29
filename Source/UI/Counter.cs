using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.TyporiumUtilities.UI
{


    // Counter that lets you render text and image
    public class Counter : Entity
    {


        // Amount
        int amount;
        Vector2 amount_position;

        
        // Image
        MTexture image;
        Vector2 image_position;
        int image_width;
        int image_height;


        // Other
        float text_amount_margin;
        float scale;


        // Constructor
        public Counter(int amount, MTexture texture, Vector2 position, float scale = 1f, int text_amount_margin = 10) : base()
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

            // Amount
            this.amount = amount;
            this.amount_position = new Vector2(
                this.image_position.X + this.image_width + this.text_amount_margin,
                this.image_position.Y + this.image_height / 2 - ActiveFont.Measure("× " + this.amount.ToString()).Y * this.scale / 2
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
            ActiveFont.Draw("× " + this.amount.ToString(), this.amount_position, Vector2.Zero, Vector2.One * this.scale, Color.White);
            ActiveFont.DrawOutline("× " + this.amount.ToString(), this.amount_position, Vector2.Zero, Vector2.One * this.scale, Color.White, 2f, Color.Black);
        }


        // Modifiers
        public void ChangeAmount(int amount)
        {
            this.amount = amount;
        }

        public void ChangeAmountBy(int amount)
        {
            this.amount += amount;
        }


        // Properties
        public int GetHeight()
        {
            float height = (
                Math.Max(
                    this.image_height,
                    ActiveFont.Measure("× " + this.amount.ToString()).Y
                ) + 1
            );

            return (int)height;
            
        }
    }
}