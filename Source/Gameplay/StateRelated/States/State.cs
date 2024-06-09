using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities.States {


    [Tracked(true)]
    public class State : Entity
    {
     
        string state_name;
        Color text_color;


        public State() : base()
        {
            this.state_name = "NoneState";
            this.text_color = Color.White;
        }


        public void SetStateName(string state_name)
        {
            this.state_name = state_name;
        }


        public string GetStateName()
        {
            return this.state_name;
        }


        public void SetTextColor(Color color)
        {
            this.text_color = color;
        }


        public Color GetTextColor()
        {
            return this.text_color;
        }


        public virtual void Reset()
        {}


        public virtual void Switched()
        {
        }
    }

}