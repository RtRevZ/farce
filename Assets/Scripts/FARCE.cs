using System.IO;
using System;
using UnityEngine;

namespace FARCEUtils
{

    public class item
    {
        public bool stackable = false;
        public int quantity;
        public int value;
        public float weight;
    }

    public class GameWarden
    {
        // Start is called before the first frame update
        string[] fauna;
        string[] classes;
        string[] weapons;

        string tsvreader(string[] lit, int ent, int i)
        {
            return lit[ent].Split('\t')[i];
        }

        public string getClassName(int i)
        {
            return tsvreader(classes, i, 0);
        }

        public int getClassBox(int i)
        {
            return int.Parse(tsvreader(classes, i, 4));
        }

        public int getClassSpecial(int i)
        {
            return int.Parse(tsvreader(classes, i, 6));
        }

        public string getFaunaName(int i)
        {
            return tsvreader(fauna, i, 0);
        }

        public int getFaunaLevel(int i)
        {
            return int.Parse(tsvreader(fauna, i, 7));
        }

        public int[] getFaunaAttrs(int i)
        {
            return Array.ConvertAll(tsvreader(fauna, i, 2).Split(','), int.Parse);
        }

        public int[] getFaunaStats(int i)
        {
            return Array.ConvertAll(tsvreader(fauna, i, 3).Split(','), int.Parse);
        }

        public int[] getWeaponEffects(int i, int j)
        {
            string effect = tsvreader(weapons, i, 3).Split(';')[j];
            int[] ieffect = Array.ConvertAll(effect.Split(','), int.Parse);
            int amt = int.Parse(tsvreader(weapons, i, 2));
            return new int[] {ieffect[1], ieffect[0], amt, ieffect[2]};
        }

        public GameWarden()
        {
            fauna = File.ReadAllLines(Application.dataPath + "/StreamingAssets/fauna.txt");
            classes = File.ReadAllLines(Application.dataPath + "/StreamingAssets/classes.txt");
            weapons = File.ReadAllLines(Application.dataPath + "/StreamingAssets/weapons.txt");
        }


    }


    public class FARCE //character sheet
    {

        public string name;

        //player base properties
        public int level, pclass, exp;

        public int[] attrs_lvl = new int[5]; //  Core Attributes - F.A.R.C.E. base, rng + class at creation, affected at level up
        public int[] attrs_tmp = new int[5]; //  Core Attributes - F.A.R.C.E. but with effects

        public int[] skilz_lvl = new int[10]; // Skills - from attrs_lvl + class at creation, affected at level up

        public int[] stats_lvl = new int[5]; // combat stats - from attrs + skils + class at creation, affected at level up
        public int[] stats_tmp = new int[5]; // combat stats - stats but with affects

        public int[,] effects = new int[3,10]; //effects 0 - type, 1 - amount, 2 - turns 

        public float speed, wt;

        public int boxact, mw = 0, rank = 0;

        public int weapon_id = 0;

        public FARCE(GameWarden gw, string n, float spd, int lvl, int cls)
        {
            if(gw.getClassSpecial(cls) == 7) //special 7 indicates fauna, and that further data needs to be grabbed from the fauna tsv
            {
                name = gw.getFaunaName(lvl);
                level = gw.getFaunaLevel(lvl);
                stats_lvl = gw.getFaunaStats(lvl);
                stats_tmp = stats_lvl;
                pclass = cls;
                return;
            }

            //otherwise, presume classed n/pc

            name = n;
            speed = spd;
            boxact = gw.getClassBox(cls);
            level = lvl;
            pclass = cls;
        }

        private void lvlup()
        {
        }

        public void apply_effect(int atrstat, int type, int amount, int turns = 0)
        {
            if(type == 0)
            {
                if(atrstat < 5)
                {
                    attrs_tmp[atrstat] += amount;
                }
                else
                {
                    stats_tmp[atrstat % 5] += amount;
                }

                return;
            }


            if(type == 3)
            {
                amount = amount / turns;
                type = 1;
            }

            effects[0, atrstat] = type;
            effects[1, atrstat] = amount;
            effects[2, atrstat] = turns;
        }

        public void eval_effects()
        {
            for(int i = 0; i < 5; i++)
            {
                if (effects[2, i] != 0)
                {
                    if(effects[0, i] == 1) //repeating effect
                    {
                        attrs_tmp[i] += effects[1, i];
                    }

                    if (effects[0, i] == 2) //diminishing effect
                    {
                        attrs_tmp[i] += effects[1, i];
                        effects[1, i] = effects[1, i] * (effects[2, i] - 1) / effects[2, i];
                    }

                    effects[2, i]--;

                    if(effects[2, i] == 0)
                    {
                        effects[0, i] = 0;
                        effects[1, i] = 0;
                    }
                }

                if (effects[2, i + 5] != 0)
                {
                    if (effects[2, i + 5] != 0)
                    {
                        if (effects[0, i + 5] == 1) //repeating effect
                        {
                            stats_tmp[i] += effects[1, i + 5];
                        }

                        if (effects[0, i] == 2) //diminishing effect
                        {
                            stats_tmp[i] += effects[1, i + 5];
                            effects[1, i + 5] = effects[1, i + 5] * (effects[2, i + 5] - 1) / effects[2, i + 5];
                        }

                        effects[2, i + 5]--;

                        if (effects[2, i + 5] == 0)
                        {
                            effects[0, i + 5] = 0;
                            effects[1, i + 5] = 0;
                        }
                    }
                }
            }
        }
    }
}