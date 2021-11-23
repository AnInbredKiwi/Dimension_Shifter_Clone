using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float runSpeed = 40f;
    public float pushDistance = 1.5f;
    public float pushingSpeed = 0.5f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    Rigidbody2D pushingCube;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
    }


    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump") && pushingCube == null)
            jump = true;

        if (Input.GetButtonDown("Crouch"))
            crouch = true;
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }

        PushInput();

    }
    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        Push();
    }

    void PushInput()
    {
        if (Input.GetKey(KeyCode.Mouse0) && pushingCube == null)
        {
            Debug.Log("Attempting to push cube");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Input.GetAxisRaw("Horizontal"), 0), pushDistance, LayerMask.GetMask("Ground"));
            if (hit != false && hit.collider.gameObject.tag == "Movable")
            {
                pushingCube = hit.collider.gameObject.GetComponent<Rigidbody2D>();
                pushingCube.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                Debug.Log("2D Pushing " + pushingCube.gameObject.name);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && pushingCube != null)
        {
            pushingCube.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            pushingCube.velocity = new Vector2(0, 0);
            pushingCube = null;
        }
    }

    void Push()
    {
        if(pushingCube != null)
        {
            pushingCube.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*pushingSpeed, 0);        
        }   
    }
}
