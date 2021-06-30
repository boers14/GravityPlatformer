using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [System.NonSerialized]
    public GrappleCannon grappleCannon = null;

    private Transform grabbedObject = null;

    private bool returning = false;

    private void FixedUpdate()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == grappleCannon.gameObject && GetComponentInParent<Rope>().canDeactivate)
        {
            if (GetComponentInParent<Rope>().prevRope != null)
            {
                GetComponentInParent<Rope>().prevRope.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        if (!returning && collision.gameObject != grappleCannon.gameObject && collision.tag != "UndetectedByGrapple") {
            returning = true;
            grappleCannon.MoveGrappleBack();
            if (collision.GetComponent<GrabbebleObject>() != null)
            {
                if (collision.GetComponent<GrabbebleObject>().interactState == GrabbebleObject.InteractState.Grab)
                {
                    grabbedObject = collision.transform;
                    grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                } else if (collision.GetComponent<GrabbebleObject>().interactState == GrabbebleObject.InteractState.Push)
                {
                    collision.GetComponent<GrabbebleObject>().pushEvent.Invoke();
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            grabbedObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * 250);
        }
    }
}
