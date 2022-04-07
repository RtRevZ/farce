using UnityEngine;
using System.Collections;

// The velocity along the y axis is 10 units per second.  If the GameObject starts at (0,0,0) then
// it will reach (0,100,0) units after 10 seconds.

public class CONTROL : MonoBehaviour
{
    public GameObject leader;
    private Rigidbody2D rb;
    private bool jump = false;
    private int j = 1;


    void Start()
    {
        rb = leader.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        jump = Input.GetButtonDown("Jump");

        rb = leader.GetComponent<Rigidbody2D>();

        Vector3 movement = rb.velocity;


        if (jump && j != 0)
        {
            movement[1] = 7.0f;
            j--;
        }

        if (movement[1] == 0.0f)
        {
            j = 1;
        }

        movement[0] = 2.5f * Input.GetAxis("Horizontal");


        if (movement[0] < 0)
        {
            leader.transform.localScale = new Vector3(-1,1,1);
            leader.transform.Find("Main Camera").transform.localScale = new Vector3(1,1,1);
        }

        if (movement[0] >  0)
        {
            leader.transform.localScale = new Vector3(1, 1, 1);
        }

        rb.velocity = movement;

    }
}

