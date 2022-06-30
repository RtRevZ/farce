using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bigboard : MonoBehaviour
{

    public TextMeshPro txt;
    private int lns, max = 13;

    public void set(string str)
    {
        txt.text = str;
    }

    public void reset()
    {
        set(""); 
        lns = 0;
    }

    public void add_line(string str)
    {
        if(lns == max)
        {
            reset();
        }

        set(txt.text + str + "<br>");

        lns++;
    }

    public void Awake() 
    {
        txt = gameObject.GetComponent<TextMeshPro>();
        reset();
    }


}
