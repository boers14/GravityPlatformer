using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private Transform player = null;

    private float distanceFromPlayer = 0, downWardDistance = 0;

    [SerializeField]
    private float lerpFactor = 2;

    private bool isPickedUp = false;

    [SerializeField]
    private Door connectedDoor = null;

    private BoxCollider2D boxCollider = null;

    private void Start()
    {
        distanceFromPlayer = (GetComponent<SpriteRenderer>().size.x / 2) + 0.05f;
        downWardDistance = player.GetComponent<SpriteRenderer>().size.y / 2 - 0.25f;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (isPickedUp)
        {
            if (!boxCollider.isTrigger)
            {
                boxCollider.isTrigger = true;
                GetComponent<Rigidbody2D>().gravityScale = 0;
            }

            transform.position = Vector2.Lerp(transform.position, player.position - (player.right * distanceFromPlayer) - 
                (player.up * downWardDistance), lerpFactor);
            transform.rotation = player.rotation;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && !isPickedUp)
        {
            connectedDoor.isOpen = true;
            isPickedUp = true;
            boxCollider.isTrigger = true;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            tag = "UndetectedByGrapple";
            gameObject.layer = 3;
        }
    }
}
