namespace FARCEUtils
{



    public class FARCE //character sheet
    {

        public string name;

        //player base properties
        public int level, pclass, exp;

        public int[] attrs = new int[5]; // Core Attributes - F.A.R.C.E.
        public int[] tmp_attrs = new int[5];

        public int[] skilz = new int[10]; // Skills/Bonuses
        public int[] tmp_skilz = new int[10];

        public int[] stats = new int[5]; // Combat Stats
        public int[] tmp_stats = new int[5];

        public float speed, tmp_spd, wt, tmp_wt;

        public int boxact, mw = 0;

        public FARCE(string n, float spd, int bxact)
        {
            name = n;
            speed = spd;
            boxact = bxact;
        }

        private void lvlup()
        {
            //handle  
        }
    }
}