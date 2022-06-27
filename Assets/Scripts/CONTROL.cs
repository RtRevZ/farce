using UnityEngine;
using System.Collections;

// The velocity along the y axis is 10 units per second.  If the GameObject starts at (0,0,0) then
// it will reach (0,100,0) units after 10 seconds.

public class CONTROL : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool jump = false;
    private int j = 1;
    private GameObject oob, party;

    public float speed;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        oob = GameObject.Find("oOb");
        party = GameObject.Find("Party");


        if (rb.velocity.y != 0f) j = 0;
    }

    IEnumerator JumpSpike()
    {
        yield return new WaitForSeconds(1.2f);
        if(rb.velocity.y != 0)
        {
            Vector3 tmp = rb.velocity;
            tmp.y = -7.5f;
            rb.velocity = tmp;
        }
        else j = 1;

    }

    void Update()
    {
        jump = Input.GetButtonDown("Jump");

        Vector3 movement = rb.velocity;

        if (jump && j != 0)
        {
            movement[1] = 15f;
            StartCoroutine(JumpSpike());
            j--;
        }

        if (movement[1] == 0.0f && j != 1)
        {
            j = 1;
        }

        movement[0] = speed * (j == 1 ? 1f : .95f) * Input.GetAxis("Horizontal") * (1 + (j == 1 ? (1.15f * Input.GetAxis("Sprint")): 0f));




        if (movement[0] < 0)
        {
            gameObject.transform.localScale = new Vector3(-1,1,1);
            gameObject.transform.Find("Main Camera").transform.localScale = new Vector3(1,1,1);
        }

        if (movement[0] >  0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        rb.velocity = movement;

    }

}

