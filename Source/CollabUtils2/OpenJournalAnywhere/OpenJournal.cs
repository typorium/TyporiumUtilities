using Monocle;
using System.Collections.Generic;
using Celeste.Mod.CollabUtils2.Triggers;
using System.Linq;
using Microsoft.Xna.Framework;
using Celeste.Mod.CollabUtils2.UI;
using System.Transactions;


namespace Celeste.Mod.TyporiumUtilities_DEV.ForCollabUtils2.OpenJournalAnywhere
{


    public static class Functions
    {


        public static void OpenJournal(Scene scene, Player player)
        {

            Level level = scene as Level;
            if (level == null)
            {
                return;
            }

            List<JournalTrigger> triggers = [.. level.Entities.OfType<JournalTrigger>()];

            JournalTrigger closest = null;
            float closest_distance = float.MaxValue;

            triggers.ForEach(current =>
            {
                float current_distance = Vector2.Distance(player.Center, current.Center);
                if (current_distance < closest_distance)
                {
                    closest = current;
                    closest_distance = current_distance;
                }
            });


            if (closest == null)
            {
                return;
            }

            InGameOverworldHelper.OpenJournal(player, closest.levelset);
        }

    }
}