using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // I was gonna use an enum but there are just 2 states, a bool should do LOL
    public static bool state3D;

    Camera cam;
    GameObject player;
    GameObject[] Cubes;
    float[] CubesZ;

    public static Transform currentGroundCube;

    public float cam2DSize = 7f;


    void Awake()
    {
        state3D = true;
        cam = FindObjectOfType<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
        Cubes = GameObject.FindGameObjectsWithTag("Cube");
        CubesZ = new float[Cubes.Length];
        

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            state3D = !state3D;
            cam.orthographic = !cam.orthographic;
            cam.GetComponent<CinemachineBrain>().enabled = !cam.GetComponent<CinemachineBrain>().enabled;
            player.transform.GetChild(0).GetComponent<Collider>().enabled = !player.transform.GetChild(0).GetComponent<Collider>().enabled;

            Rigidbody rb;

            if(state3D)
            {
                for (int cubeIndex = 0; cubeIndex < Cubes.Length; cubeIndex++)
                {
                    Cubes[cubeIndex].transform.position = new Vector3(Cubes[cubeIndex].transform.position.x, Cubes[cubeIndex].transform.position.y, CubesZ[cubeIndex]);
                    rb = Cubes[cubeIndex].GetComponent<Rigidbody>();
                    rb.isKinematic = false;     
                    if(currentGroundCube != null)
                    {
                        player.GetComponent<CharacterController>().enabled = false;
                        player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, currentGroundCube.position.z);

                        player.GetComponent<CharacterController>().enabled = true;
                    }
                }
            }  

            if (!state3D)
            {
                cam.transform.position = player.transform.position + new Vector3(0, 4, -300);
                cam.transform.rotation = Quaternion.Euler(0, 0, 0);
                cam.orthographicSize = cam2DSize;
                for (int cubeIndex = 0; cubeIndex < Cubes.Length; cubeIndex++)
                {
                    CubesZ[cubeIndex] = Cubes[cubeIndex].transform.position.z;
                    rb = Cubes[cubeIndex].GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                    Debug.Log(CubesZ[cubeIndex]);
                    Cubes[cubeIndex].transform.position = new Vector3(Cubes[cubeIndex].transform.position.x, Cubes[cubeIndex].transform.position.y, player.transform.position.z);
                }
            }
        }

        if (!state3D)
        {          
            cam.transform.position = player.transform.position + new Vector3(0, 4, -300);
        }
    }
}