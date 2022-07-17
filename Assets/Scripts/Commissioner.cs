using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FARCEUtils;
using UnityEngine.UI;
using System;

/* OYEZ! ALL RISE BEFORE THE HONORABLE
 * THE SUPREME COMMISSIONER OF THE GAME,
 * MISTER COMMISSIONER SIR,
 * BELOVED OF ALL GAME MASTERS,
 * UNDER WHOSE UNBIASED JUDGEMENT AND SUPERVISION THESE PROCEEDS COMMENCETH
 * IN A CHAOTIC, POTENTIALLY ILLICIT, AND WHOLLY LUDICROUS MANNER
 */

public class Commissioner : MonoBehaviour
{
    public GameObject disp, oob;
    public oOb oobc;
    public GameObject[] buttons;

    public GameObject[] pinfo;
    public GameObject[] tinfo;
    public GameObject[] attrs;
    public GameObject[] Mstats;
    public GameObject[] mStats;

    public AudioClip[] marches;

    private AudioSource audios;
    private bigboard bb;
    private int i;

    private int selection = -1;
    private bool down = true;

    private bool cphase, udnames=false;


    private FARCE ptarget, etarget;
    private List<FARCE> combatants = new List<FARCE>(), pmembers = new List<FARCE>(), emembers = new List<FARCE>();

    // Start is called before the first frame update
    void disableButtons()
    {
        foreach(GameObject button in buttons)
        {
            button.SetActive(false);
        }
    }

    void enableAndName(int n, string[] titles)
    {
        for (int i = 0; i < n; i++)
        {
            buttons[i].SetActive(true);
            buttons[i].transform.GetChild(0).gameObject.GetComponent<Text>().text = titles[i];
        }
    }

    IEnumerator printetdelay(string[] strs, float delay)
    {
        bb.reset();

        foreach(string str in strs)
        {
            bb.add_line(str);
        }

        yield return new WaitForSeconds(delay);

        bb.reset();
    }

    void manyprint(string[] strs)
    {
        foreach (string str in strs)
        {
            bb.add_line(str);
        }
    }

    IEnumerator pturn(FARCE farce)
    {
        ptarget = farce;

        farce.eval_effects();
        if (farce.stats_tmp[0] == 0)
        {
            int tmp = pmembers.IndexOf(farce);
            if(pmembers.Count - 1 != 0)
            {
                ptarget = pmembers[(tmp + 1) % pmembers.Count];
            }
            combatants.Remove(farce);
            pmembers.Remove(farce);
            yield break;
        }

        string[] button_titles = new string[6];

        bb.reset();

        if (!oobc.opportunity)
        {
            bb.add_line(farce.name + " engages");
        } else
        {
            bb.add_line(oobc.party[oobc.getLeader()].name + " is first to engage");
            oobc.opportunity = false;
        }

        /* prototyping space for turn structure
         * PHASE 1: ITEM
         *      In which the player can choose to perform up to two of any of these actions
         *          USE any consumable item
         *              Salves applied to self or left hand active item
         *          EQUIP any item in combat inventory to an empty active slot
         *          UNEQUIP any item in active inventory to combat inventory
         *          SWAP items in active inventory
         * 
         * 
         * PHASE 2: ACTION
         *      In which the player can choose to perform at most one of these actions
         *          USE any consumable item
         *          Use any weapon/class special
         *              DWTH if DW'ing will be actioned 2x
         */

        cphase = false;

        do //ITEM PHASE
        {
            //What do?
            //USE
            //EQUIP
            //UNEQUIP

            button_titles[0] = "USE";
            button_titles[1] = "UN/EQUIP";

            manyprint(new string[] { "What will " + farce.name.Split(' ')[0] + " do?", "", "USE a consumable item,", "or, UN/EQUIP an item" });

            enableAndName(2, button_titles);

            selection = -1;

            while (selection == -1)
            {
                yield return null;
            }

            if (selection == -2)
            {
                break;
            }// break to check
            if (selection == -3)
            {
                yield break;
            }// abruptly end turn coroutine on pass
            if (selection == -4)
            {
                continue; //go back to top of turn
            }

            if (selection == 0)
            {
                //use
            }

            if (selection == 1)
            {
                if(farce.weapon_id != 0) //unequip
                {
                    yield return StartCoroutine(printetdelay(new string[] { "Unequipped " + oobc.gw.getWeaponName(farce.weapons[farce.weapon_id]) }, 3f));
                    farce.weapon_id = 0;
                }
                else if(farce.weapons[0] != 0) //equip if there are weapons
                {
                    bb.reset();
                    bb.add_line("");
                    int tmp = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        if (farce.weapons[i] == 0)
                        {
                            tmp = i;
                            break;
                        }

                        bb.add_line((i + 1).ToString() + ": " + oobc.gw.getWeaponName(farce.weapons[i]));
                        button_titles[i] = (i + 1).ToString();
                    }

                    enableAndName(tmp, button_titles);

                    selection = -1;

                    while (selection == -1)
                    {
                        yield return null;
                    }

                    if (selection == -2)
                    {
                        break;
                    }// break to check
                    if (selection == -3)
                    {
                        yield break;
                    }// abruptly end turn coroutine on pass
                    if (selection == -4)
                    {
                        continue; //go back to top of turn
                    }

                    yield return StartCoroutine(printetdelay(new string[] { "Equipped " + oobc.gw.getWeaponName(farce.weapons[selection]) }, 3f));
                    farce.weapon_id = farce.weapons[selection];
                }
                else
                {
                    yield return StartCoroutine(printetdelay(new string[] { "No Weapons to Equip." }, 3f));
                    continue;
                }
            }


            cphase = true;
        } while (!cphase);

