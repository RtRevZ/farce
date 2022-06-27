using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed, dist;
    public GameObject leader;

    private Rigidbody2D rb;
    private bool jump = false;
    public bool stay = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        if (rb.velocity.y != 0f) jump = true;
    }

    IEnumerator JumpSpike()
    {
        yield return new WaitForSeconds(1.2f);
        if (rb.velocity.y != 0)
        {
            Vector3 tmp = rb.velocity;
            tmp.y = -7.5f;
            rb.velocity = tmp;
        }
        else jump = false;

    }


    // Update is called once per frame
    void Update()
    {
        if (!stay)
        {
            Vector3 movement = rb.velocity;

            if (movement.y == 0f && jump)
            {
                jump = false;
            }

            if (Vector3.Distance(leader.transform.position, gameObject.transform.position) > dist)
            {
                if (leader.transform.position.x >= gameObject.transform.position.x + .1f)
                {
                    movement.x = speed;
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (leader.transform.position.x <= gameObject.transform.position.x - .1f)
                {
                    movement.x = -1 * speed;
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    if (!jump && leader.transform.position.y >= gameObject.transform.position.y)
                    {
                        movement.y = 15f;
                        StartCoroutine(JumpSpike());
                        jump = true;
                    }

                    movement.x = 0f;
                }
            }


            rb.velocity = movement;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "button")
        {
            collider.gameObject.GetComponent<BS>().n++;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "button")
        {
            collider.gameObject.GetComponent<BS>().n--;
        }
    }
}
