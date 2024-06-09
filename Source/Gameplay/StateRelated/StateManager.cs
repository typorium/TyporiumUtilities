using System.Linq;
using Microsoft.Xna.Framework;
using Monocle;


namespace Celeste.Mod.TyporiumUtilities.States {

    
    public class StateManager : Entity
    {

        // Shared instance
        public static StateManager shared_instance = null;

        // All possible states
        static State[] states = [
            new NormalState(),
            new ViewerState()
        ];

        // Current state
        int current_state;

        // State text display
        const float TEXT_DISPLAY_TIME = 2f;
        bool draw_state_text = false;
        float time_until = 0f;


        public static void Load()
        {

            // Loading general
            Everest.Events.Level.OnLoadLevel += StateManager.OnLoadLevel;
            Everest.Events.Level.OnExit += StateManager.OnExit;

            // Viewer State
            ViewerState.Load();
        }


        public static void Unload()
        {

            // Unloading general
            Everest.Events.Level.OnLoadLevel -= StateManager.OnLoadLevel;
            Everest.Events.Level.OnExit -= StateManager.OnExit;

            // View State
            ViewerState.Unload();
        }
        

        // Constructor
        public StateManager() : base()
        {

            // Tags
            this.AddTag(Tags.HUD);
            this.AddTag(Tags.TransitionUpdate);
            this.AddTag(Tags.Persistent);
            
            // Current State
            this.current_state = 0;
        }


        // Resets all variables
        public void Reset()
        {
            this.current_state = 0;
            this.GetCurrentState().Reset();

            this.draw_state_text = false;
            this.time_until = TEXT_DISPLAY_TIME;
        }


        // Changes state
        public void SwitchNextState()
        {
            this.GetCurrentState().Switched();
            (Scene as Level).Remove(this.GetCurrentState());

            this.current_state++;
            if (this.current_state == states.Length){
                this.current_state = 0;
            }

            this.GetCurrentState().Reset();
            (Scene as Level).Add(this.GetCurrentState());

            this.draw_state_text = true;
            this.time_until = TEXT_DISPLAY_TIME;
        }


        // Get current state
        public State GetCurrentState()
        {
            return states[this.current_state];
        }


        // Created when loading and going out of a level
        public static void OnLoadLevel(Level level, Player.IntroTypes playerIntro, bool isFromLoader)
        {

            if (shared_instance == null)
            {
                shared_instance = new StateManager();
            }

            shared_instance.Reset();

            level.Add(shared_instance);
            level.Add(shared_instance.GetCurrentState());
        }

        public static void OnExit(Level level, LevelExit exit, LevelExit.Mode mode, Session session, HiresSnow snow)
        {
            level.Remove(shared_instance);
        }


        // Updater
        public override void Update()
        {
            base.Update();

            // OffUpdates for each state which aren't active
            int states_count = states.Count();
            for (int i = 0; i < states_count; i++)
            {
                if (this.current_state == i)
                {
                    continue;
                }
                states[i].OffUpdate();
            }

            // If change state bind is pressed, switch
            if (TyporiumUtilitiesModule.Settings.Gameplay_States_ChangeState.Pressed)
            {
                this.SwitchNextState();
            }

            // Check if the state's name should be displayed
            if (this.draw_state_text)
            {
                this.time_until -= Engine.DeltaTime;
                if (this.time_until < 0)
                {
                    this.time_until = 0;
                    this.draw_state_text = false;
                }
            }
        }


        // Renderer
        public override void Render()
        {
            base.Render();


            // If not drawing
            if (!this.draw_state_text) {return;}

            // Get the state's name
            string text = this.GetCurrentState().GetStateName();

            // Calculates the display position
            int margin_x = 10;
            int margin_y = (int)(Engine.Height - ActiveFont.HeightOf(text) - margin_x);

            // Draws
            ActiveFont.DrawOutline(
                text,
                new Vector2(margin_x, margin_y),
                Vector2.Zero,
                Vector2.One,
                this.GetCurrentState().GetTextColor(),
                2 ,
                Color.Black
            );
        }
    }
}