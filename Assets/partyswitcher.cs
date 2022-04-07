using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The velocity along the y axis is 10 units per second.  If the GameObject starts at (0,0,0) then
// it will reach (0,100,0) units after 10 seconds.

public class partyswitcher : MonoBehaviour
{

    public GameObject hero, digby, samuel, mcamera;
    private GameObject oob;

    private GameObject[] party = new GameObject[3];

    private int leader;



    void Start()
    {
        party[0] = digby;
        party[1] = samuel;
        party[2] = hero;

        oob = GameObject.Find("oOb");
        leader = oob.GetComponent<oOb>().leader;
        gameObject.GetComponent<CONTROL>().leader = party[leader];

        gameObject.GetComponent<follow>().leader = party[leader];
        gameObject.GetComponent<follow>().mFollower = party[(leader + 1) % 3];
        gameObject.GetComponent<follow>().rFollower = party[(leader + 2) % 3];



        mcamera.transform.SetParent(party[leader].transform, false);


        mcamera.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = oob.GetComponent<oOb>().party[leader].name;

        party[leader].AddComponent<teleport>();
    }

    void Update()
    {
        bool doSwitch;
        doSwitch = Input.GetButtonDown("Switch");

        if (doSwitch)
        {
            int lastLeader = leader++;

            int nextLeader = (leader + 1) % 3;

            leader %= 3;
            oob.GetComponent<oOb>().leader = leader;

            gameObject.GetComponent<CONTROL>().enabled = false;
            gameObject.GetComponent<follow>().enabled = false;

            party[leader].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";
            party[leader].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "FParty";

            mcamera.transform.SetParent(party[leader].transform, false);

            mcamera.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = oob.GetComponent<oOb>().party[leader].name;

            Destroy(party[lastLeader].GetComponent<teleport>());


            party[leader].AddComponent<teleport>();


            gameObject.GetComponent<CONTROL>().leader = party[leader];


            party[lastLeader].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";
            party[lastLeader].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "MParty";

            party[nextLeader].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";
            party[nextLeader].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "RParty";

            gameObject.GetComponent<follow>().leader = party[leader];
            gameObject.GetComponent<follow>().mFollower = party[lastLeader];
            gameObject.GetComponent<follow>().rFollower = party[nextLeader];

            gameObject.GetComponent<follow>().enabled = true;
            gameObject.GetComponent<CONTROL>().enabled = true;


        }

    }

}