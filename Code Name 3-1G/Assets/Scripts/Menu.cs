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
        string level2 = "LevelConceptB1";
        string level3 = "LevelConceptB2";
        string level4 = "LevelConcept2";
        string level5 = "LevelConcept3";
        string level6 = "LevelConcept4";
        string level7 = "LevelConcept5";

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
            Debug.Log("Loading Scene: " + level4);
            SceneManager.LoadScene(level4);
        }
        else if (thisButton.name == level5 + "Button")
        {
            Debug.Log("Loading Scene: " + level5);
            SceneManager.LoadScene(level5);
        }
        else if (thisButton.name == level6 + "Button")
        {
            Debug.Log("Loading Scene: " + level6);
            SceneManager.LoadScene(level6);
        }
        else if (thisButton.name == level7 + "Button")
        {
            Debug.Log("Loading Scene: " + level7);
            SceneManager.LoadScene(level7);
        }
        else if (thisButton.name == "QuitButton")
        {
            Debug.Log("Quitting game");
            Application.Quit();
        }
            
    }
}
