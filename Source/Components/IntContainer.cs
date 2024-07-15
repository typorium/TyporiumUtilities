using Monocle;


namespace Celeste.Mod.TyporiumUtilities.Components
{


    public class IntContainer : Component
    {


        // Value
        private int value;
        private int original_value;

        private string name;


        // Constructor
        public IntContainer(string name, int value) : base(true, false)
        {

            // Values
            this.value = value;
            this.original_value = value;

            // Name
            this.name = name;

        }


        // Changes value
        public void Change(int value)
        {

            // Value
            this.value = value;

        }


        // Get value
        public int Get()
        {
            return this.value;
        }


        // Get name
        public string GetName()
        {
            return this.name;
        }


    }
}