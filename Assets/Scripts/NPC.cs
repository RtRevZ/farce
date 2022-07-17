using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI txt;
    public string[] quotes;

    void Start()
    {
        txt = gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator npcSay()
    {
        txt.text = quotes[UnityEngine.Random.Range(0, quotes.Length)];
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        yield break;
    }
}
