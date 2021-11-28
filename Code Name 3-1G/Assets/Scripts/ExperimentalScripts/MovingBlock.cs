using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float movementDuration = 5f;
    public float distanceToStop = 0.05f;
    public TriggerMechanism trigger;
    public Transform startPosition;
    public Transform endPosition;

    Rigidbody rb3d;
    Rigidbody2D rb2d;

    private void Start()
    {
        rb3d = transform.GetChild(0).GetComponent<Rigidbody>();
        rb2d = transform.GetChild(1).GetComponent<Rigidbody2D>();
        startPosition.position = rb3d.position;
    }



    void FixedUpdate()
    {
        //if (trigger.triggered && transform.position != endPosition.position)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, endPosition.position, (startPosition.position - endPosition.position).magnitude / movementDuration * Time.deltaTime);
        //}
        //else if (!trigger.triggered && transform.position != startPosition.position)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, startPosition.position, (startPosition.position - endPosition.position).magnitude / movementDuration * Time.deltaTime);
        //}

        if (GameState.currentState == GameState.GameStates.ThreeD)
        {
            if (trigger.triggered && Vector3.Distance(rb3d.transform.position, endPosition.position) > distanceToStop)
            {
                Debug.Log(Vector3.Distance(transform.position, endPosition.position));
                Debug.Log("Trying to move block");
                rb3d.isKinematic = false;
                Vector3 vector3 = (endPosition.position - startPosition.position) / movementDuration;
                rb3d.velocity = vector3;
                Debug.Log(vector3);
            }
            else if (!trigger.triggered && Vector3.Distance(rb3d.transform.position, startPosition.position) > distanceToStop)
            {
                rb3d.isKinematic = false;
                Debug.Log("Trying to move block back");
                Vector3 vector3 = (startPosition.position - endPosition.position) / movementDuration;
                rb3d.velocity = vector3;
            }
            else
            {
                Debug.Log("Stopp");
                rb3d.velocity = Vector3.zero;
                rb3d.isKinematic = true;
            }
        }

        if (GameState.currentState == GameState.GameStates.TwoD)
        {
            if (trigger.triggered && Vector3.Distance(rb2d.transform.position, endPosition.position) > distanceToStop)
            {
                Debug.Log(Vector3.Distance(transform.position, endPosition.position));
                Debug.Log("Trying to move block");
                rb2d.isKinematic = false;
                Vector3 vector3 = (endPosition.position - startPosition.position) / movementDuration;
                rb2d.velocity = vector3;
                Debug.Log(vector3);
            }
            else if (!trigger.triggered && Vector3.Distance(rb2d.transform.position, startPosition.position) > distanceToStop)
            {
                rb2d.isKinematic = false;
                Debug.Log("Trying to move block back");
                Vector3 vector3 = (startPosition.position - endPosition.position) / movementDuration;
                rb2d.velocity = vector3;
            }
            else
            {
                Debug.Log("Stopp");
                rb2d.velocity = Vector3.zero;
                rb2d.isKinematic = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPosition.position, endPosition.position);
    }
}
