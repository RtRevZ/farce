using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using FARCEUtils;

public class oOb : MonoBehaviour
{

    public int leader = 0;

    public FARCE[] party = new FARCE[3];

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        party[0] = new FARCE("Digby Ketton");
        party[1] = new FARCE("Samuel, Son of Goerthe");
        party[2] = new FARCE("Hero the Enchanter");

    }


}