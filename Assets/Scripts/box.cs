using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    private int click = 0;
    private GameObject oob;
    private oOb oob_comp;
    public int type = 0; // v/h flag for vertical or horizontal box. 0 - lift pull and push; 1 - pull and push; 2 - lift
    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        oob = GameObject.Find("oOb");
        oob_comp = oob.GetComponent<oOb>();
    }

    // Update is called once per frame
    void Update()
    {
        if (click != 0)
        {
            if(click == 1)
            {
                float vx = oob_comp.PARTY.GetComponent<Party>().party[oob_comp.PARTY.GetComponent<Party>().leader].GetComponent<Rigidbody2D>().velocity.x;
                float x1 = oob_comp.PARTY.GetComponent<Party>().party[oob_comp.PARTY.GetComponent<Party>().leader].transform.position.x;
                float x2 = gameObject.transform.position.x;

                if((x1 < x2 && x1 + vx - x2 < x1 - x2) || (x1 > x2 && x1 + vx - x2 > x1 - x2))
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(vx, 0f, 0f);
            }
            if (click == 2)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 5 * Input.GetAxis("Mouse Y"), 0f);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "button")
        {
            collider.gameObject.GetComponent<BS>().n--;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "button")
        {
            collider.gameObject.GetComponent<BS>().n++;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (first)
        {
            first = false;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (type != 2 && col.transform == oob_comp.PARTY.GetComponent<Party>().party[oob_comp.PARTY.GetComponent<Party>().leader].transform && oob_comp.party[oob_comp.PARTY.GetComponent<Party>().leader].boxact == 0)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionX & RigidbodyConstraints2D.FreezeAll;
        }
    }

    void OnMouseDown()
    {
        if (oob_comp.party[oob_comp.PARTY.GetComponent<Party>().leader].boxact == 1 && Vector3.Distance(oob_comp.PARTY.GetComponent<Party>().party[oob_comp.PARTY.GetComponent<Party>().leader].transform.position, gameObject.transform.position) > 3f && Vector3.Distance(oob_comp.PARTY.GetComponent<Party>().party[oob_comp.PARTY.GetComponent<Party>().leader].transform.position, gameObject.transform.position) <= 10f)
        {
            if(type == 0 || type == 1)
            {
                click = 1;
                gameObject.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionX & RigidbodyConstraints2D.FreezeAll;
            }
        }

        if (oob_comp.party[oob_comp.PARTY.GetComponent<Party>().leader].boxact == 2)
        {
            if (type == 0 || type == 2)
            {
                click = 2;
                gameObject.GetComponent<Rigidbody2D>().constraints = ~RigidbodyConstraints2D.FreezePositionY & RigidbodyConstraints2D.FreezeAll;
            }
        }
        
    }

    void OnMouseUp()
    {

        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        click = 0;
    }


    void OnCollisionExit2D(Collision2D col)
    {
        if (type != 2 && col.transform == oob_comp.PARTY.GetComponent<Party>().party[oob_comp.PARTY.GetComponent<Party>().leader].transform && oob_comp.party[oob_comp.PARTY.GetComponent<Party>().leader].boxact == 0)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

}
