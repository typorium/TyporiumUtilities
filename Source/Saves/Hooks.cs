namespace Celeste.Mod.TyporiumUtilities_DEV.Saves
{


    public class Hooks
    {


        public static void Load()
        {
            
            // SaveFile Searcher
            SaveFileSearcher.Hooks.Load();

            // SaveFile Stats
            SaveFileStats.Hooks.Load();

            // SaveFile Swapper
            SaveFileSwapper.Hooks.Load();

            // SaveFile Others
            RemoveEmptyOnEnter.Hooks.Load();

        }
        

        public static void Unload()
        {

            // SaveFile Searcher
            SaveFileSearcher.Hooks.Unload();

            // SaveFile Stats
            SaveFileStats.Hooks.Unload();

            // SaveFile Swapper
            SaveFileSwapper.Hooks.Unload();

            // SaveFile Others
            RemoveEmptyOnEnter.Hooks.Unload();
        
        }
    }
}