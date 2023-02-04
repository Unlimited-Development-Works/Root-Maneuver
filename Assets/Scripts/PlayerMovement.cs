using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public string playerName;

    public float moveSpeed;
    private float boostValue = 3;
    public Rigidbody2D rb;
    private float roundedX;
    private float roundedY;

    public float rotationSpeed;

    private Vector2 moveDirection;
    private Vector2 rotateDirection;

    private ChainedSounds sounds;

    public RootPlacementController rootController;

    public bool isRetracting = false;

    public bool isBoosting = false;
    private float boostStart;
    private float boostDuration = 1;
    private float boostCoolDown = 5;
    private float boostEnd = -5;

    void Start() {
        sounds = GetComponent<ChainedSounds>();
    }

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        
        // Check public values and set defaults
        if (rotationSpeed == 0)
        {
            rotationSpeed = 5;
        }

        if (moveSpeed == 0)
        {
            moveSpeed = 5;
        }


        if (Input.GetButtonDown(playerName + "_Fire1"))
        {
            isRetracting = true;
        }

        if (!isRetracting)
        {
            float moveX = Input.GetAxisRaw(playerName + "_Horizontal");
            roundedX = (int)Math.Round(moveX, 0);

            float moveY = Input.GetAxisRaw(playerName + "_Vertical");
            roundedY = (int)Math.Round(moveY, 0);

            // trigger boost
            if (Input.GetButtonDown(playerName + "_Fire2"))
            {
                float boostCooldownLeft = Time.time - boostEnd;
                if (boostCoolDown < boostCooldownLeft)
                {
                    isBoosting = true;
                    boostStart = Time.time;
                }

            }
        }
        else
        {
            // sounds?.PlayFor(1f);
            /*            // Do retracting here
                        if (rootController.RetractRoots())
                        {
                            isRetracting = false;
                        }
                        */
            rb.velocity = Vector2.zero;
            isRetracting = rootController.RetractRoots();
            isBoosting = false;
        }

        moveDirection = new Vector2(roundedX, roundedY).normalized;

        // Using minus rotation because the sprite is facing down so everything needs to be reversed 
        rotateDirection = new Vector2(-roundedX, -roundedY).normalized;

        // Used to rotate the sprite to face where it is going
        if (moveDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, rotateDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * 300f * Time.deltaTime);
        }

    }

    void Move()
    {
        if (isBoosting)
        {
            rb.velocity = moveDirection * moveSpeed * boostValue;
            float boostLength = Time.time - boostStart;
            if (boostLength > boostDuration)
            {
                isBoosting = false;
                boostEnd = Time.time;
            }
        } 
        else
        {
            rb.velocity = moveDirection * moveSpeed;
        }
        

       
    }
}
