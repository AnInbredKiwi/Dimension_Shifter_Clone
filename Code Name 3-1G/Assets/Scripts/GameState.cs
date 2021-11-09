using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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
    Camera cam;
    public float zPosition2D;
    public float spaceBetweenRaycasts;
    GameObject player;
    Transform player2D;
    GameObject[] environment;

    public float camYOffset2D = 3f;

    public float cam2DSize = 7f;

    public float ground2dRaycastDistance = 1.5f;

    void Awake()
    {
        currentState = GameStates.ThreeD;
        cam = FindObjectOfType<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        player2D = player.transform.GetChild(1);
        environment = GameObject.FindGameObjectsWithTag("Environment");
        Debug.Log(environment);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
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
            cam.transform.position = player2D.position + new Vector3(0, camYOffset2D, zPosition2D);
        }
    }

    IEnumerator Transition()
    {
        Debug.Log("Entering transition from state " + previousState);

        cam.orthographic = !cam.orthographic;
        cam.GetComponent<CinemachineBrain>().enabled = !cam.GetComponent<CinemachineBrain>().enabled;

        if (previousState == GameStates.ThreeD)
        {
            player.transform.GetChild(0).GetComponent<ThirdPersonMovement>().ReleaseCube();
            cam.transform.position = player.transform.position + new Vector3(0, 4, -zPosition2D);
            cam.transform.rotation = Quaternion.Euler(0, 0, 0);
            cam.orthographicSize = cam2DSize;
        }
        else if (previousState == GameStates.TwoD)
        {

        }

        SwitchDimension(player);
        foreach (GameObject item in environment)
        {
            if (previousState == GameStates.ThreeD)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(item.transform.GetChild(0).position.x, item.transform.GetChild(0).position.y, zPosition2D), item.transform.position - new Vector3(item.transform.GetChild(0).position.x, item.transform.GetChild(0).position.y, zPosition2D).normalized, out hit)) ;
                {
                    item.SetActive(false);
                }
            }
            Debug.Log(item.name);
            SwitchDimension(item);
        }

        if (previousState == GameStates.ThreeD)
            ChangeState(GameStates.TwoD);

        else if (previousState == GameStates.TwoD)
            ChangeState(GameStates.ThreeD);


        yield return null;
    }



    void ChangeState(GameStates newState)
    {
        previousState = currentState;
        currentState = newState;
        Debug.Log("Switched Game State to " + currentState);
    }

    void SwitchDimension(GameObject parentObject)
    {

        parentObject.SetActive(true);
        if (previousState == GameStates.ThreeD)
        {
            parentObject.transform.position = parentObject.transform.GetChild(0).transform.position;
            parentObject.transform.GetChild(0).transform.localPosition = Vector3.zero;
            parentObject.transform.GetChild(0).gameObject.SetActive(false);
            parentObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (previousState == GameStates.TwoD)
        {
            if (parentObject.transform.GetChild(1).gameObject.tag == "Movable" || parentObject.tag == "Player")
            {
                RaycastHit2D hit = Physics2D.Raycast(parentObject.transform.GetChild(1).transform.position, Vector2.down, ground2dRaycastDistance, LayerMask.GetMask("Ground"));
                if (hit != false && hit.collider.tag == "Movable") 
                {
                    Debug.Log($"Moving {parentObject} to {hit}'s z");
                    float zToMove = hit.collider.transform.parent.transform.position.z;
                    parentObject.transform.GetChild(1).transform.position = new Vector3(parentObject.transform.GetChild(1).transform.position.x, parentObject.transform.GetChild(1).transform.position.y, zToMove);
                }
            }

            parentObject.transform.position = parentObject.transform.GetChild(1).transform.position;
            parentObject.transform.GetChild(1).transform.localPosition = Vector3.zero;
            parentObject.transform.GetChild(1).gameObject.SetActive(false);
            parentObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // I was gonna use an enum but there are just 2 states, a bool should do LOL
    //    public static bool state3D;

    //    Camera cam;
    //    GameObject player;
    //    GameObject[] Environment;
    //    float[] CubesZ;

    //    public static Transform currentGroundCube;

    //    public float cam2DSize = 7f;


    //    void Awake()
    //    {
    //        state3D = true;
    //        cam = FindObjectOfType<Camera>();
    //        player = GameObject.FindGameObjectWithTag("Player");
    //        Environment = GameObject.FindGameObjectsWithTag("Environment");
    //        CubesZ = new float[Environment.Length];


    //    }

    //    void Update()
    //    {

    //        if (Input.GetKeyDown(KeyCode.Q))
    //        {
    //            state3D = !state3D;
    //            cam.orthographic = !cam.orthographic;
    //            cam.GetComponent<CinemachineBrain>().enabled = !cam.GetComponent<CinemachineBrain>().enabled;
    //            //player.transform.GetChild(0).GetComponent<Collider>().enabled = !player.transform.GetChild(0).GetComponent<Collider>().enabled;

    //            Rigidbody rb;

    //            if (state3D)
    //            {
    //                for (int cubeIndex = 0; cubeIndex < Environment.Length; cubeIndex++)
    //                {
    //                    Environment[cubeIndex].transform.position = new Vector3(Environment[cubeIndex].transform.position.x, Environment[cubeIndex].transform.position.y, CubesZ[cubeIndex]);
    //                    rb = Environment[cubeIndex].GetComponent<Rigidbody>();
    //                    if (rb != null)
    //                        rb.isKinematic = false;
    //                    if (currentGroundCube != null)
    //                    {
    //                        Debug.Log("Moving to cube " + currentGroundCube);
    //                        player.GetComponent<CharacterController>().enabled = false;
    //                        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, currentGroundCube.position.z);

    //                        player.GetComponent<CharacterController>().enabled = true;
    //                    }
    //                }
    //            }

    //            if (!state3D)
    //            {
    //                player.GetComponent<ThirdPersonMovement>().ReleaseCube();
    //                cam.transform.position = player.transform.position + new Vector3(0, 4, -300);
    //                cam.transform.rotation = Quaternion.Euler(0, 0, 0);
    //                cam.orthographicSize = cam2DSize;
    //                for (int cubeIndex = 0; cubeIndex < Environment.Length; cubeIndex++)
    //                {
    //                    CubesZ[cubeIndex] = Environment[cubeIndex].transform.position.z;
    //                    rb = Environment[cubeIndex].GetComponent<Rigidbody>();
    //                    if (rb != null)
    //                        rb.isKinematic = true;
    //                    Debug.Log(CubesZ[cubeIndex]);
    //                    Environment[cubeIndex].transform.position = new Vector3(Environment[cubeIndex].transform.position.x, Environment[cubeIndex].transform.position.y, player.transform.position.z);
    //                }
    //            }
    //        }

    //        if (!state3D)
    //        {
    //            cam.transform.position = player.transform.position + new Vector3(0, 4, -300);
    //        }
    //    }
}