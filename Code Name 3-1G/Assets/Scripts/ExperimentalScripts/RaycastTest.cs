using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.GetChild(1).GetComponent<Collider2D>().enabled = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(1).transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
            Debug.Log(hit.collider.name);
            transform.GetChild(1).GetComponent<Collider2D>().enabled = true;
        } 
    }
}
