using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject thisButton;
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        thisButton = gameObject;
        Button btn = thisButton.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        string level1 = "LevelConcept1";
        string level2 = "LevelConcept2";
        string level3 = "LevelConcept3";
        string level4 = "LevelConcept4";

        if (thisButton.name == level1 + "Button")
        {
            Debug.Log("Loading Scene: " + level1);
            SceneManager.LoadScene(level1);
        }
        else if(thisButton.name == level2 + "Button")
        {
            Debug.Log("Loading Scene: " + level2);
            SceneManager.LoadScene(level2);
        }
        else if(thisButton.name == level3 + "Button")
        {
            Debug.Log("Loading Scene: " + level3);
            SceneManager.LoadScene(level3);
        }
        else if (thisButton.name == level4 + "Button")
        {
            Debug.Log("Loading Scene: " + level3);
            SceneManager.LoadScene(level4);
        }
        else if (thisButton.name == "QuitButton")
        {
            Debug.Log("Quitting game");
            Application.Quit();
        }
            
    }
}
