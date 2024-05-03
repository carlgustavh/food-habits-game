using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float rightForce = 500f;
    public float leftForce = -500f;
    public float startPush = -0f;
    public float jumpForce = 15f;
    public bool isGrounded; // Track if the player is grounded

    // Buffer zone to compensate for small gaps
    public float groundBuffer = 0.1f;

    public float crouchScale = 0.5f; // Scale factor for crouching
    public float crouchDuration = 1f; // Duration of crouching in seconds

    public GameObject PlayerModel; // Reference to the player model

    private Vector3 originalColliderScale; // Original scale of the collider
    private Vector3 originalModelScale; // Original scale of the player model

    private bool isCrouching = false; // Flag to track if the player is crouching
    private bool isMovingRight = false;
    private bool isMovingLeft = false;
    private bool isJumping = false;
    private bool isCrouchButtonDown = false;

    void FixedUpdate()
    {
        // Apply forward force
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);

        // Check for right movement input
        if (isMovingRight)
        {
            rb.AddForce(rightForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        // Check for left movement input
        if (isMovingLeft)
        {
            rb.AddForce(leftForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        // Check for jump input (spacebar)
        if (isJumping && isGrounded) // Only allow jumping if grounded
        {
            Jump();
        }

        // Check if the player has fallen below a certain y-position
        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }

        // Check if crouching in mid-air and apply pull-down force
        if (isCrouchButtonDown && !isGrounded && isCrouching)
        {
            rb.AddForce(Vector3.down * 5000f, ForceMode.Force); // Apply downward force
        }

        if (isCrouchButtonDown && !isCrouching)
        {
            StartCoroutine(CrouchCoroutine());
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(0, 0, startPush * Time.deltaTime);

        // Store original scales
        originalColliderScale = transform.localScale;
        originalModelScale = PlayerModel.transform.localScale;
    }
    void Update()
    {
        // Check for right movement input
        if (Input.GetKey("d"))
        {
            SetIsMovingRight(true);
        }
        else
        {
            SetIsMovingRight(false);
        }

        // Check for left movement input
        if (Input.GetKey("a"))
        {
            SetIsMovingLeft(true);
        }
        else
        {
            SetIsMovingLeft(false);
        }

        // Check for jump input (spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetIsJumping(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            SetIsJumping(false);
        }

        // Check for crouch input (Left Control)
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            SetIsCrouchButtonDown(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            SetIsCrouchButtonDown(false);
        }
    }


    // Function to make the player jump
    public void Jump()
    {
        // Check if the player is grounded before jumping
        if (isGrounded)
        {
            // Zero out the y-velocity to ensure consistent jumping
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // Add an upward force to simulate jumping
            rb.AddForce(0, jumpForce, 0, ForceMode.VelocityChange);
        }
    }

    // OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.point.y < transform.position.y - groundBuffer)
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }

    // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    // Method to simulate crouching by scaling down the collider on the y-axis
    public void Crouch()
    {
        Vector3 originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x, originalScale.y * crouchScale, originalScale.z);
    }

    public void SetForwardForce(float modifier)
    {
        forwardForce = 10000 + modifier;
    }

    IEnumerator CrouchCoroutine()
    {
        isCrouching = true;

        // Scale down the collider and player model
        transform.localScale = new Vector3(originalColliderScale.x, originalColliderScale.y * crouchScale, originalColliderScale.z);
        PlayerModel.transform.localScale = new Vector3(originalModelScale.x, originalModelScale.y * crouchScale, originalModelScale.z);

        yield return new WaitForSeconds(crouchDuration);

        // Scale back to normal after the duration
        transform.localScale = originalColliderScale;
        PlayerModel.transform.localScale = originalModelScale;

        isCrouching = false;
    }

    // Update button state methods
    public void SetIsMovingRight(bool value)
    {
        isMovingRight = value;
    }

    public void SetIsMovingLeft(bool value)
    {
        isMovingLeft = value;
    }

    public void SetIsJumping(bool value)
    {
        isJumping = value;
    }

    public void SetIsCrouchButtonDown(bool value)
    {
        isCrouchButtonDown = value;
    }
}