using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;
    bool isLadder;
    bool isClimbing;
	public float speed = 8;
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isLadder && Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            isClimbing = true;
        }

    }

    private void FixedUpdate()
    {
        if(isClimbing)
        {
            playerRb.gravityScale = 0;
            playerRb.velocity = new Vector2(playerRb.velocity.x, Input.GetAxis("Vertical") * speed); //8 is speed
        }
        else
        {
            playerRb.gravityScale = 3; //initial value
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
        else
        {
            isLadder = false;
            isClimbing = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
        }
    }
}
