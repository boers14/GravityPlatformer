using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleCannon : MonoBehaviour
{
    [SerializeField]
    private GameObject grapple = null;

    [SerializeField]
    private float grappleDistance = 5, tweenTime = 1.5f;

    private bool isFiring = false, isRetreating = false;

    [System.NonSerialized]
    public bool isCarrying = false;

    [System.NonSerialized]
    public Transform carriedObject = null;

    [System.NonSerialized]
    public float extraCarryDist = 0;

    private float placementDist = 0, upPlacementDist = 0, timer = 0, retreatTime = 0;

    private GameObject currentGrapple = null;

    [System.NonSerialized]
    public List<GameObject> allGrapplePieces = new List<GameObject>();

    private Vector3 returnPos = Vector3.zero;

    private Animator animator = null;

    private void Start()
    {
        Vector3 pos = transform.position;
        pos.z += 1;
        transform.position = pos;
        placementDist = GetComponent<BoxCollider2D>().bounds.size.x / 4;
        upPlacementDist = GetComponent<BoxCollider2D>().bounds.size.y * 0.2f;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isCarrying)
        {
            if (Input.GetKey(KeyCode.X) && carriedObject != null)
            {
                carriedObject.position = transform.position + transform.right * (placementDist * 2 + extraCarryDist);
            } else
            {
                carriedObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                carriedObject.GetComponent<BoxCollider2D>().isTrigger = false;
                isCarrying = false;
                carriedObject = null;
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.X) && !isFiring)
            {
                isFiring = true;

                iTween.Stop(gameObject);
                returnPos = transform.position + transform.right * placementDist - (transform.up * upPlacementDist);
                currentGrapple = Instantiate(grapple, returnPos, transform.rotation);
                currentGrapple.transform.SetParent(transform);

                currentGrapple.GetComponentInChildren<Grapple>().grappleCannon = this;
                currentGrapple.GetComponent<Rope>().grappleCannon = this;
                allGrapplePieces.Add(currentGrapple);

                GetComponent<PlayerMovement>().enabled = false;
                GetComponent<GravitySwitch>().enabled = false;
                animator.SetBool("isWalking", false);

                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                iTween.MoveTo(currentGrapple, iTween.Hash("position", currentGrapple.transform.position + (transform.right * grappleDistance),
                    "time", tweenTime / 2, "easetype", iTween.EaseType.linear, "oncomplete", "MoveGrappleBack", "oncompletetarget", gameObject));
                timer = tweenTime / 2;
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }
    }

    public void MoveGrappleBack()
    {
        if (!isRetreating)
        {
            isRetreating = true;

            for (int i = 0; i < allGrapplePieces.Count; i++)
            {
                allGrapplePieces[i].GetComponent<Rope>().canDeactivate = true;
            }
            iTween.Stop(currentGrapple);

            retreatTime = (tweenTime / 2) - timer;
            iTween.MoveTo(currentGrapple, iTween.Hash("position", returnPos, "time", retreatTime, "easetype", iTween.EaseType.linear,
                "oncomplete", "ResetCannon", "oncompletetarget", gameObject));

            timer = tweenTime / 2;
        }
    }

    private void ResetCannon()
    {
        isRetreating = false;
        isFiring = false;
        for (int i = 0; i < allGrapplePieces.Count; i++)
        {
            Destroy(allGrapplePieces[i]);
        }
        allGrapplePieces.Clear();
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().AddForce(-transform.up * 0.01f);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<GravitySwitch>().enabled = true;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetBool("isWalking", true);
        }
    }
}
