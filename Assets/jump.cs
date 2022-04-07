using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    private int j = 1;
    public int follower = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.name == "JC")
        {
            Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

            Vector3 movement = rb.velocity;

            if (j != 0)
            {
                movement[1] = 7.0f;
                if(follower == 1)
                {
                    Destroy(collider.gameObject);
                }
                j--;
            }

            if (movement[1] == 0.0f)
            {
                j = 1;
            }

            rb.velocity = movement;
        }
    }
}
