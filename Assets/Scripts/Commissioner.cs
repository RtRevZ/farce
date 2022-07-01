using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject[] buttons;
    public AudioClip[] marches;
    public bool called = false;

    private AudioSource audios;
    private bigboard bb;
    private int i;

    private int selection = -1;

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

    IEnumerator fightLoop()
    {
        string[] button_titles = new string[6];

        bool down = true;

        while (down)
        {
            button_titles[0] = "ONE";
            button_titles[1] = "TWO";
            button_titles[2] = "THREE";
            
            enableAndName(3, button_titles);

            while(selection == -1)
            {
                yield return null;
            }

            if (selection == -2) oob.GetComponent<oOb>().complete = true;

            if (selection == -3) down = false;

            selection = -1;

            yield return null;
        }



        oob.GetComponent<oOb>().complete = true;
    }

    void Start()
    {
        bb = disp.GetComponent<bigboard>();
        oob = GameObject.Find("oOb");

        bb.add_line("OYEZ! ALL RISE BEFORE THE HONORABLE AND MOST GRACIOUS, ");
        bb.add_line("THE SUPREME COMMISSIONER OF THE GAME, MISTER COMMISSIONER SIR,");
        bb.add_line("BELOVED OF ALL GAME MASTERS, UNDER WHOSE UNBIASED");
        bb.add_line("JUDGEMENT AND SUPERVISION THESE PROCEEDS COMMENCETH IN A");
        bb.add_line("CHAOTIC, POTENTIALLY ILLICIT, AND WHOLLY LUDICROUS MANNER...");

        audios = gameObject.GetComponent<AudioSource>();

        i = Random.Range(0, 2);

        audios.clip = marches[i];
        audios.Play();

        disableButtons();
        //get party farces
        //generate opponents, fauna in bushes, otherwise people of specified classes (not enchanter) 
        //determine turn order
        //add tmp 0th turn for leader first to attack if bush and not from timeout

        StartCoroutine(fightLoop());

    }



    void buttonPress()
    {
        selection = 0;
        disableButtons();
    }

    void buttonPress1()
    {
        selection = 1;
        disableButtons();
    }

    void buttonPress2()
    {
        selection = 2;
        disableButtons();
    }

    void buttonPress3()
    {
        selection = 3;
        disableButtons();
    }

    void buttonPress4()
    {
        selection = 4;
        disableButtons();
    }

    void buttonPress5()
    {
        selection = 5;
        disableButtons();
    }

    void buttonPressBAIL()
    {
        selection = -2;
        disableButtons();
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
