using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float addedMovementSpeed = 0.5f, slowDownSpeed = 0.99f, maxMovementSpeed = 5, jumpSpeed = 5;

    private float currentMoveSpeed = 0, rayDist = 0;

    private bool canJump = false;

    public bool setRotationConstraint { get; set; } = true;

    private new Rigidbody2D rigidbody = null;

    [System.NonSerialized]
    private Vector3 rotationConstraint = Vector3.zero;

    [SerializeField]
    private LayerMask layerMask = 0;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rayDist = GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.02f;
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

        canJump = GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            velocity.y += jumpSpeed;
        }

        rigidbody.velocity = velocity;

        if (setRotationConstraint)
        {
            transform.eulerAngles = rotationConstraint;
        }
    }

    private bool GroundCheck()
    {
        if (Physics2D.Raycast(transform.position, -transform.up, rayDist, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FlipJumpSpeed()
    {
        jumpSpeed *= -1;
        if (rotationConstraint == Vector3.zero)
        {
            rotationConstraint = new Vector3(0, 0, 180);
        } else
        {
            rotationConstraint = Vector3.zero;
        }
    }
}
