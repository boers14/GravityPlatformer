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
                GetComponentInParent<Rope>().prevRope.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        if (!returning && collision.gameObject != grappleCannon.gameObject && collision.tag != "UndetectedByGrapple") {
            returning = true;
            grappleCannon.MoveGrappleBack();
            if (collision.GetComponent<GrabbebleObject>() != null)
            {
                if (collision.GetComponent<GrabbebleObject>().grappleInteractEvent != null)
                {
                    collision.GetComponent<GrabbebleObject>().grappleInteractEvent.Invoke();
                }

                if (collision.GetComponent<GrabbebleObject>().interactState == GrabbebleObject.InteractState.Grab)
                {
                    grabbedObject = collision.transform;
                    grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (grabbedObject != null)
        {
            if (grabbedObject.GetComponent<GrabbebleObject>().endOfGrabEvent != null)
            {
                grabbedObject.GetComponent<GrabbebleObject>().endOfGrabEvent.Invoke();
            }

            if (Input.GetKey(KeyCode.X))
            {
                grabbedObject.GetComponent<BoxCollider2D>().isTrigger = true;
                grabbedObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

                grappleCannon.rigidbodyOfCarriedObject = grabbedObject.GetComponent<Rigidbody2D>();
                grappleCannon.extraCarryDist = grabbedObject.GetComponent<SpriteRenderer>().size.x / 2;
                grappleCannon.isCarrying = true;
                grappleCannon.carriedObject = grabbedObject;
            }
            else
            {
                grabbedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                grabbedObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * 300);
            }
        }
    }
}
