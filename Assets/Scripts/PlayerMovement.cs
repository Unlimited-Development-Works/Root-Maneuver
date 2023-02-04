using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    public float roundedX;
    public float roundedY;

    public float rotationSpeed;

    public string playerName = "player2";

    private Vector2 moveDirection;
    private Vector2 rotateDirection;

    public RootPlacementController rootController;

    public bool isRetracting = false;

    // Update is called once per frame
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

        if (Input.GetKeyDown("space") || Input.GetButtonDown(playerName + "_Fire1"))
        {
            Debug.LogError("space key was pressed");
            isRetracting = true;
        }

        if (!isRetracting)
        {
            float moveX = Input.GetAxisRaw(playerName + "_Horizontal");
            roundedX = (int)Math.Round(moveX, 0);

            float moveY = Input.GetAxisRaw(playerName + "_Vertical");
            roundedY = (int)Math.Round(moveY, 0);
        }
        else
        {
            /*            // Do retracting here
                        if (rootController.RetractRoots())
                        {
                            isRetracting = false;
                        }
                        */
            isRetracting = rootController.RetractRoots();
        }

        moveDirection = new Vector2(roundedX, roundedY).normalized;

        // Using minus rotation because the sprite is facing down so everything needs to be reversed 
        rotateDirection = new Vector2(-roundedX, -roundedY).normalized;

        // Used to rotate the sprite to face where it is going
        if (moveDirection != Vector2.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, rotateDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }

    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }
}
