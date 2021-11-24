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
    public float grabDistance = 1.5f;

    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool isPushing;
    GameObject grabbedObject;
    Transform grabbedObjectsOGParent;
    //Material[] Mats;
    //public Material[] Hologram = new Material[2];

    public float throwForce = 10f;

    private void Start()
    {
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (GameState.currentState == GameState.GameStates.ThreeD)
        {

            isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
            if (isGrounded && velocity.y <= 0)
                velocity.y = -2f;

            velocity.y += gravity * Time.deltaTime;


            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }


            float horizontal = Input.GetAxisRaw("Horizontal"); //for mouse movement on X axis
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move((moveDir.normalized * speed * Time.deltaTime) + (new Vector3(0, velocity.y, 0) * Time.deltaTime));
            }
            else
            {
                controller.Move(velocity * Time.deltaTime);
            }


            Push();
            Grab();

        }
    }

    void Push()
    {
        //Debug.Log("Pushing? " + isPushing);
        if (Input.GetKeyDown(KeyCode.Mouse0))
            isPushing = true;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            isPushing = false;
        RaycastHit hit;
        if (isPushing && Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 1f) && grabbedObject == null)
        {
            if (hit.transform.tag == "Movable")
            {
                //Debug.Log("Pushing Movable Object");
                hit.transform.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed / 2f;
            }
        }
    }

    void Grab()
    {
        bool grabbingFrame = false;

        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse1) && Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, grabDistance) && grabbedObject == null)
        {
            if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
            {
                grabbedObject = hit.transform.gameObject;
                grabbedObjectsOGParent = grabbedObject.transform.parent;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                grabbedObject.transform.position = transform.position + transform.forward * 1.5f + Vector3.up * 1.5f;
                grabbedObject.transform.SetParent(transform);
                grabbedObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //Mats = grabbedObject.GetComponent<MeshRenderer>().materials;
                //grabbedObject.GetComponent<MeshRenderer>().materials = Hologram;
                grabbingFrame = true;
            }
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
            //grabbedObject.GetComponent<MeshRenderer>().materials = Mats;
            grabbedObject.transform.localScale = new Vector3(1, 1, 1);
            grabbedObject.transform.SetParent(grabbedObjectsOGParent);
            grabbedObject.transform.SetSiblingIndex(0);
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (Input.GetKeyDown(KeyCode.Mouse0))
                grabbedObject.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up * 3).normalized * throwForce, ForceMode.VelocityChange);
            grabbedObject = null;
        }
    }
}