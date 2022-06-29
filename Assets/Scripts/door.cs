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
    // Start is called before the first frame update
    void Start()
    {

        string str = "";

        if (BS.Length > 0)
        {
            locked = true;
        }

        ops = new bool[BS.Length];
        neg = new bool[BS.Length];
        vals = new int[BS.Length];

        do
        {
            for (int i = 0; i < ops.Length - 1; i++)
            {
                vals[i] = Random.Range(0, BS.Length);
                neg[i] = Random.Range(0f, 1f) < .5f ? false : true;
                ops[i] = Random.Range(0f, 1f) < .5f ? false : true; //f = and; t = or
            }

            if (BS.Length != 0)
            {
                vals[BS.Length - 1] = Random.Range(0, BS.Length);
                neg[BS.Length - 1] = Random.Range(0f, 1f) < .5f ? false : true;
                ops[BS.Length - 1] = false;
            }

            str = "";

            for (int i = 0; i < vals.Length; i++)
            {
                if (BS[vals[i]].GetComponent<BS>().state ^ neg[i])
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

        } while (DFA(str)); // prevent doors spawning unlocked
    }

    private bool DFA(string str) //returns TRUE iif DFA HALTS and string is ACCEPTED
    {

        int state = 0, nextst = 0;

        foreach (char c in str)
        {
            if (state == 0)
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
                if (c == '0' || c == '1' || c == '+')
                {
                    nextst = 2;
                }
            }

            state = nextst;
        }

        if (state == 2 || state == 0)
        {
            return true;
        }
        else return false;


    }

    // Update is called once per frame
    void Update()
    {
        string str = "", str2 = "";

        for (int i = 0; i < vals.Length; i++)
        {
            if (BS[vals[i]].GetComponent<BS>().state ^ neg[i])
            {
                str += "1";
            }
            else
            {
                str += "0";
            }

            str2 += vals[i];
            if (neg[i]) str2 += "'";

            if (ops[i])
            {
                str += "+";
                str2 += "+";
            }
        }

        Debug.Log(str + " " + str2);

        locked = !DFA(str);
    }
}
