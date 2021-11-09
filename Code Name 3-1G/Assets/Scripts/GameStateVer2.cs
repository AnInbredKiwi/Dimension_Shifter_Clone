using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameStateVer2 : MonoBehaviour
{
    // I was gonna use an enum but there are just 2 states, a bool should do LOL
    public static bool state3D;
    [SerializeField] GameObject cube2DPrefab;

    [SerializeField] Camera cam3D;
    [SerializeField] Camera cam2D;

    GameObject player;
    GameObject[] Cubes;
    Vector3[] CubesPositionIn3D;

    public static Transform currentGroundCube;

    void Awake()
    {
        state3D = true;
        cam3D = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        Cubes = GameObject.FindGameObjectsWithTag("Cube");
        CubesPositionIn3D = new Vector3[Cubes.Length];
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            state3D = !state3D;

            cam3D.enabled = state3D;
            cam2D.enabled = !state3D;

            if (state3D)
            {
                SwitchTo3D();
            }

            if (!state3D)
            {
                SwitchTo2D();
            }
        }

    }

    void SwitchTo2D()
    {
        player.GetComponent<ThirdPersonMovement>().ReleaseCube();
        for (int cubeIndex = 0; cubeIndex < Cubes.Length; cubeIndex++)
        {
            //var col = Cubes[cubeIndex].GetComponent<BoxCollider>(); 
            //if (col)
            //    col.enabled = false;    //disabling collider so they will not bother in 2D ///// TODO enable on switch

            var rb = Cubes[cubeIndex].GetComponent<Rigidbody>(); //TODO remove?
            if (rb != null)
                rb.isKinematic = true;
        }
    } 
    
    void SwitchTo3D()
    {
        for (int cubeIndex = 0; cubeIndex < Cubes.Length; cubeIndex++)
        {
           var rb = Cubes[cubeIndex].GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = false;
            if (currentGroundCube != null)
            {
                Debug.Log("Moving to cube " + currentGroundCube);
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, currentGroundCube.position.z);

                player.GetComponent<CharacterController>().enabled = true;
            }
        }
    }
}