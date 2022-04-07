using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject leader, mFollower, rFollower;

    private Rigidbody2D mrb, rrb;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mrb = mFollower.GetComponent<Rigidbody2D>();
        rrb = rFollower.GetComponent<Rigidbody2D>();

        Vector3 mmovement = mrb.velocity;
        Vector3 rmovement = rrb.velocity;



        if (leader.transform.position.x > mFollower.transform.position.x)
        {
            if (Vector3.Distance(leader.transform.position, mFollower.transform.position) > 3)
            {
                mmovement.x = 2.5f;
            }
            mFollower.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (Vector3.Distance(leader.transform.position, mFollower.transform.position) > 3)
            {
                mmovement.x = -2.5f;
            }
            mFollower.transform.localScale = new Vector3(-1, 1, 1);
        }

        mrb.velocity = mmovement;



        if (leader.transform.position.x > rFollower.transform.position.x)
        {
            if (Vector3.Distance(leader.transform.position, rFollower.transform.position) > 5)
            {
                rmovement.x = 2.5f;
            }
            rFollower.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (Vector3.Distance(leader.transform.position, rFollower.transform.position) > 5)
            {
                rmovement.x = -2.5f;
            }
            rFollower.transform.localScale = new Vector3(-1, 1, 1);
        }

        rrb.velocity = rmovement;
    }
}
