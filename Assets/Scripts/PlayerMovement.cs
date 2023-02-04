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


    private Vector2 moveDirection;
    private Vector2 rotateDirection;


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


        float moveX = Input.GetAxisRaw("Horizontal");
        roundedX = (int)Math.Round(moveX, 0);

        float moveY = Input.GetAxisRaw("Vertical");
        roundedY = (int)Math.Round(moveY, 0);

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
