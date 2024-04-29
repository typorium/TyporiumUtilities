using Celeste.Mod.Core;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;
using System.IO;


namespace Celeste.Mod.TyporiumUtilities.UI.SaveFile
{

    // Global stats little UI to see stats of all savefiles
    [Tracked(false)]
    public class TyporiumUtilities_OuiSaveFileStatsViewer : Entity
    {

        // Text Displayer
        static TextDisplayer time_textdisplayer = new TextDisplayer("0", MTN.Journal["time"], Vector2.UnitY * 0, text_amount_margin:0, scale:1.5f, scale_text:false);

        // Counters
        static Counter strawberry_counter = new Counter(0, GFX.Gui["collectables/strawberry"], time_textdisplayer.Position + Vector2.UnitY * time_textdisplayer.GetHeight(), text_amount_margin:0, scale:0.8f);
        static Counter death_counter = new Counter(0, GFX.Gui["collectables/skullBlue"], strawberry_counter.Position + Vector2.UnitY * strawberry_counter.GetHeight(), text_amount_margin:0, scale:0.8f);
        static Counter clear_counter = new Counter(0, MTN.Journal["clear"], death_counter.Position + Vector2.UnitY * death_counter.GetHeight(), scale:0.8f, text_amount_margin:0);
        static Counter golden_counter = new Counter(0, GFX.Gui["collectables/goldberry"], clear_counter.Position + Vector2.UnitY * clear_counter.GetHeight(), scale:0.7f, text_amount_margin:0);


        // Load, Unload
        public static void Load()
        {
            On.Celeste.OuiFileSelect.Enter += ModOuiFileSelectEnter;
            On.Celeste.OuiFileSelect.Leave += ModOuiFileSelectLeave;
        }

        public static void Unload()
        {
            On.Celeste.OuiFileSelect.Enter -= ModOuiFileSelectEnter;
            On.Celeste.OuiFileSelect.Leave -= ModOuiFileSelectLeave;
        }


        // Loads said stats in the FileSelect UI
        private static IEnumerator ModOuiFileSelectEnter(On.Celeste.OuiFileSelect.orig_Enter orig, OuiFileSelect self, Oui from)
        {
            IEnumerator toreturn = orig(self, from);

            self.Scene.Add(time_textdisplayer);

            self.Scene.Add(strawberry_counter);
            self.Scene.Add(death_counter);
            self.Scene.Add(clear_counter);
            self.Scene.Add(golden_counter);
            LoadData();

            return toreturn;
        }

        private static IEnumerator ModOuiFileSelectLeave(On.Celeste.OuiFileSelect.orig_Leave orig, OuiFileSelect self, Oui next)
        {
            IEnumerator toreturn = orig(self, next);

            self.Scene.Remove(time_textdisplayer);

            self.Scene.Remove(strawberry_counter);
            self.Scene.Remove(death_counter);
            self.Scene.Remove(clear_counter);
            self.Scene.Remove(golden_counter);

            return toreturn;
        }


        // Gets the number of savefile the user has
        private static int GetSaveFilesAmount()
        {
            int save_slots_number;
            if (CoreModule.Settings.MaxSaveSlots != null)
            {
                save_slots_number = Math.Max(3, CoreModule.Settings.MaxSaveSlots.Value);
            }
            else
            {
                save_slots_number = 1;
                string saveFilePath = UserIO.GetSaveFilePath(null);
                if (Directory.Exists(saveFilePath))
                {
                    string[] files = Directory.GetFiles(saveFilePath);
                    for (int i = 0; i < files.Length; i++)
                    {
                        string fileName = Path.GetFileName(files[i]);
                        int num2;
                        if (fileName.EndsWith(".celeste") && int.TryParse(fileName.Substring(0, fileName.Length - 8), out num2))
                        {
                            save_slots_number = Math.Max(save_slots_number, num2);
                        }
                    }
                }
                save_slots_number += 2;
            }
            return save_slots_number;
        }


        // Loads all necessary data
        private static void LoadData()
        {

            // If not readable, stop
            if (!UserIO.Open(UserIO.Mode.Read))
            {
                return;
            }

            // Resets data
            time_textdisplayer.ChangeText("0");

            strawberry_counter.ChangeAmount(0);
            death_counter.ChangeAmount(0);
            clear_counter.ChangeAmount(0);
            golden_counter.ChangeAmount(0);

            // For each slots
            long all_times = 0;

            int save_slots_number = GetSaveFilesAmount();
            for(int i = 0; i < save_slots_number; i++)
            {
                // If save doesn't exist, skip
                if (!UserIO.Exists(SaveData.GetFilename(i)))
                {
                    continue;
                }

                // Get data
                SaveData data = UserIO.Load<SaveData>(SaveData.GetFilename(i), false);
                if (data == null)
                {
                    continue;
                }
                

                // Add data
                golden_counter.ChangeAmountBy(data.TotalGoldenStrawberries);
                
                data.Areas_Safe.ForEach(area => {
                    strawberry_counter.ChangeAmountBy(area.TotalStrawberries);
                    death_counter.ChangeAmountBy(area.TotalDeaths);
                    all_times += area.TotalTimePlayed;

                    for(int j = 0; j < area.Modes.Length; j++)
                    {
                        clear_counter.ChangeAmountBy(area.Modes[j].Completed ? 1 : 0);  
                    }
                });

                data.LevelSetRecycleBin.ForEach(level_set => {
                    level_set.AreasIncludingCeleste.ForEach(area => {
                        strawberry_counter.ChangeAmountBy(area.TotalStrawberries);
                        death_counter.ChangeAmountBy(area.TotalDeaths);
                        all_times += area.TotalTimePlayed;

                        for(int j = 0; j < area.Modes.Length; j++)
                        {
                            clear_counter.ChangeAmountBy(area.Modes[j].Completed ? 1 : 0);  
                        }
                    });
                });
            }

            time_textdisplayer.ChangeText(Dialog.FileTime(all_times));

            UserIO.Close();
        }
    }
}