using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The velocity along the y axis is 10 units per second.  If the GameObject starts at (0,0,0) then
// it will reach (0,100,0) units after 10 seconds.

public class Party : MonoBehaviour
{

    private GameObject oob, hero, digby, samuel, mcamera;

    public GameObject[] party = new GameObject[3];

    public int leader = 0;

    private bool doSwitch, stay;

    void Start()
    {
        oob = GameObject.Find("oOb");
        hero = GameObject.Find("Hero");
        digby = GameObject.Find("Digby");
        samuel = GameObject.Find("Samuel");
        mcamera = GameObject.Find("Main Camera");

        party[0] = digby;
        party[1] = samuel;
        party[2] = hero;

        party[0].AddComponent<CONTROL>();
        party[1].AddComponent<follow>();
        party[2].AddComponent<follow>();

        party[1].GetComponent<follow>().leader = party[leader];
        party[2].GetComponent<follow>().leader = party[leader];

        party[1].GetComponent<follow>().dist = 3f;
        party[2].GetComponent<follow>().dist = 4f;

        party[0].GetComponent<CONTROL>().speed = oob.GetComponent<oOb>().party[0].speed;
        party[1].GetComponent<follow>().speed = oob.GetComponent<oOb>().party[1].speed;
        party[2].GetComponent<follow>().speed = oob.GetComponent<oOb>().party[2].speed;


        mcamera.transform.SetParent(party[0].transform, false);


        mcamera.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = oob.GetComponent<oOb>().party[0].name;

        party[0].AddComponent<interact>();
    }

    void Update()
    {
        stay = Input.GetButtonDown("Stay");
        doSwitch = Input.GetButtonDown("Switch") || stay;

        mcamera.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = "Magic Wind: " + oob.GetComponent<oOb>().mw;

        if (doSwitch)
        {
            int lastLeader = leader++;

            int nextLeader = (leader + 1) % 3;

            leader %= 3;

            Destroy(party[lastLeader].GetComponent<CONTROL>());
            Destroy(party[leader].GetComponent<follow>());


            party[leader].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(3).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(3).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(3).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(4).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(4).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(2).GetChild(4).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";

            mcamera.transform.SetParent(party[leader].transform, false);

            mcamera.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = oob.GetComponent<oOb>().party[leader].name;

            Destroy(party[lastLeader].GetComponent<interact>());
            
            party[leader].AddComponent<interact>();
            party[leader].AddComponent<CONTROL>();
            party[leader].GetComponent<CONTROL>().speed = oob.GetComponent<oOb>().party[leader].speed;

            party[lastLeader].AddComponent<follow>();
            party[lastLeader].GetComponent<follow>().dist = 2f;
            party[lastLeader].GetComponent<follow>().leader = party[leader];
            party[lastLeader].GetComponent<follow>().speed = oob.GetComponent<oOb>().party[lastLeader].speed;
            if (stay) party[lastLeader].GetComponent<follow>().stay = true;
            party[lastLeader].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(3).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(3).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(3).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty"; 
            party[lastLeader].transform.GetChild(2).GetChild(4).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(4).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(2).GetChild(4).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";


            party[nextLeader].GetComponent<follow>().leader = party[leader];
            party[nextLeader].GetComponent<follow>().dist = 3f;
            party[nextLeader].GetComponent<follow>().speed = oob.GetComponent<oOb>().party[nextLeader].speed;
            party[nextLeader].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(3).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(3).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(3).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(4).GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(4).GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(2).GetChild(4).GetChild(2).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
        }

    }

    public void updatePartyPositions()
    {

        party[leader].transform.position = gameObject.transform.position;

        int m = 1;
        if (party[leader].transform.localScale.x == -1) m = -1;

        party[(leader + 2) % 3].transform.position = party[leader].transform.position - new Vector3(m * 2, 0, 0);
        party[(leader + 1) % 3].transform.position = party[leader].transform.position - new Vector3(m * 3, 0, 0);

        party[(leader + 2) % 3].GetComponent<follow>().stay = false;
        party[(leader + 1) % 3].GetComponent<follow>().stay = false;
    }

    public void updatePartyPosition()
    {

        gameObject.transform.position = party[leader].transform.position;
    }

}