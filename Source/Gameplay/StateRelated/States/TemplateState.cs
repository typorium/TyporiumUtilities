using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities.States {


    [Tracked(true)]
    public class State : Entity
    {

        // Display informations
        string state_name;
        Color text_color;


        public State() : base()
        {

            // Display informations
            this.state_name = "NoneState";
            this.text_color = Color.White;
        }


        // Changes name of state for display purposes
        public void SetStateName(string state_name)
        {
            this.state_name = state_name;
        }


        // Gets name of state for display purposes
        public string GetStateName()
        {
            return this.state_name;
        }

        
        // Changes state's name's color for display purposes
        public void SetTextColor(Color color)
        {
            this.text_color = color;
        }


        // Gets state's name's color for display purposes
        public Color GetTextColor()
        {
            return this.text_color;
        }


        // Event for when state is activated
        public virtual void Reset()
        {}


        // Event for when state is desactivated
        public virtual void Switched()
        {
        }


        // Update for unactive state
        public virtual void OffUpdate()
        {}
    }

}