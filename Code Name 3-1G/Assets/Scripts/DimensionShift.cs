using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionShift : MonoBehaviour
{
    Transform standingGroundBeforeShift;

    public void SwitchZ()
    {
        // Transform Z position of player and movable objects
        if (standingGroundBeforeShift != null)
            Debug.Log(gameObject.name + "'s standingGroundBeforeShift Z is " + standingGroundBeforeShift.position.z);
        else
            Debug.LogWarning(gameObject.name + "'s standingGroundBeforeShift is null");

        if (transform.GetChild(1).gameObject.tag == "Movable" || tag == "Player")
        {
            Debug.Log($"Shiftin {transform.GetChild(1).name} to 3D, castung raycast");
            transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(1).transform.position, Vector2.down, GameState.ground2dRaycastDistance, LayerMask.GetMask("Ground"));

               //Debug.Log($"Ray casted from {transform.GetChild(1).name}, results: {hit.collider.name}");
            transform.GetChild(1).GetComponent<Collider2D>().enabled = true;
            if (hit != false && hit.collider.transform.parent != standingGroundBeforeShift)
            {
                float zToMove = hit.collider.transform.position.z;
                transform.GetChild(1).transform.position = new Vector3(transform.GetChild(1).transform.position.x, transform.GetChild(1).transform.position.y, zToMove);
                Debug.Log($"Moving {gameObject} to {hit.collider.transform.parent.name}'s z, namely:  { zToMove}");
                Debug.Log(transform.GetChild(1).transform.position.z);
            }
        }

    }

    public void SwitchDimension()
    {

        gameObject.SetActive(true);

        if (GameState.previousState == GameState.GameStates.ThreeD)
        {
            // Remember the ground object on which it's standing before shifting to 2D
            if (transform.GetChild(0).gameObject.tag == "Movable" || tag == "Player")
            {
                RaycastHit hit;
                transform.GetChild(0).GetComponent<Collider>().enabled = false;
                if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, GameState.ground3dRaycastDistance))
                {
                    standingGroundBeforeShift = hit.collider.transform.parent;
                    Debug.Log($"Assigned {standingGroundBeforeShift} to {gameObject} as standing ground");
                    transform.GetChild(0).transform.position += Vector3.up * 0.05f;
                }
                else
                    standingGroundBeforeShift = null;
                transform.GetChild(0).GetComponent<Collider>().enabled = true;
            }

            transform.position = transform.GetChild(0).transform.position;
            transform.GetChild(0).transform.localPosition = Vector3.zero;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        if (GameState.previousState == GameState.GameStates.TwoD)
        {
            transform.position = transform.GetChild(1).transform.position;
            transform.GetChild(1).transform.localPosition = Vector3.zero;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
        }


    }
}
