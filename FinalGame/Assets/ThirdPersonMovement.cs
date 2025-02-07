using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    //jumping
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    private bool groundedPlayer;
    private Vector3 playerVelocity;
    private bool isSlopeJump = false;
    //private bool hasJumped //this is for double jumping

    //dash
    // Dash parameters
    public float dashSpeed = 10f;
    public float dashTime = 0.2f;
    private bool isDashing = false;
    Vector3 Drag = new Vector3(1, 1, 1);


    private IEnumerator Dash(bool wasGroundedBeforeDash)
    {
        isDashing = true;
        float startTime = Time.time;
        float originalY = transform.position.y; // Store Y position when the dash starts
        bool jumpedDuringDash = false; // Track if we jumped mid-dash

        while (Time.time < startTime + dashTime)
        {
            // Move forward while keeping the Y position locked (if still grounded)
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);

            if (!jumpedDuringDash) // Only lock Y if we haven't jumped
                transform.position = new Vector3(transform.position.x, originalY, transform.position.z);

            // Allow jumping from the dash
            if (Input.GetButtonDown("Jump"))
            {
                jumpedDuringDash = true; // Track that we jumped
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue); // Apply jump force

                // Maintain dash momentum in the jump
                playerVelocity.x = transform.forward.x * dashSpeed;
                playerVelocity.z = transform.forward.z * dashSpeed;
                break; // Exit the dash
            }

            // Stop dash if we were falling and hit the ground
            if (groundedPlayer && !wasGroundedBeforeDash)
                break;

            yield return null;
        }

        isDashing = false;

        // If falling, reset downward velocity to a smaller value
        if (playerVelocity.y < 0)
            playerVelocity.y = Mathf.Max(playerVelocity.y, -2f); // Slow down the fall slightly
    }






    void Start()
    {
        float height = GetComponent<CharacterController>().height;
        Debug.Log("Character height: " + height);
    }
    // Update is called once per frame
    void Update()
    {
        //determines if I'm hitting the floor
        RaycastHit hit;
        Vector3 sphereOrigin = transform.position + Vector3.down * (controller.height / 2f - controller.radius);
        float sphereRadius = controller.radius - 0.05f;  // Slightly smaller than the CharacterController
        float sphereCastLength = 0.3f;  // Small enough to detect ground but not false positives
        bool wasGrounded = groundedPlayer;

        if (Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out hit, sphereCastLength))
        {
            groundedPlayer = true;

            //if it detects it's on ground, then check if it's a slope
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

            if (slopeAngle >= 0.1f) // handles any slope
            {
                Debug.Log("Slope detected! Commencing stick");

                if (Input.GetButtonDown("Jump"))
                {
                    Debug.Log("slope jump");
                    //jump from slope
                    playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
                    isSlopeJump = true;
                }
                else
                {
                    // instead of forcing downward velocity, smoothly apply gravity
                    if (playerVelocity.y < 0)
                    {
                        playerVelocity.y += gravityValue * Time.deltaTime * 0.5f; //softer gravity to avoid hard falls
                    }
                    isSlopeJump = false;
                }
            }

            // Debug.Log("Ground detected at: " + hit.point);
        }
        else
        {
            //Debug.Log("No ground detected.");
            groundedPlayer = false;
            isSlopeJump = false;


            if (wasGrounded)
            {
                Debug.Log("Transitioning to Airborne");
                playerVelocity.y = Mathf.Max(playerVelocity.y, -2f); // Slow down the fall slightly
            }
            else
            {
                playerVelocity.y += gravityValue * Time.deltaTime;
            }
        }


        float horizontal = Input.GetAxisRaw("Horizontal"); //-1 for a, + 1 for d
        float vertical = Input.GetAxisRaw("Vertical"); //1 for s, +1 for w
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;



        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; //rotates the character with the camera
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (groundedPlayer || isSlopeJump || isDashing) // Allow jumping from dashing state
            {
                Debug.Log("Jump!");

                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);

                // Preserve existing movement & add dash momentum only if dashing
                if (isDashing)
                {
                    playerVelocity += transform.forward * dashSpeed;
                    isDashing = false; // End dash on jump
                }
            }
        }


        controller.Move(playerVelocity * Time.deltaTime);

        // Dash input
        if (Input.GetButtonDown("Dash") && !isDashing)
        {
            Debug.Log("Dash");
            StartCoroutine(Dash(wasGrounded));
        }

        if (!isDashing)
        {
            playerVelocity.x /= 1 + Drag.x * Time.deltaTime;
            playerVelocity.y /= 1 + Drag.y * Time.deltaTime;
            playerVelocity.z /= 1 + Drag.z * Time.deltaTime;
        }

    }
}


