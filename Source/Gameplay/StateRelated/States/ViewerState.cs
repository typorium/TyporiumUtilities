using Microsoft.Xna.Framework;
using System.Linq;

namespace Celeste.Mod.TyporiumUtilities.States {


    public class ViewerState : State
    {

        // Runtime
        static bool is_active = false;

        // Level modifing 
        static Vector2 original_camera_position;
        static Vector2 current_camera_offset;


        // Loading
        public static void Load()
        {
            On.Celeste.Level.Update += ViewerState.LevelUpdateMod;
        }


        // Unloading
        public static void Unload()
        {
            On.Celeste.Level.Update -= ViewerState.LevelUpdateMod;
        }


        public ViewerState() : base()
        {
            this.SetStateName("Viewer");
            this.SetTextColor(Color.DarkGreen);

            original_camera_position = new Vector2(0, 0);
            current_camera_offset = new Vector2(0, 0);

            this.PostUpdate += delegate {
                Monocle.Engine.Scene.Entities.ToList().ForEach(e => {
                    if ((Monocle.Engine.Scene as Level).InsideCamera(e.Position, Monocle.Calc.Max(e.Width, e.Height)))
                    {
                        e.Visible = true;
                    }
                });
            };
        }


        public override void Update()
        {
            base.Update();
        }


        public override void Render()
        {
            base.Render();
        }


        public override void Reset()
        {
            base.Reset();

            is_active = true;
            original_camera_position = (Monocle.Engine.Scene as Level).Camera.Position;
            current_camera_offset = Vector2.Zero;
        }


        public override void Switched()
        {
            base.Switched();

            is_active = false;
            (Monocle.Engine.Scene as Level).Camera.Position = original_camera_position;
        }


        public static void LevelUpdateMod(On.Celeste.Level.orig_Update orig, Level self)
        {

            // Update Level when not active
            if (!is_active)
            {
                orig(self);
                return;
            }

            if (StateManager.shared_instance != null)
            {
                StateManager.shared_instance.Update();
            }

            // Move camera
            float speed_modifier = Input.MenuConfirm.Check ? 8 : 3;
            current_camera_offset += new Vector2(speed_modifier * Input.MoveX.Value, speed_modifier * Input.MoveY.Value);

            self.Camera.Position = original_camera_position + current_camera_offset;
        }
    }
}