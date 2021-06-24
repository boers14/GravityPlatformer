using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float addedMovementSpeed = 0.5f, slowDownSpeed = 0.99f, maxMovementSpeed = 5, jumpSpeed = 5;

    private float currentMoveSpeed = 0;

    private bool canJump = false;

    private new Rigidbody2D rigidbody = null;

    [System.NonSerialized]
    private Vector3 rotationConstraint = Vector3.zero;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 velocity = rigidbody.velocity;

        if (Input.GetKey(KeyCode.RightArrow) && currentMoveSpeed < maxMovementSpeed)
        {
            currentMoveSpeed += addedMovementSpeed;

            if (currentMoveSpeed > maxMovementSpeed)
            {
                currentMoveSpeed = maxMovementSpeed;
            }
        } else if (Input.GetKey(KeyCode.LeftArrow) && currentMoveSpeed > -maxMovementSpeed)
        {
            currentMoveSpeed -= addedMovementSpeed;

            if (currentMoveSpeed < maxMovementSpeed)
            {
                currentMoveSpeed = -maxMovementSpeed;
            }
        } else
        {
            currentMoveSpeed *= slowDownSpeed;

            if (currentMoveSpeed > -0.01f && currentMoveSpeed < 0.01f)
            {
                currentMoveSpeed = 0;
            }
        }

        velocity.x = currentMoveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            velocity.y += jumpSpeed;
        }

        rigidbody.velocity = velocity;
        transform.eulerAngles = rotationConstraint;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            canJump = false;
        }
    }
}
