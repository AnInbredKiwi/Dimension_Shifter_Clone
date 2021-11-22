using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : TriggerMechanism
{
    public KeyCode triggerKey;
    public Vector3 checkBoxSize = new Vector3(0.6f, 0.1f, 0.6f);

    Vector3 ogPosition;
    public float movementDuration = 2f;

    public override void Start()
    {
        base.Start();
        ogPosition = transform.position;
        Debug.Log(new Vector3(0, GetComponentInChildren<MeshCollider>().bounds.size.y + 1));
    }

    void Update()
    {
        if (GameState.currentState == GameState.GameStates.ThreeD)
        {
            if (Physics.CheckBox(transform.position + new Vector3(0, GetComponentInChildren<MeshCollider>().bounds.size.y + checkBoxSize.y + 0.05f, 0), checkBoxSize) && !triggered)
            {
                Trigger();
            }
            else if (!Physics.CheckBox(transform.position + new Vector3(0, GetComponentInChildren<MeshCollider>().bounds.size.y + checkBoxSize.y + 0.05f, 0), checkBoxSize) && triggered)
            {
                Untrigger();
            }
        }

        if (GameState.currentState == GameState.GameStates.TwoD)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position + new Vector3(0, GetComponentInChildren<Collider2D>().bounds.size.y + checkBoxSize.y + 0.05f, 0), checkBoxSize, 0f, Vector2.up, 0f);
            if (hit && !triggered)
            {
                Trigger();
            }
            else if (!hit && triggered)
            {
                Untrigger();
            }
        }
    }

    protected override void Trigger()
    {
        base.Trigger();
        StartCoroutine(MoveDown());
    }

    protected override void Untrigger()
    {
        base.Untrigger();
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveDown()
    {
        Debug.Log("Moving plate down");
        Vector3 targetPosition = ogPosition;
        if (GameState.currentState == GameState.GameStates.ThreeD)
            targetPosition = ogPosition - new Vector3(0, GetComponentInChildren<MeshCollider>().bounds.extents.y, 0);
        else if (GameState.currentState == GameState.GameStates.TwoD)
            targetPosition = ogPosition - new Vector3(0, GetComponentInChildren<Collider2D>().bounds.extents.y, 0);
        else
            yield return null;

        while (transform.position != targetPosition && triggered)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, (ogPosition.y - targetPosition.y) / movementDuration * Time.deltaTime);
            yield return null;
        }

        Debug.Log("Plate moved down");
        yield return null;
    }

    IEnumerator MoveUp()
    {
        Debug.Log("Moving plate up");
        Vector3 pressedPosition = transform.position;
        while (transform.position != ogPosition && !triggered)
        {
            transform.position = Vector3.MoveTowards(transform.position, ogPosition, (ogPosition.y - pressedPosition.y) / movementDuration * Time.deltaTime);
            yield return null;
        }
        Debug.Log("Plate moved up");
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position + new Vector3(0, GetComponentInChildren<MeshCollider>().bounds.size.y + checkBoxSize.y + 0.05f, 0), checkBoxSize * 2);
    }
}
