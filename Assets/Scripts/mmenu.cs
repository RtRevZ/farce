using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class mmenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void fiatLudum()
    {
        SceneManager.LoadScene("Areas/CONTROL/genesis", LoadSceneMode.Single);
    }
}
