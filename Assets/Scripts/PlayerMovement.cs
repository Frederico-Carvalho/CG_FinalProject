using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.SceneView;

public class PlayerMovement : MonoBehaviour
{
    // Input variables
    float hozizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    public Transform mesh;
    Rigidbody rb;

    // Movement speed
    public float Speed;

    // Orientation and Camera
    public Transform orientation;
    public Transform Cam;

    public float groundDrag;

    //Ground Check
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;


    // Start is called before the first frame update
    void Start()
    {
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Lock the cursor to the center of the screen and makes it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void MyInput()
    {
        // Get input from keyboard or controller
        hozizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void MovePlayer()
    {
        // Calculate movement direction relative to camera orientation
        Vector3 camForward = Cam.forward;
        Vector3 camRight = Cam.right;

        // Flatten the vectors to the horizontal plane 
        camForward.y = 0f;
        camRight.y = 0f;

        // Normalize the vectors 
        camForward.Normalize();
        camRight.Normalize();

        // Calculate move direction based on input and camera orientation
        moveDirection = camForward * verticalInput + camRight * hozizontalInput;

        // Apply movement to the Rigidbody
        rb.linearVelocity = new Vector3(
            moveDirection.x * Speed,
            rb.linearVelocity.y,
            moveDirection.z * Speed
        );
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Limit velocity if needed
        if (flatVel.magnitude > Speed)
        {
            Vector3 limitedVel = flatVel.normalized * Speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void FaceMovementDir()
    {
        // Flatten the movement direction to the horizontal plane
        Vector3 flatMove = new Vector3(moveDirection.x, 0, moveDirection.z);

         // Calculate target rotation based on movement direction
        Quaternion targetRotation = Quaternion.LookRotation(flatMove);
        mesh.rotation = Quaternion.Slerp(mesh.rotation, targetRotation, 10f * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        //ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        SpeedControl();

        //Handle Drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

    }
    private void FixedUpdate()
    {
        MovePlayer();
        FaceMovementDir();
    }

}