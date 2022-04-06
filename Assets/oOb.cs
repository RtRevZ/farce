using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class oOb : MonoBehaviour
{

    public int leader = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }


}