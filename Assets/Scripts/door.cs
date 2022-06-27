using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool locked = false;
    public GameObject[] BS;

    private bool[] ops;

    // Start is called before the first frame update
    void Start()
    {
        if (BS.Length > 0)
        {
            locked = true;
        }

        ops = new bool[BS.Length];

        for(int i = 0; i < ops.Length - 1; i++)
        {
            ops[i] = Random.Range(0f, 1f) < .5f ? false : true; //f = and; t = or
        }

        if (ops.Length > 0) ops[ops.Length - 1] = false;
    }

    // Update is called once per frame
    void Update()
    {
        string str = "";

        for(int i = 0; i < BS.Length; i++)
        {
            if (BS[i].GetComponent<BS>().state)
            {
                str += "1";
            }
            else
            {
                str += "0";
            }

            if (ops[i])
            {
                str += "+";
            }
        }

        //Debug.Log(str);

        int state = 0, nextst = 0;

        foreach (char c in str)
        {
            if(state == 0)
            {
                if (c == '0')
                {
                    nextst = 1;
                }
                if (c == '1')
                {
                    nextst = 0;
                }
                if (c == '+')
                {
                    nextst = 2;
                }
            }
            if (state == 1) 
            {
                if (c == '0' || c == '1')
                {
                    nextst = 1;
                }
                if (c == '+')
                {
                    nextst = 0;
                }
            }
            if (state == 2)
            {
                if (c == '0'|| c == '1' || c == '+')
                {
                    nextst = 2;
                }
            }

            state = nextst;
        }

        if (state == 2 || state == 0)
        {
            locked = false;
        }
        else locked = true;
    }
}
