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
    private void OnCollisionEnter(Collision collision)
    {
        GameObject UI = GameObject.Find("Canvas");
        if (collision.collider.name == "Player3D")
        {
            UI.transform.GetChild(0).gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();            
            }
            else
            {
                UI.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject UI = GameObject.Find("Canvas");
        if (collision.collider.name == "Player2D")
        {
            UI.transform.GetChild(0).gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else
            {
                UI.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
