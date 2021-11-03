using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float wallDistance = 0.1f;
    public float jumpHeight = 4f;

    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isPushing;
    GameObject grabbedObject;
    Transform grabbedObjectsOGParent;

    public float throwForce = 10f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -4f;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // Fix movement and jump bug in 2D
        if (!GameState.state3D)
        {
            vertical = 0f;
            if (Physics.CheckSphere(transform.position + (cam.right * 0.6f) + (Vector3.up * wallDistance), wallDistance, groundMask) && !isGrounded)
                controller.Move(cam.right * -0.02f);
            if (Physics.CheckSphere(transform.position + (cam.right * -0.6f) + (Vector3.up * wallDistance), wallDistance, groundMask) && !isGrounded)
                controller.Move(cam.right * 0.02f);
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }


        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }



        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    Debug.Log("Throwing Raycast");
        //    RaycastHit hit;
        //    if (Physics.Raycast(transform.position + new Vector3(1, 0, 0), transform.forward, out hit, 2f))
        //    {
        //        Debug.Log("Raycast thrown");
        //        if (hit.transform.tag == "Cube")
        //        {
        //            Debug.Log("Launching Cube");
        //            hit.transform.gameObject.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up * 3).normalized * throwForce, ForceMode.VelocityChange);
        //        }
        //    }
        //}

        if (GameState.state3D)
        {
            Push();
            Grab();
        }
    }

    void Push()
    {
        Debug.Log("Pushing? " + isPushing);
        if (Input.GetKeyDown(KeyCode.Mouse0))
            isPushing = true;
        else if (Input.GetKeyUp(KeyCode.Mouse0))
            isPushing = false;
        RaycastHit hit;
        if (isPushing && Physics.Raycast(transform.position /*+ transform.forward*0.8f*/ + Vector3.up, transform.forward, out hit, 1f) && grabbedObject == null)
        {
            if (hit.transform.tag == "Cube")
            {
                Debug.Log("Pushing Cube");
                hit.transform.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed / 2f;
            }
        }
    }

    void Grab()
    {
        bool grabbingFrame = false;

        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse1) && Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 1f) && grabbedObject == null)
        {
            grabbedObject = hit.transform.gameObject;
            grabbedObjectsOGParent = grabbedObject.transform.parent;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.transform.position = transform.position + transform.forward * 2.5f + Vector3.up * 1.5f;
            grabbedObject.transform.SetParent(transform);
            grabbingFrame = true;
        }
        if (grabbedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !grabbingFrame)
            {
                ReleaseCube();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ReleaseCube();
            }
        }
    }

    public void ReleaseCube()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(grabbedObjectsOGParent);
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (Input.GetKeyDown(KeyCode.Mouse0))
                grabbedObject.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up * 3).normalized * throwForce, ForceMode.VelocityChange);
            grabbedObject = null;
        }
    }
}
