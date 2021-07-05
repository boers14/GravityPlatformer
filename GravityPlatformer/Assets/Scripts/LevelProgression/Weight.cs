using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    [SerializeField]
    private float ySpeedNecessary = 2;

    private new Rigidbody2D rigidbody = null;

    private float prevSpeed = 0;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        prevSpeed = rigidbody.velocity.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7) {
            if (prevSpeed <= -ySpeedNecessary || prevSpeed >= ySpeedNecessary)
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
