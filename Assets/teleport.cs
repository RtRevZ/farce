using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "AC" && Input.GetButtonDown("Use"))
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
    }
}
