using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyFPSController : MonoBehaviour {
    float playerHeight = 2f;

    [Header("Movement")]
    public float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;
    [SerializeField] Transform orientation;

    [Header("Jumping")]
    public float jumpForce = 15f;
    [SerializeField] int jumps;
    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;


    float horizontalMovement;
    float verticalMovement;
    [SerializeField]
    bool isGrounded;
    public GameObject groundCheck;
    public LayerMask groundMask;

    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector3 slopeMoveDirection;
    Rigidbody rb;

    RaycastHit slopeHit;
    [SerializeField] bool onSlope;

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, (playerHeight * 0.5f) + 0.5f)) {
            if (slopeHit.normal != Vector3.up) {
                return true;
            }
            else {
                return false;
            }

        }
        return false;
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, 0.15f, groundMask);

        MyInput();
        ControlDrag();

        if (isGrounded) {
            jumps = 2;
        }

        if (Input.GetKeyDown(jumpKey) && jumps > 0) {
            Jump();
        }
        onSlope = OnSlope();
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

    }

    void MyInput() {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.transform.forward * verticalMovement + orientation.transform.right * horizontalMovement;
    }

    void Jump() {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        jumps--;
    }

    void ControlDrag() {
        if (isGrounded) {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    void MovePlayer() {
        if (isGrounded && !OnSlope()) {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope()) {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);

        }
        else if (!isGrounded) {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

}
