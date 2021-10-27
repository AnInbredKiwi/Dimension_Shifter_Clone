using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void Awake()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Cube")
        {
            GameState.currentGroundCube = collision.transform;
            Debug.Log("Standing on " + GameState.currentGroundCube);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
            GameState.currentGroundCube = null;
    }

}
