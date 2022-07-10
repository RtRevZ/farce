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

    private bool cphase;


    private FARCE ptarget, etarget;

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
        pinfo[0].GetComponent<Text>().text = farce.name;
        pinfo[1].GetComponent<Text>().text = oobc.gw.getClassName(farce.pclass);
        pinfo[2].GetComponent<Text>().text = farce.level.ToString();

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
            button_titles[1] = "EQUIP";
            button_titles[2] = "UNEQUIP";

            manyprint(new string[] { "What will " + farce.name.Split(' ')[0] + " do?", "", "USE a consumable item,", "EQUIP an item,", "or, UNEQUIP an item" });

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

            cphase = true;
        } while (!cphase);

        cphase = false;

        do //ITEM PHASE
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

                etarget.apply_effect(weapon_effect[0], weapon_effect[1], weapon_effect[2], weapon_effect[3]);
            }

            cphase = true;
        } while (!cphase);

        //Duplicate Item & action phases with conditions and prob
    }

    IEnumerator eturn(FARCE farce)
    {
        //"literally the same as pturn but rng based on opponent AI type: flailing (pure rng), ...
        yield return StartCoroutine(printetdelay(new string[] { farce.name + "ly actions", "CP: " + farce.stats_tmp[0].ToString() }, 5f));
        yield return null;
    }


    IEnumerator fightLoop()
    {

        //generate opponents, fauna in bushes, otherwise people of specified classes (not enchanter) 
        FARCE[] combatants = { new FARCE(oobc.gw, "", 0f, 1, 10), oobc.party[0], oobc.party[1], oobc.party[2] };

        ptarget = oobc.party[oobc.getLeader()];
        etarget = combatants[0];

        pinfo[0].GetComponent<Text>().text = ptarget.name;
        pinfo[1].GetComponent<Text>().text = oobc.gw.getClassName(ptarget.pclass);
        pinfo[2].GetComponent<Text>().text = ptarget.level.ToString();

        tinfo[0].GetComponent<Text>().text = etarget.name;
        tinfo[1].GetComponent<Text>().text = oobc.gw.getClassName(etarget.pclass);
        tinfo[2].GetComponent<Text>().text = etarget.level.ToString();

        if (oobc.opportunity == true)
        {
            yield return StartCoroutine(pturn(oobc.party[oobc.getLeader()]));
        }

        while (down)
        {
            foreach (FARCE combatant in combatants)
            {
                if (Array.Exists(oobc.party, member => member == combatant)) {
                    yield return StartCoroutine(pturn(combatant));
                } else yield return StartCoroutine(eturn(combatant));
            }
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
    }
}
