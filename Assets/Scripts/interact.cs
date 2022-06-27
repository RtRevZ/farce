using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class interact : MonoBehaviour //rename to use
{

    private GameObject oob;
    private oOb oOb_comp;
    private IEnumerator coroutine;

    void Start()
    {
        oob = GameObject.Find("oOb");
        oOb_comp = oob.GetComponent<oOb>();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "tp_up" && Input.GetButtonDown("Up"))
        {
            abs_load(collider);
        }
        if (collider.gameObject.tag == "tp_down" && Input.GetButtonDown("Down"))
        {
            abs_load(collider);
        }
        if (collider.gameObject.tag == "switch" && Input.GetButtonDown("Use"))
        {
            collider.gameObject.GetComponent<BS>().Toggle();
        }
        if (collider.gameObject.tag == "tp_door" && Input.GetButtonDown("Use"))
        {
            //if the door is locked
            //.. check for key
            //.. if key: abs_load(collider);
            //else 
            if (collider.gameObject.GetComponent<door>().locked)
            {

            }
            else
            {
                abs_load(collider);
            }
        }
        if (collider.gameObject.tag == "tp_combat" && Input.GetButtonDown("Use"))
        {
            StopCoroutine(coroutine);
            //flip a coin for whether a bush has something in it
            //flip a coin for whether that thing is an enemy or an item
            //if enemy
            abs_load(collider);
            //if item
            //.. add item to inventory
        }
    }

    void abs_load(Collider2D collider)
    {
        string[] tmp = collider.name.Split(':');
        oOb_comp.loadScene(tmp[0],tmp[1],tmp[2]); //area, scene, entrypoint
    }

    IEnumerator TravelTimeout(Collider2D collider)
    {
        yield return new WaitForSeconds(4.0f); //armadillo run dissapear
        abs_load(collider);
    }

    IEnumerator FightTimeout(Collider2D collider)
    {
        //flip a coin to determin if the bush has anything in it
        //if there is anything in it, rng timeout
        yield return new WaitForSeconds(4.0f);
        abs_load(collider);
    }

    void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "tp_armadillo" || collider.gameObject.tag == "tp_combat")
        {
            StopCoroutine(coroutine);
        }
        if (collider.gameObject.tag == "button")
        {
            collider.gameObject.GetComponent<BS>().n--;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "MW")
        {
            oob.GetComponent<oOb>().mw++;
            Destroy(collider.gameObject);
        }

        if (collider.gameObject.tag == "tp_armadillo")
        {
            coroutine = TravelTimeout(collider);
            StartCoroutine(coroutine);
        }

        if (collider.gameObject.tag == "tp_edge")
        {
            abs_load(collider);
        }

        if (collider.gameObject.tag == "tp_combat")
        {
            coroutine = FightTimeout(collider);
            StartCoroutine(coroutine);
        }

        if (collider.gameObject.tag == "button")
        {
            collider.gameObject.GetComponent<BS>().n++;
        }
    }

}
