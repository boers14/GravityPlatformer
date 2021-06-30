using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbebleObject : MonoBehaviour
{
    public enum InteractState
    {
        Grab,
        Push
    }

    public InteractState interactState = InteractState.Grab;

    public UnityEvent pushEvent = null;

    private Vector3 startPos = Vector3.zero;

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "DeathBlock")
        {
            transform.position = startPos;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
