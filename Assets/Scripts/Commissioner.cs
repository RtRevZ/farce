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
    public bool called = false;

    private AudioSource audios;
    private bigboard bb;
    private int i;

    private int selection = -1;
    private bool down = true;

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

    IEnumerator pturn(FARCE farce)
    {
        pinfo[0].GetComponent<Text>().text = farce.name;
        pinfo[1].GetComponent<Text>().text = INFO.classes[farce.pclass];
        pinfo[2].GetComponent<Text>().text = farce.level.ToString();

        if (!oobc.opportunity)
        {
            bb.add_line(farce.name + " attacks");
        } else
        {
            bb.add_line(oobc.party[oobc.getLeader()].name + " attacks first");
            oobc.opportunity = false;
        }

        /* prototyping space for turn structure
         * 
         * phases can be CHECKed at any point
         * turns can be PASSED likewise
         * attempts to BAIL, on failure, will PASS the present turn, otherwise bailing
         * On bail, the Commissioner will randomly applaud or chastise the cowardice of the player, then determine whether the player will bail
         * 
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
         *              Salves applied to self only
         *          
         * 
         * (PHASE 3: BONUS ITEM)
         *      Chance with appropriate class
         * 
         * (PHASE 4: BONUS ACTION)
         *      Chance with appropriate class
         * 
         */

        string[] button_titles = new string[6];

        button_titles[0] = "ONE";
        button_titles[1] = "TWO";
        button_titles[2] = "THREE";

        enableAndName(3, button_titles);

        while (selection == -1)
        {
            yield return null;
        }

        if (selection == -3) down = false;

        selection = -1;

        yield return null;
    }

    IEnumerator eturn(FARCE farce)
    {
        //"literally the same as pturn but rng based on opponent AI type: flailing (pure rng), ...
        yield return null;
    }


    IEnumerator fightLoop()
    {

        //generate opponents, fauna in bushes, otherwise people of specified classes (not enchanter) 
        FARCE[] combatants = oobc.party;

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
        audios.Play();

        disableButtons();

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

        StartCoroutine(bail());

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
