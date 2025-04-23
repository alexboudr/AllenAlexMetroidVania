using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour //, IHitable
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
    private bool canJump = true;
    private bool canDoubleJump = true;
    private bool pickedUpDoubleJump = false;

    //private bool hasJumped //this is for double jumping

    //dash
    // Dash parameters
    public float dashSpeed = 10f;
    public float dashTime = 0.2f;
    private bool isDashing = false;
    Vector3 Drag = new Vector3(1, 1, 1);
    public float dashCooldown = 0.5f;
    private float dashTimer = 0f;
    private bool canDash = true;
    //private bool dashCancel = true;
    private bool pickedupDash = false;
    //private bool hasDashedOnce = false;

    //sound effect stuff
    private AudioSource audioSource; //audio source...
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip dashRegenSound;



    private void Jump(int define)
    {
        
        //"three" definitions
        // 0 = regular jump
        // 1 = double jump
        // 2 = jump during dash
        if (define == 0)
        {
            Debug.Log("Jump!");

            audioSource.PlayOneShot(jumpSound, 1F);

            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            canDash = true;
        }
        else if (define == 1)
        {
            Debug.Log("Double Jump!");

            audioSource.PlayOneShot(jumpSound, 0.7F);

            playerVelocity.y = Mathf.Sqrt(jumpHeight * 0.75f * -2.0f * gravityValue);
            canDoubleJump = false;
        }
        else if (define == 2)
        {

           // if (canJump)//stupid fucking variable I put to hopefully help in some fucking stupid way
            //{
                playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * 0.75f * gravityValue); //apply jump force

                //maintain dash momentum in the jump
                playerVelocity.x = transform.forward.x * dashSpeed;
                playerVelocity.z = transform.forward.z * dashSpeed;
            //canDoubleJump = true;
            //hasDashedOnce = true;
            //}

            
        }
        else
        {
            Debug.Log("error on jumpng?");
        }
    }




    private IEnumerator Dash(bool wasGroundedBeforeDash)
    {
        isDashing = true;

        if (wasGroundedBeforeDash)
        {
            canJump = true; // <-- Add this line back if it was removed
        }
        else
        {
            //canJump = false;
        }

        //if (!groundedPlayer)
        //{
        //    canJump = false;
        //}
        float startTime = Time.time;
        float originalY = transform.position.y; // Store Y position when the dash starts
        bool jumpedDuringDash = false; // Track if we jumped mid-dash

        //play sound!
        audioSource.PlayOneShot(dashSound, 1F);

        while (Time.time < startTime + dashTime)
        {
            // Move forward while keeping the Y position locked (if still grounded)
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);

            if (groundedPlayer) //this helps with dashing on slopes specifically
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Debug.Log("slope dash");
            }
            else if (!jumpedDuringDash) // Only lock Y if we haven't jumped during a dash
            {
                transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
            }

            if (Input.GetButtonDown("Jump") && (groundedPlayer || canJump || canDoubleJump))
            //have we dash jumped already?
            {
                //if (groundedPlayer || (pickedUpDoubleJump && canDoubleJump)) // Only allow double jump if they have the ability
                //{
                jumpedDuringDash = true; //track that we jumped
                Debug.Log("ok,,,,,,, so I should be able to jump now");
                //canJump = true;

                if (canJump) //ok SO, if they HAVEN"T jumped, run jump2
                {
                    Debug.Log("YAHAHAHAHAHA");
                    Jump(2);
                    canJump = false;
                }
                else if (canDoubleJump)
                {
                    Debug.Log("uyooyoyoyoyoyoyo");
                    Jump(2);
                    canDoubleJump = false;
                }
                
                
                

                    //playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * 0.75f * gravityValue); // Apply jump force

                    //// Maintain dash momentum in the jump
                    //playerVelocity.x = transform.forward.x * dashSpeed;
                    //playerVelocity.z = transform.forward.z * dashSpeed;
                    //canDoubleJump = true;
                    //hasDashedOnce = true;

                //if (!groundedPlayer) // If we used the double jump, disable it
                //    {
                //        canDoubleJump = false;
                //    }

                break; // Exit the dash
                //}
            }

            //stop dash if we hit the ground
            if (groundedPlayer && !wasGroundedBeforeDash)
            {
                break;
            }

                

            yield return null;
        }

        isDashing = false;

        //it's own gravity, or else it will keep on increasing
        if (playerVelocity.y < 0)
        {
            playerVelocity.y = Mathf.Max(playerVelocity.y, -2f); 
        }

        //set dashing to false
        canDash = false;
        //start dash timer
        dashTimer = dashCooldown;

    }


    //this is activated in the powerup thing
    public void PickedupDoubleJump()
    {
        pickedUpDoubleJump = true;
    }

    //anotehr powerup THINGY
    public void PickedupDash()
    {
        pickedupDash = true;
    }


    void Start()
    {
        float height = GetComponent<CharacterController>().height;
        Debug.Log("Character height: " + height);

        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        //determines if I'm hitting the floor
        Debug.Log(groundedPlayer);
        RaycastHit hit;
        Vector3 sphereOrigin = transform.position + Vector3.down * (controller.height / 2f - controller.radius);
        float sphereRadius = controller.radius - 0.05f;  // Slightly smaller than the CharacterController
        float sphereCastLength = 0.3f;  // Small enough to detect ground but not false positives
        bool wasGrounded = groundedPlayer;
        //float slopeAngleToTransfer = 0; //im lazy lol

        if (Physics.SphereCast(sphereOrigin, sphereRadius, Vector3.down, out hit, sphereCastLength))
        {
            //we need this so that you can't jump on slops
            //if it detects it's on ground, then check if it's a slope
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle < 50)
            {
                canJump = true;
                groundedPlayer = true;
            }


            
            Debug.Log("so this should reset canJump");
            //groundedPlayer = true;
            //hasDashedOnce = false;

            // Only reset canDoubleJump if we actually have the power-up
            if (pickedUpDoubleJump)
            {
                canDoubleJump = true;
            }
            

            

            // Debug.Log("Ground detected at: " + hit.point);
        }
        else
        {
            //Debug.Log("No ground detected.");
            groundedPlayer = false;


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
           if (groundedPlayer)//normal - this prevents from spammign double jump 
            {
                Jump(0);
                //Debug.Log("Jump!");

                //audioSource.PlayOneShot(jumpSound, 0.7F);

                //playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
                //canDash = true;

                //if (isDashing)
                //{
                //    isDashing = false; 
                //    dashTimer = 0; 
                //}
           }
           else if (canDoubleJump && pickedUpDoubleJump) //double
           {
                Jump(1);
                //Debug.Log("Double Jump!");

                //audioSource.PlayOneShot(jumpSound, 0.7F);

                //playerVelocity.y = Mathf.Sqrt(jumpHeight * 0.75f * -2.0f * gravityValue);
                //canDoubleJump = false;
           }
        }
        //if (Input.GetButtonDown("Jump"))
        //{
        //    if (groundedPlayer || isSlopeJump || isDashing) // Allow jumping from dashing state
        //    {
        //        Debug.Log("Jump!");

        //        //play sound!
        //        audioSource.PlayOneShot(jumpSound, 0.7F);

        //        Debug.Log("can double jump: " + canDoubleJump + "picked up value: " + pickedUpDoubleJump);
        //        if (canDoubleJump && pickedUpDoubleJump)
        //        {
        //            playerVelocity.y = Mathf.Sqrt(jumpHeight * 0.75f * -2.0f * gravityValue);

        //            canDoubleJump = false;
        //        }
        //        else
        //        {
        //            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        //            canDash = true;
        //            if (isDashing)
        //            {
        //                isDashing = false; // End dash on jump
        //                dashTimer = 0; // Reset cooldown immediately
        //            }
        //        }
        //    }
        //}


        controller.Move(playerVelocity * Time.deltaTime);

        //dash timer that counts down
        if (!canDash)
        {
            dashTimer -= Time.deltaTime;
            
            if (dashTimer <= 0)
            {
                audioSource.PlayOneShot(dashRegenSound, 0.5F);
                canDash = true; // Reset dash after cooldown
            }
        }

        // Dash input
        if (Input.GetButtonDown("Dash") && !isDashing && canDash && pickedupDash)
        {

            Debug.Log("Dash");
            StartCoroutine(Dash(wasGrounded));
            
        }

        //this applies gravity to the player UNLESS they are dashing (locks y)
        if (!groundedPlayer && !isDashing)
        {
            Debug.Log("THIS IS GETTING ACTIVATED");
            playerVelocity.x /= 1 + Drag.x * Time.deltaTime * 0.5f; // Reduce momentum gradually
            playerVelocity.z /= 1 + Drag.z * Time.deltaTime * 0.5f;
        }

        if (groundedPlayer && !wasGrounded) //reset momentum when you land
        {
            Debug.Log("Landing! Reset momentum");
            playerVelocity.x = 0f;
            playerVelocity.z = 0f;

            //reset dash on landing (or else the sound will play ANY time you land, for hte if statement)
            if (!canDash)
            {
                Debug.Log("Resetting dash on landing");
                dashTimer = 0f;
                canDash = true;
                audioSource.PlayOneShot(dashRegenSound, 0.5f);
            }

        }




    }


    //void OnCollisionEnter(Collision other)
    //{
    //    //Debug.Log("Oops I crashed!");

    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        Debug.Log("Oops I crashed into an enemy!");
    //    }
    //}

    public void Knockback()
    {
        transform.position -= transform.forward * Time.deltaTime * 100;
    }
}


