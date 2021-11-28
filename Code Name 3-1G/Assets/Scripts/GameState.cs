using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public enum GameStates
    {
        ThreeD,
        TwoD,
        Transition
    }
    public static GameStates currentState;
    public static GameStates previousState;
    [SerializeField] Camera cam3D;
    [SerializeField] Camera cam2D; //assign in the inspector
    [SerializeField] GameObject cinemachineBrain;
    public float zPosition2D;
    public float spaceBetweenRaycasts;
    public GameObject player;
    Transform player2D;
    GameObject[] environment;
    // DimensionShift[] dimensionShifters;
    public List<DimensionShift> dimensionShifters = new List<DimensionShift>();
    bool gamePaused = false;
    public GameObject PausedMenu;


    public float camYOffset2D = 3f;

    public float cam2DSize = 7f;

    public float ground2dRaycastDistanceEditor = 3f, ground3dRaycastDistanceEditor = 4f;
    public static float ground2dRaycastDistance = 3f, ground3dRaycastDistance = 4f;

    void Start()
    {
        //Pause Menu things
        PausedMenu = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        Button Button = PausedMenu.transform.GetChild(1).gameObject.GetComponent<Button>();
        Button.onClick.AddListener(ReturnMenu);

        // Assign helper field values to their corresponidng static fields
        ground2dRaycastDistance = ground2dRaycastDistanceEditor;
        ground3dRaycastDistance = ground3dRaycastDistanceEditor;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentState = GameStates.ThreeD;
        cam3D = Camera.main;
        player2D = player.transform.GetChild(1);
        player2D.gameObject.SetActive(false);
        environment = GameObject.FindGameObjectsWithTag("Environment");
        // dimensionShifters = GameObject.FindObjectsOfType<DimensionShift>();
        dimensionShifters.AddRange(FindObjectsOfType<DimensionShift>());
        //Debug.Log(environment);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ChangeState(GameStates.Transition);
            if (currentState == GameStates.Transition && previousState != GameStates.Transition)
                StartCoroutine("Transition");
            else
                Debug.Log("Bruh, where Transition State?");
        }

        // Camera follow player in 2D
        if (currentState == GameStates.TwoD)
        {
            cam2D.transform.position = player2D.position + new Vector3(0, camYOffset2D, zPosition2D);
        }

        //Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !(gamePaused);
            PauseGame();
        }
    }

    IEnumerator Transition()
    {
        Debug.Log("Entering transition from state " + previousState);

        //Transition Animation
        if (previousState == GameStates.ThreeD)
        {
            cinemachineBrain.SetActive(false);
            Vector3 camOgPos = cam3D.transform.position;
            float targetY = player.transform.GetChild(0).transform.position.y + camYOffset2D;

            cam3D.transform.GetChild(0).gameObject.SetActive(true);
            while (cam3D.transform.position.y != targetY)
            {
                cam3D.transform.position = Vector3.MoveTowards(cam3D.transform.position, new Vector3(cam3D.transform.position.x, targetY, cam3D.transform.position.z), ((camOgPos.y - targetY) /0.25f) * Time.deltaTime);
                cam3D.transform.LookAt(player.transform.GetChild(0));
                Debug.Log("Transition Animation phase 1");
                yield return null; 
            }
            cam3D.transform.GetChild(0).gameObject.SetActive(true);
            for (float i = 0; i < 0.5; i = i + Time.deltaTime)
            {
                cam3D.transform.RotateAround(player.transform.GetChild(0).transform.position, Vector3.up, 720 * Time.deltaTime);
                yield return null;
            }
            while (cam3D.transform.rotation.eulerAngles.y < 5 && cam3D.transform.rotation.eulerAngles.y > -5)
            {
                cam3D.transform.RotateAround(player.transform.GetChild(0).transform.position, Vector3.up, 360 * Time.deltaTime);
                yield return null;
            }
            cam3D.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (previousState == GameStates.TwoD)
        {
            cam2D.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            cinemachineBrain.SetActive(true);
            cam2D.transform.GetChild(0).gameObject.SetActive(false);
        }
        // Animations end here


        cam3D.enabled = !cam3D.enabled;
        cam2D.enabled = !cam2D.enabled;
        cam3D.gameObject.GetComponent<AudioListener>().enabled = !cam3D.gameObject.GetComponent<AudioListener>().enabled;
        cam2D.gameObject.GetComponent<AudioListener>().enabled = !cam2D.gameObject.GetComponent<AudioListener>().enabled;

        if (previousState == GameStates.ThreeD)
        {
            player.transform.GetChild(0).GetComponent<ThirdPersonMovement>().ReleaseCube();
        }

        else if (previousState == GameStates.TwoD)
        {
            // reaorders the shifters list by y position
            dimensionShifters = dimensionShifters.OrderBy(item => item.transform.position.y).ToList();

            foreach (DimensionShift item in dimensionShifters)
                print(item.name + "has y of " + item.transform.GetChild(1).position.y);


            foreach (DimensionShift item in dimensionShifters)
                item.SwitchZ();

        }

        foreach (DimensionShift item in dimensionShifters)
        {
            //if (previousState == GameStates.ThreeD)
            //{
            //RaycastHit hit;
            //if (Physics.Raycast(new Vector3(item.transform.GetChild(0).position.x, item.transform.GetChild(0).position.y, zPosition2D), item.transform.position - new Vector3(item.transform.GetChild(0).position.x, item.transform.GetChild(0).position.y, zPosition2D).normalized, out hit)) 
            //{
            //    item.SetActive(false);
            //}
            //}

            item.SwitchDimension();
        }

        if (previousState == GameStates.ThreeD)
            ChangeState(GameStates.TwoD);

        else if (previousState == GameStates.TwoD)
            ChangeState(GameStates.ThreeD);


        yield return null;
    }

    void PauseGame()
    {
        Debug.Log("Game paused: " + gamePaused);
        Time.timeScale = gamePaused ? 0 : 1;
        Cursor.visible = gamePaused ? true : false;
        Cursor.lockState = gamePaused ? CursorLockMode.Confined : CursorLockMode.Locked;
        PausedMenu.SetActive(gamePaused ? true : false);
        Debug.Log(PausedMenu.name);
    }

    void ReturnMenu()
    {
        Debug.Log("Returning to Main menu");
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    void ChangeState(GameStates newState)
    {
        previousState = currentState;
        currentState = newState;
        Debug.Log("Switched Game State to " + currentState);
    }

    int SortByY(Transform t1, Transform t2)
    {
        return t1.position.y.CompareTo(t2.position.y);
    }


    //void SwitchDimension(GameObject parentObject)
    //{

    //    parentObject.SetActive(true);

    //    if (previousState == GameStates.ThreeD)
    //    {
    //        parentObject.transform.position = parentObject.transform.GetChild(0).transform.position;
    //        parentObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
    //        parentObject.transform.GetChild(0).gameObject.SetActive(false);
    //        parentObject.transform.GetChild(1).gameObject.SetActive(true);
    //    }
    //    if (previousState == GameStates.TwoD)
    //    {

    //        // Transform Z position of player and movable objects

    //        if (parentObject.transform.GetChild(1).gameObject.tag == "Movable" || parentObject.tag == "Player")
    //        {
    //            Debug.Log($"Shiftin {parentObject.transform.GetChild(1).name} to 3D, castung raycast");
    //            parentObject.transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
    //            RaycastHit2D hit = Physics2D.Raycast(parentObject.transform.GetChild(1).transform.position, Vector2.down, ground2dRaycastDistance, LayerMask.GetMask("Ground"));
    //            Debug.Log($"Ray casted from {parentObject.transform.GetChild(1).name}, results: {hit.collider.name}");
    //            parentObject.transform.GetChild(1).GetComponent<Collider2D>().enabled = true;
    //            if (hit != false /*&& hit.collider.tag == "Movable"*/)
    //            {
    //                Debug.Log($"Moving {parentObject} to {hit.collider.name}'s z");
    //                float zToMove = hit.collider.transform.parent.transform.position.z;
    //                parentObject.transform.GetChild(1).transform.position = new Vector3(parentObject.transform.GetChild(1).transform.position.x, parentObject.transform.GetChild(1).transform.position.y, zToMove);
    //            }
    //        }

    //        //

    //        parentObject.transform.position = parentObject.transform.GetChild(1).transform.position;
    //        parentObject.transform.GetChild(1).transform.localPosition = Vector3.zero;
    //        parentObject.transform.GetChild(1).gameObject.SetActive(false);
    //        parentObject.transform.GetChild(0).gameObject.SetActive(true);
    //    }
    //}

}