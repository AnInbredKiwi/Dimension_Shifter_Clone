using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // I was gonna use an enum but there are just 2 states, a bool should do LOL
    public static bool state3D;

    Camera cam;
    Transform player;

    public float cam2DSize = 7f;

    void Awake()
    {
        state3D = true;
        cam = FindObjectOfType<Camera>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            state3D = !state3D;
            cam.orthographic = !cam.orthographic;
            cam.GetComponent<CinemachineBrain>().enabled = !cam.GetComponent<CinemachineBrain>().enabled;

            if (!state3D)
            {
                cam.transform.position = player.position + new Vector3(0, 4, -300);
                cam.transform.rotation = Quaternion.Euler(0, 0, 0);
                cam.orthographicSize = cam2DSize;
            }
        }

        if (!state3D)
        {
            cam.transform.position = player.position + new Vector3(0, 4, -300);
        }
    }
}