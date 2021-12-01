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
        if (thisButton.name == "QuitButton")
        {
            Debug.Log("Quitting game");
            Application.Quit();
        }
        else
        {
            Debug.Log("Loading: " + thisButton.name);
            SceneManager.LoadScene(thisButton.name);
        }
    }
}
