using Microsoft.Xna.Framework;

namespace Celeste.Mod.TyporiumUtilities.States {


    public class NormalState : State
    {


        public NormalState() : base()
        {
            this.SetStateName("Normal");
            this.SetTextColor(Color.White);
        }


        public override void Reset()
        {
            base.Reset();

            Logger.Log(LogLevel.Info, "typoporin", "changed to" + this.GetStateName());
        }


    }
}