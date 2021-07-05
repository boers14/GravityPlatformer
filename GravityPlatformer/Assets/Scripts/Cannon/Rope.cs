using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [System.NonSerialized]
    public GrappleCannon grappleCannon = null;

    [SerializeField]
    private GameObject rope = null;

    private bool hasCreatedNewRope = false;

    [System.NonSerialized]
    public bool canDeactivate = false;

    [System.NonSerialized]
    public GameObject prevRope = null;

    [SerializeField]
    private bool removePrevRope = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDeactivate && collision.gameObject == grappleCannon.gameObject)
        {
            if (prevRope != null)
            {
                if (removePrevRope)
                {
                    prevRope.GetComponent<SpriteRenderer>().enabled = false;
                    prevRope.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasCreatedNewRope && collision.gameObject == grappleCannon.gameObject && !canDeactivate)
        {
            CreateNewRope();
        }
    }

    private void CreateNewRope()
    {
        hasCreatedNewRope = true;

        GameObject newRope = Instantiate(rope, transform.position, transform.rotation);
        newRope.transform.position -= transform.right * GetComponent<SpriteRenderer>().size.x;

        newRope.GetComponent<Rope>().grappleCannon = grappleCannon;
        newRope.transform.SetParent(transform);
        prevRope = newRope;
        grappleCannon.allGrapplePieces.Add(newRope);
    }
}