        cphase = false;

        bb.reset();

        do //ACTION PHASE
        {
            //What do?
            //USE
            //EQUIP
            //UNEQUIP

            button_titles[0] = "USE";
            button_titles[1] = "ATTACK";
            button_titles[2] = "SPECIAL";

            manyprint(new string[] { "What will " + farce.name.Split(' ')[0] + " do now?", "", "USE a consumable item,", "ATTACK using the active weapon,", "or, perform a SPECIAL attack" });

            enableAndName(3, button_titles);

            selection = -1;

            while (selection == -1)
            {
                yield return null;
            }

            if (selection == -2)
            {
                break;
            }// break to check
            if (selection == -3)
            {
                yield break;
            }// abruptly end turn coroutine on pass
            if (selection == -4)
            {
                continue; //go back to top of turn
            }

            if (selection == 1)
            {
                int[] weapon_effect = oobc.gw.getWeaponEffects(farce.weapon_id, 0);

                if (farce.stats_tmp[1] < 1)
                {
                    bb.reset();
                    yield return StartCoroutine(printetdelay(new string[] { "Not Enough Stamina" }, 3f));
                    continue;
                }

                farce.stats_tmp[1] -= 1;

                etarget.apply_effect(weapon_effect[0], weapon_effect[1], weapon_effect[2], weapon_effect[3]);
                if (etarget.stats_tmp[0] == 0)
                {
                    int tmp = emembers.IndexOf(etarget);
                    if (emembers.Count - 1 != 0)
                    {
                        FARCE ftp;
                        ftp = emembers[(tmp + 1) % emembers.Count];
                        combatants.Remove(etarget);
                        emembers.Remove(etarget);
                        etarget = ftp;
                    } else
                    {
                        combatants.Remove(etarget);
                        emembers.Remove(etarget);
                        yield break;
                    }

                }
            }

            if (selection == 2)
            {
                switch (oobc.gw.getClassSpecial(farce.pclass)) {
                    case 0:
                        button_titles[0] = "Dual Strike";
                        button_titles[1] = "Two Handed";

                        manyprint(new string[] { "What kind?", "", "Dual Strike", "Two Handed"});

                        enableAndName(2, button_titles);

                        int[] weapon_effect = oobc.gw.getWeaponEffects(farce.weapon_id, 0);

                        selection = -1;

                        while (selection == -1)
                        {
                            yield return null;
                        }

                        if (selection == -2)
                        {
                            break;
                        }// break to check
                        if (selection == -3)
                        {
                            yield break;
                        }// abruptly end turn coroutine on pass
                        if (selection == -4)
                        {
                            continue; //go back to top of turn
                        }

                        if (selection == 0)
                        {
                            if(farce.stats_tmp[1] < 2)
                            {
                                bb.reset();
                                yield return StartCoroutine(printetdelay(new string[] { "Not Enough Stamina" }, 3f));
                                continue;
                            }
                            if (farce.stats_tmp[2] < 1)
                            {
                                bb.reset();
                                yield return StartCoroutine(printetdelay(new string[] { "Not Enough Power" }, 3f));
                                continue;
                            }

                            farce.stats_tmp[1] -= 2;
                            farce.stats_tmp[2] -= 1;

                            weapon_effect[2] *= UnityEngine.Random.Range(0, 3); 
                        }

                        if (selection == 1)
                        {
                            if (farce.stats_tmp[1] < 3)
                            {
                                bb.reset();
                                yield return StartCoroutine(printetdelay(new string[] { "Not Enough Stamina" }, 3f));
                                continue;
                            }
                            if (farce.stats_tmp[2] < 2)
                            {
                                bb.reset();
                                yield return StartCoroutine(printetdelay(new string[] { "Not Enough Power" }, 3f));
                                continue;
                            }

                            farce.stats_tmp[1] -= 3;
                            farce.stats_tmp[2] -= 2;

                            weapon_effect[2] *= 2;
                        }

                        etarget.apply_effect(weapon_effect[0], weapon_effect[1], weapon_effect[2], weapon_effect[3]);
                        if (etarget.stats_tmp[0] == 0)
                        {
                            int tmp = emembers.IndexOf(etarget);
                            if (emembers.Count - 1 != 0)
                            {
                                FARCE ftp;
                                ftp = emembers[(tmp + 1) % emembers.Count];
                                combatants.Remove(etarget);
                                emembers.Remove(etarget);
                                etarget = ftp;
                            }
                            else
                            {
                                combatants.Remove(etarget);
                                emembers.Remove(etarget);
                                yield break;
                            }

                        }
                        break;
                    case 1:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                    case 6:

                        break;

                }
            }

            cphase = true;
        } while (!cphase);
        disableButtons();
        //Duplicate Item & action phases with conditions and prob
    }

    IEnumerator eturn(FARCE farce)
    {

        etarget = farce;
        farce.eval_effects();
        if(farce.stats_tmp[0] == 0)
        {
            int tmp = emembers.IndexOf(farce);
            if (emembers.Count - 1 != 0)
            {
                etarget = emembers[(tmp + 1) % emembers.Count];
            }
            combatants.Remove(farce);
            emembers.Remove(farce);
            yield break;
        }
        //"literally the same as pturn but rng based on opponent AI type: flailing (pure rng), ...
        if (farce.pclass == 10)
        {
            yield return StartCoroutine(printetdelay(new string[] { farce.name + "ly actions", "CP: " + farce.stats_tmp[0].ToString() }, 3f));

            Tuple<string, int[], int> atk = oobc.gw.getFaunaAttack(farce.ftype, 0);

            ptarget.apply_effect(atk.Item2[1], atk.Item2[0], atk.Item3, atk.Item2[2]);
            if (ptarget.stats_tmp[0] == 0)
            {
                int tmp = pmembers.IndexOf(ptarget);
                if (pmembers.Count - 1 != 0)
                {
                    FARCE ftp;
                    ftp = pmembers[(tmp + 1) % pmembers.Count];
                    combatants.Remove(ptarget);
                    pmembers.Remove(ptarget);
                    ptarget = ftp;
                }
                else
                {
                    combatants.Remove(ptarget);
                    pmembers.Remove(ptarget);
                    yield break;
                }

            }
        }
        else
        {

        }
        
        yield return null;
    }



    IEnumerator fightLoop()
    {

        //generate opponents, fauna in bushes, otherwise people of specified classes (not enchanter) 
        pmembers.AddRange(oobc.party);
        emembers.AddRange(new FARCE[] { new FARCE(oobc.gw, "", 0f, 1, 10), new FARCE(oobc.gw, "", 0f, 1, 10) });

        combatants.AddRange(emembers);
        combatants.AddRange(pmembers);

        ptarget = oobc.party[oobc.getLeader()];
        etarget = combatants[0];

        udnames = true;


        if (oobc.opportunity == true)
        {
            yield return StartCoroutine(pturn(oobc.party[oobc.getLeader()]));
        }

        while (down)
        {
            for(int i = 0; i < combatants.Count; i++)
            {
                if (Array.Exists(oobc.party, member => member == combatants[i])) {
                    yield return StartCoroutine(pturn(combatants[i]));
                } else {
                    yield return StartCoroutine(eturn(combatants[i]));
                }

                if(pmembers.Count == 0 || emembers.Count == 0)
                {
                    down = false;
                    break;
                }
            }
        }

        if(pmembers.Count == 0)
        {
            yield return StartCoroutine(printetdelay(new string[] { "Opponent(s) Win[s]" }, 3f));
        }

        if(emembers.Count == 0)
        {
            yield return StartCoroutine(printetdelay(new string[] { "Player Wins" }, 3f));
        }

        oobc.complete = true;
    }

    IEnumerator Start()
    {
        bb = disp.GetComponent<bigboard>();
        oob = GameObject.Find("oOb");

        oobc = oob.GetComponent<oOb>();

        audios = gameObject.GetComponent<AudioSource>();

        i = UnityEngine.Random.Range(0, 2);

        audios.clip = marches[i];

        disableButtons();
        audios.PlayDelayed(5f);

        yield return StartCoroutine(printetdelay(new string[] { "OYEZ! ALL RISE BEFORE THE HONORABLE AND MOST RIGHTEOUS, ", "THE SUPREME COMMISSIONER OF THE GAME, MISTER COMMISSIONER SIR,", "BELOVED OF ALL GAME MASTERS, UNDER WHOSE UNBIASED", "JUDGEMENT AND SUPERVISION THESE PROCEEDS COMMENCETH IN A", "CHAOTIC, POTENTIALLY ILLICIT, AND WHOLLY LUDICROUS MANNER..." }, 10f));

        StartCoroutine(fightLoop());

    }



    void buttonPress()
    {
        selection = 0;
        bb.add_line("0");
        disableButtons();
    }

    void buttonPress1()
    {
        selection = 1;
        bb.add_line("1");
        disableButtons();
    }

    void buttonPress2()
    {
        selection = 2;
        bb.add_line("2");
        disableButtons();
    }

    void buttonPress3()
    {
        selection = 3;
        bb.add_line("3");
        disableButtons();
    }

    void buttonPress4()
    {
        selection = 4;
        bb.add_line("4");
        disableButtons();
    }

    void buttonPress5()
    {
        selection = 5;
        bb.add_line("5");
        disableButtons();
    }

    IEnumerator bail()
    {
        yield return StartCoroutine(printetdelay(new string[] { "Dude... Bail?", "   Uhm yeah. Bail.", "       Don't need to tell me twice" }, 3f));
        oobc.complete = true;
    }

    void buttonPressBAIL()
    {
        StopAllCoroutines();
        disableButtons();
        audios.Stop();
        StartCoroutine(bail());
    }

    void buttonPressCHECK()
    {
        selection = -2;
    }

    void buttonPressPASS()
    {
        selection = -3;
    }

    void buttonPressBACK()
    {
        selection = -4;
    }


    // Update is called once per frame
    void Update()
    {
        if (!audios.isPlaying)
        {
            if (i == 0)
            {
                i = 1;
            } else i = 0;

            audios.clip = marches[i];
            audios.Play();
        }

        if (udnames) {
            pinfo[0].GetComponent<Text>().text = ptarget.name;
            pinfo[1].GetComponent<Text>().text = oobc.gw.getClassName(ptarget.pclass);
            pinfo[2].GetComponent<Text>().text = ptarget.level.ToString();

            for (int i = 0; i < 5; i++) // attributes
            {
                attrs[i].GetComponent<Text>().text = ptarget.attrs_tmp[i].ToString();
            }

            for (int i = 0; i < 3; i ++) // attributes
            {
                Mstats[2 * i].GetComponent<Text>().text = (ptarget.stats_tmp[i] / 3).ToString();
                Mstats[2 * i + 1].GetComponent<Text>().text = (ptarget.stats_tmp[i] % 3).ToString();
            }

            mStats[0].GetComponent<Text>().text = ptarget.stats_tmp[3].ToString();
            mStats[1].GetComponent<Text>().text = ptarget.stats_tmp[4].ToString();

            tinfo[0].GetComponent<Text>().text = etarget.name;
            tinfo[1].GetComponent<Text>().text = oobc.gw.getClassName(etarget.pclass);
            tinfo[2].GetComponent<Text>().text = etarget.level.ToString();


        }

    }
}
