using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool locked = false;
    public GameObject[] BS;

    private bool[] ops;
    private bool[] neg;
    private int[] vals;

    private string last;
    // Start is called before the first frame update
    void Start()
    {
        if (BS.Length > 0)
        {
            locked = true;
        }

        ops = new bool[BS.Length];
        neg = new bool[BS.Length];
        vals = new int[BS.Length];

        for (int i = 0; i < ops.Length - 1; i++)
        {
            ops[i] = Random.Range(0f, 1f) < .5f ? false : true; //f = and; t = or
            neg[i] = Random.Range(0f, 1f) < .5f ? false : true;
            vals[i] = Random.Range(0, BS.Length);
        }

        if (BS.Length != 0)
        {
            ops[BS.Length - 1] = false;
            neg[BS.Length - 1] = Random.Range(0f, 1f) < .5f ? false : true;
            vals[BS.Length - 1] = Random.Range(0, BS.Length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        string str = "", str2 = "";

        for(int i = 0; i < vals.Length; i++)
        {
            if (BS[vals[i]].GetComponent<BS>().state ^ neg[i])
            {
                str += "1";
            }
            else
            {
                str += "0";
            }

            str2 += vals[i].ToString();
            if (neg[i]) str2 += "'";

            if (ops[i])
            {
                str += "+";
                str2 += "+";
            }
        }

        if (str != last)
        {
            Debug.Log(str2 + " " + str);
        }

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

        last = str;
    }
}
