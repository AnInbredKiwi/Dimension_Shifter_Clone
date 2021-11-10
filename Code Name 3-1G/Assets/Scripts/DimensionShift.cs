using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionShift : MonoBehaviour
{
    public void SwitchZ()
    {
        // Transform Z position of player and movable objects

        if (transform.GetChild(1).gameObject.tag == "Movable" || tag == "Player")
        {
            Debug.Log($"Shiftin {transform.GetChild(1).name} to 3D, castung raycast");
            transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(1).transform.position, Vector2.down, GameState.ground2dRaycastDistance, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.GetChild(1).transform.position, Vector2.down, Color.red, 60, true);

            //   Debug.Log($"Ray casted from {transform.GetChild(1).name}, results: {hit.collider.name}");
            transform.GetChild(1).GetComponent<Collider2D>().enabled = true;
            if (hit != false /*&& hit.collider.tag == "Movable"*/)
            {
                Debug.Log($"Moving {gameObject} to {hit.collider.name}'s z");
                float zToMove = hit.collider.transform.parent.transform.position.z;
                transform.GetChild(1).transform.position = new Vector3(transform.GetChild(1).transform.position.x, transform.GetChild(1).transform.position.y, zToMove);
            }
        }

    }

    public void SwitchDimension()
    {

        gameObject.SetActive(true);

        if (GameState.previousState == GameState.GameStates.ThreeD)
        {
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
