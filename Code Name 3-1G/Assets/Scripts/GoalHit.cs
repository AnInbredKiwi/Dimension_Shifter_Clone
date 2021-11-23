using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collision)
    {
        GameObject UI = GameObject.Find("Canvas");
        if (collision.name == "Player3D")
        {
            UI.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject UI = GameObject.Find("Canvas");
        if (collision.name == "Player2D")
        {
            UI.transform.GetChild(0).gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
