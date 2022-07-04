namespace FARCEUtils
{

    public class item
    {
        public bool stackable = false;
        public int quantity;
        public int value;
        public float weight;
    }

    public class INFO
    {
        static public string[] classes = { "Pugilist", "Woodsman", "Warrior", "Scout", "Defender", "Man-At-Large", "Bard", "Salesman", "Enchanter", "Monk"};
    }


    public class FARCE //character sheet
    {

        public string name;

        //player base properties
        public int level, pclass, exp;

        public int[] attrs = new int[5]; // Core Attributes - F.A.R.C.E. base, affected at level up
        public int[] tmp_attrs = new int[5]; //FARCE with effects

        public int[] skilz = new int[10]; // Skills/Bonuses base (calc'd from farce)
        public int[] skilzz = new int[10]; // Skills/Bonuses added by level up
        public int[] tmp_skilz = new int[10]; //skills/bonuses with effects

        public int[] stats = new int[5]; // Combat Stats (calc'd however)
        public int[] statss = new int[5]; // Combat Stats added by level up
        public int[] tmp_stats = new int[5]; // Combat stats with effects

        public float speed, wt;

        public int boxact, mw = 0;

        public item[,] inventory = new item[2, 36]; //0 - regular: 1 - combat


        public FARCE(string n, float spd, int bxact, int lvl, int cls)
        {
            name = n;
            speed = spd;
            boxact = bxact;
            level = lvl;
            pclass = cls;
        }

        public void recalculate()
        {

        }

        private void lvlup()
        {
            //handle  
        }
    }
}