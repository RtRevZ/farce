using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
