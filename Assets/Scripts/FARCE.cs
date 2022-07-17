using System.IO;
using System;
using UnityEngine;

namespace FARCEUtils
{

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

        public Tuple<string, int[], int> getFaunaAttack(int i, int j)
        {
            return new Tuple<string, int[], int>(tsvreader(fauna, i, 4).Split(',')[j], Array.ConvertAll(tsvreader(fauna, i, 6).Split(';')[j].Split(','), int.Parse), int.Parse(tsvreader(fauna, i, 5)));
        }

        public int[] getClassAttrs(int i)
        {
            return Array.ConvertAll(tsvreader(classes, i, 1).Split(','), int.Parse);
        }

        public int[] getClassSkills(int i)
        {
            return Array.ConvertAll(tsvreader(classes, i, 2).Split(','), int.Parse);
        }

        public int[] getClassStats(int i)
        {
            return Array.ConvertAll(tsvreader(classes, i, 3).Split(','), int.Parse);
        }

        public int[] getWeaponEffects(int i, int j)
        {
            string effect = tsvreader(weapons, i, 3).Split(';')[j];
            int[] ieffect = Array.ConvertAll(effect.Split(','), int.Parse);
            int amt = int.Parse(tsvreader(weapons, i, 2));
            return new int[] {ieffect[1], ieffect[0], amt, ieffect[2]};
        }

        public string getWeaponName(int i)
        {
            return tsvreader(weapons, i, 0);
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
        public int level, pclass, exp, ftype;

        public int[] attrs_lvl = new int[5]; //  Core Attributes - F.A.R.C.E. base, rng + class at creation, affected at level up
        public int[] attrs_tmp = new int[5]; //  Core Attributes - F.A.R.C.E. but with effects

        public int[] skilz_lvl = new int[10]; // Skills - from attrs_lvl + class at creation, affected at level up

        public int[] stats_lvl = new int[5]; // combat stats - from attrs + skils + class at creation, affected at level up
        public int[] stats_tmp = new int[5]; // combat stats - stats but with affects

        public int[,] effects = new int[3,10]; //effects 0 - type, 1 - amount, 2 - turns 

        public float speed, wt;

        public int boxact, mw = 0, rank = 0;

        public int weapon_id = 0;
        public int[] weapons = new int[6];

        public FARCE(GameWarden gw, string n, float spd, int lvl, int cls)
        {
            if(gw.getClassSpecial(cls) == 7) //special 7 indicates fauna, and that further data needs to be grabbed from the fauna tsv
            {
                name = gw.getFaunaName(lvl);
                ftype = lvl;
                level = gw.getFaunaLevel(lvl);
                stats_lvl = gw.getFaunaStats(lvl);
                stats_tmp = stats_lvl;
                pclass = cls;
                weapons[0] = 1;
                return;
            }

            //otherwise, presume classed n/pc

            name = n;
            speed = spd;
            boxact = gw.getClassBox(cls);

            stats_lvl = gw.getClassStats(cls);
            skilz_lvl = gw.getClassSkills(cls);
            attrs_lvl = gw.getClassAttrs(cls);

            for(int i = 0; i < attrs_lvl.Length; i++)
            {
                attrs_lvl[i] += UnityEngine.Random.Range(2, 5);
            }

            for (int i = 0; i < stats_lvl.Length; i++)
            {
                stats_lvl[i] += UnityEngine.Random.Range(2, 5);
            }

            stats_lvl[0] += attrs_lvl[0] / 3 + attrs_lvl[0] / 5;
            stats_lvl[1] += attrs_lvl[0] / 3 + attrs_lvl[1] / 5;
            stats_lvl[2] += attrs_lvl[0] / 3 + attrs_lvl[2] / 5;

            stats_tmp = stats_lvl;
            attrs_tmp = attrs_lvl;

            weapons[0] = 1;

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
                    if (attrs_tmp[atrstat] < 0)
                    {
                        attrs_tmp[atrstat] = 0;
                    }

                    if (attrs_tmp[atrstat] > attrs_lvl[atrstat])
                    {
                        attrs_tmp[atrstat] = attrs_lvl[atrstat];
                    }
                }
                else
                {
                    stats_tmp[atrstat % 5] += amount;

                    if (stats_tmp[atrstat % 5] < 0)
                    {
                        stats_tmp[atrstat % 5] = 0;
                    }

                    if (stats_tmp[atrstat % 5] > stats_lvl[atrstat % 5])
                    {
                        stats_tmp[atrstat % 5] = stats_lvl[atrstat % 5];
                    }
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

                if (attrs_tmp[i] < 0)
                {
                    attrs_tmp[i] = 0;
                }

                if (attrs_tmp[i] > attrs_lvl[i])
                {
                    attrs_tmp[i] = attrs_lvl[i];
                }

                if (stats_tmp[i] < 0)
                {
                    stats_tmp[i] = 0;
                }

                if (stats_tmp[i] > stats_lvl[i])
                {
                    stats_tmp[i] = stats_lvl[i];
                }
            }
        }
    }
}