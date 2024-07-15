using Microsoft.Xna.Framework;
using Monocle;
using System.Collections.Generic;
using System.Linq;

namespace Celeste.Mod.TyporiumUtilities.States {


    public class ViewerState : State
    {

        // Runtime
        static bool is_active = false;

        // Level viewing
        static Vector2 original_camera_position;
        static Vector2 current_camera_offset;

        // Player Related
        static List<Collider> player_positions = new List<Collider>();
        static Color[] player_positions_color = {Color.YellowGreen, Color.Blue, Color.Red};
        static int PLAYER_POSITIONS_MAXCOUNT = 60 * TyporiumUtilitiesModule.Settings.Gameplay_States_Viewer_HitboxTrailInSecond;


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


        // Constructor
        public ViewerState() : base()
        {

            // Display informations
            this.SetStateName("Viewer");
            this.SetTextColor(Color.DarkGreen);

            // Level viewing
            original_camera_position = new Vector2(0, 0);
            current_camera_offset = new Vector2(0, 0);
        }


        public override void Render()
        {
            base.Render();

            // If not active, don't render
            if (!is_active) {return; }

            // Renders each positions stored by the game as a different color defined earlier
            for (int i = 0; i < player_positions.Count(); i++)
            {
                Collider pcollider = player_positions[i];
                Draw.HollowRect(pcollider, player_positions_color[i % player_positions_color.Count()]);
            };
        }


        public override void Reset()
        {
            base.Reset();

            // Resets the state's runtime variables
            is_active = true;
            PLAYER_POSITIONS_MAXCOUNT = 60 * TyporiumUtilitiesModule.Settings.Gameplay_States_Viewer_HitboxTrailInSecond;

            // Resets the saved level's camera informations
            original_camera_position = (Monocle.Engine.Scene as Level).Camera.Position;
            current_camera_offset = Vector2.Zero;
        }


        public override void Switched()
        {
            base.Switched();

            // Overwrite the camera's position to its original position
            is_active = false;
            (Monocle.Engine.Scene as Level).Camera.Position = original_camera_position;
        }


        public static void LevelUpdateMod(On.Celeste.Level.orig_Update orig, Level self)
        {

            // Update Level when not active
            if (!is_active)
            {

                // Updates storage of previous player positions
                if (player_positions.Count() > PLAYER_POSITIONS_MAXCOUNT)
                {
                    player_positions.RemoveAt(0);
                }

                Player player = self.Tracker.GetEntity<Player>();
                if (player != null)
                {
                    Hitbox hitbox_to_add = new(player.Width, player.Height);
                    hitbox_to_add.Center = player.Center;
                    player_positions.Add(hitbox_to_add);
                }

                // Original level update method
                orig(self);
                return;
            }

            // Update manually the state manager since all entities are blocked
            if (StateManager.shared_instance != null)
            {
                StateManager.shared_instance.Update();
            }

            // Moves camera
            float speed_modifier = Input.MenuConfirm.Check ? 8 : 3;

            current_camera_offset += new Vector2(speed_modifier * Input.MoveX.Value, speed_modifier * Input.MoveY.Value);
            self.Camera.Position = original_camera_position + current_camera_offset;
        }
    }
}