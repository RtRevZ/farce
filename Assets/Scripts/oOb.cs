using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using FARCEUtils;

public class oOb : MonoBehaviour
{

    public FARCE[] party = new FARCE[3];
    public GameObject PARTY;
    public GameObject mc, shade;

    public GameWarden gw;

    public bool complete = false, opportunity = false;

    private string par="CONTROL";
    private string psc="genesis";

    private void abs_partypos(GameObject EP)
    {
        PARTY.transform.position = EP.transform.position;
        PARTY.GetComponent<Party>().updatePartyPositions();
    }

    public int getLeader()
    {
        return PARTY.GetComponent<Party>().leader;
    }

    public IEnumerator sceneLoader(string ar, string sc, string ep) //load scene by area, scene, entrypoint
    {
        PARTY.GetComponent<Party>().updatePartyPosition();
        Vector3 partypos = PARTY.transform.position;
        shade.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        if (ar != par || (ar == par && sc != psc))
        {
            AsyncOperation async1, async2;

            if (ar != par)
            {
                /*  if minigame 1 cripple creek
                 * 
                 * 
                 *  if minigame 2 sea voyage
                 *  
                 *  
                 *  
                 *  if fight
                 *  
                 *      ar = par
                 * 
                 * 
                 *  finally oglethorpes
                 *  
                 *      do async load like below, set timeout according to distance array
                 * 
                 * 
                 * 
                 */
                if (ar == "CONTROL" && sc == "combat")
                {
                    SceneManager.LoadScene("Areas/CONTROL/load", LoadSceneMode.Single);
                    async1 = SceneManager.LoadSceneAsync("Areas/" + ar + "/" + sc, LoadSceneMode.Additive);
                    async1.allowSceneActivation = false;
                    mc.GetComponent<AudioListener>().enabled = false;
                    while (!async1.isDone) { yield return 0; }
                    PARTY.SetActive(false);

                    yield return new WaitForSeconds(5.0f);

                    async2 = SceneManager.UnloadSceneAsync("load");
                    while (!async2.isDone) { yield return 0; }
                    async1.allowSceneActivation = true;

                    //get comissioner
                    //while the commissioner is running the fight, do nothing

                    ar = par;
                    sc = psc;

                    while (!complete) { yield return 0; }
                    complete = false;
                }


            }

            PARTY.SetActive(false);

            SceneManager.LoadScene("Areas/CONTROL/load", LoadSceneMode.Single);

            async1 = SceneManager.LoadSceneAsync("Areas/" + ar + "/" + sc, LoadSceneMode.Additive);
            async1.allowSceneActivation = false;
            while (!async1.isDone) { yield return 0; }

            yield return new WaitForSeconds(5.0f);

            async2 = SceneManager.UnloadSceneAsync("load");
            while (!async2.isDone) { yield return 0; }
            async1.allowSceneActivation = true;

            mc.GetComponent<AudioListener>().enabled = true;




        }

        GameObject EP;

        if (ep == "TMP")
        {
            EP = new GameObject("TMP");
            EP.transform.position = partypos;
            abs_partypos(EP);
            Destroy(EP);
        } else
        {
            EP = GameObject.Find(ep);
            abs_partypos(EP);
            par = ar;
            psc = sc;
        }

        PARTY.SetActive(true);
        shade.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        yield break;
    }

    public void loadScene(string ar, string sc, string ep)
    {
        StopAllCoroutines();
        StartCoroutine(sceneLoader(ar, sc, ep));
    }

    public void mw(int amt)
    {
        party[0].mw += amt;
        party[1].mw += amt;
        party[2].mw += amt;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Debug.Log("AWAKE");
        gw = new GameWarden();


        party[0] = new FARCE(gw, "Digby Ketton", 2.6f, 0, 9);
        party[1] = new FARCE(gw, "Samuel , Son of Goerthe", 3f, 0, 1);
        party[2] = new FARCE(gw, "Hero the Enchanter", 2.8f, 0, 8);

        PARTY = gameObject.transform.GetChild(0).gameObject;
    }

    void Start()
    {
        loadScene("TEST1", "0", "EPOGLE");
    }



}