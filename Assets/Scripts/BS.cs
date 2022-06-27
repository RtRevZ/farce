using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BS : MonoBehaviour
{

    public bool act_st, neg, state, button;

    public int n = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = false;
    }

    public void Toggle()    //switches toggle on interaction
    {
        state = !state;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * -1, 1, 1);
    }

    void Update()
    {
        if (button)
        {
            if(n == 0)
            {
                state = false;
            }
            else
            {
                state = true;
            }
        }
    }

}
