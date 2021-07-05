using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float addedMovementSpeed = 0.5f, slowDownSpeed = 0.99f, maxMovementSpeed = 5, jumpSpeed = 5;

    private float currentMoveSpeed = 0, verticalRayDist = 0, horizontalRayDist = 0;

    private bool canJump = false, isWalking = false, isJumping = false;

    public bool setRotationConstraint { get; set; } = true;

    private new Rigidbody2D rigidbody = null;

    [System.NonSerialized]
    private Vector3 rotationConstraint = Vector3.zero;

    [SerializeField]
    private LayerMask layerMask = 0;

    private GravitySwitch gravitySwitch = null;

    private Animator animator = null;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        verticalRayDist = GetComponent<BoxCollider2D>().bounds.size.y / 2 + 0.02f;
        horizontalRayDist = GetComponent<BoxCollider2D>().bounds.size.x / 2 + 0.02f;
        gravitySwitch = GetComponent<GravitySwitch>();
    }

    private void Update()
    {
        Vector3 velocity = rigidbody.velocity;
        Vector3 rotation = transform.eulerAngles;

        if (Input.GetKey(KeyCode.RightArrow) && currentMoveSpeed < maxMovementSpeed)
        {
            currentMoveSpeed += addedMovementSpeed;
            if (gravitySwitch.reverseGravity)
            {
                rotation.y = 180;
            } else
            {
                rotation.y = 0;
            }

            if (currentMoveSpeed > maxMovementSpeed)
            {
                currentMoveSpeed = maxMovementSpeed;
            }

            SetWalkAnimation();
        } else if (Input.GetKey(KeyCode.LeftArrow) && currentMoveSpeed > -maxMovementSpeed && !Input.GetKey(KeyCode.RightArrow))
        {
            currentMoveSpeed -= addedMovementSpeed;
            if (gravitySwitch.reverseGravity)
            {
                rotation.y = 0;
            }
            else
            {
                rotation.y = 180;
            }

            if (currentMoveSpeed < maxMovementSpeed)
            {
                currentMoveSpeed = -maxMovementSpeed;
            }

            SetWalkAnimation();
        } else
        {
            currentMoveSpeed *= slowDownSpeed;

            if (isWalking && currentMoveSpeed > -1f && currentMoveSpeed < 1f)
            {
                isWalking = false;
                animator.SetBool("isWalking", isWalking);
            }

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

        if (canJump)
        {
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool("IsJumping", isJumping);
            }
        } else
        {
            if (!isJumping)
            {
                isJumping = true;
                animator.SetBool("IsJumping", isJumping);
            }
        }

        bool cantMoveRight = WallCheck();
        if (cantMoveRight)
        {
            velocity.x = 0;
        }

        rigidbody.velocity = velocity;
        transform.eulerAngles = rotation;

        if (setRotationConstraint)
        {
            rotation = transform.eulerAngles;
            rotation.z = rotationConstraint.z;
            transform.eulerAngles = rotation;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void SetWalkAnimation()
    {
        if (!isWalking)
        {
            isWalking = true;
            animator.SetBool("isWalking", isWalking);
        }
    }

    private bool GroundCheck()
    {
        if (Physics2D.Raycast(transform.position, -transform.up - (transform.right * horizontalRayDist), verticalRayDist + 0.03f, layerMask) ||
            Physics2D.Raycast(transform.position, -transform.up + (transform.right * horizontalRayDist), verticalRayDist + 0.03f, layerMask) ||
            Physics2D.Raycast(transform.position, -transform.up, verticalRayDist, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WallCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, horizontalRayDist, layerMask);

        if (hit)
        {
            if (hit.transform.GetComponent<Rigidbody2D>() != null)
            {
                Rigidbody2D rigidbody = hit.transform.GetComponent<Rigidbody2D>();
                if (rigidbody.constraints == RigidbodyConstraints2D.None)
                {
                    return false;
                } else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
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
