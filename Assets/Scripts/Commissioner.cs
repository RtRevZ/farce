using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* OYEZ! ALL RISE BEFORE THE HONORABLE
 * THE SUPREME COMMISSIONER OF THE GAME,
 * MISTER COMMISSIONER SIR,
 * UNDER WHOSE UNBIASED JUDGEMENT AND SUPERVISION THESE PROCEEDS COMMENCETH
 * IN A CHAOTIC, POTENTIALLY ILLICIT, AND WHOLLY LUDICROUS MANNER
 */

public class Commissioner : MonoBehaviour
{
    public GameObject disp;
    public GameObject[] buttons;
    public AudioClip[] marches;

    private AudioSource audios;
    private bigboard bb;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        bb = disp.GetComponent<bigboard>();

        bb.add_line("OYEZ! ALL RISE BEFORE THE HONORABLE");
        bb.add_line("THE SUPREME COMMISSIONER OF THE GAME,");
        bb.add_line("MISTER COMMISSIONER SIR, UNDER WHOSE UNBIASED");
        bb.add_line("JUDGEMENT AND SUPERVISION THESE PROCEEDS COMMENCETH");
        bb.add_line("IN A CHAOTIC, POTENTIALLY ILLICIT, AND WHOLLY LUDICROUS MANNER");

        audios = gameObject.GetComponent<AudioSource>();

        i = Random.Range(0, 2);

        audios.clip = marches[i];
        audios.Play();
        //get oob
        //get party farces
        //generate opponents, fauna in bushes, otherwise people of specified classes (not enchanter) 
        //determine turn order
        //add tmp 0th turn for leader first to attack if bush and not from timeout
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

            audios.clip = marches[Random.Range(0, 2)];
            audios.Play();
        }
    }
}
