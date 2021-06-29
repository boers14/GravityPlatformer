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

    private float placementDist = 0, timer = 0, retreatTime = 0;

    private GameObject currentGrapple = null;

    [System.NonSerialized]
    public List<GameObject> allGrapplePieces = new List<GameObject>();

    private void Start()
    {
        Vector3 pos = transform.position;
        pos.z += 1;
        transform.position = pos;
        placementDist = GetComponent<BoxCollider2D>().bounds.size.x / 2 + 0.02f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isFiring)
        {
            isFiring = true;

            iTween.Stop(gameObject);
            currentGrapple = Instantiate(grapple, transform.position + transform.right * placementDist, transform.rotation);
            Vector3 newPos = currentGrapple.transform.position;
            newPos.z += 10;
            currentGrapple.transform.position = newPos;
            currentGrapple.transform.SetParent(transform);

            currentGrapple.GetComponentInChildren<Grapple>().grappleCannon = this;
            currentGrapple.GetComponent<Rope>().grappleCannon = this;
            allGrapplePieces.Add(currentGrapple);

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            iTween.MoveTo(currentGrapple, iTween.Hash("position", transform.position + (transform.right * grappleDistance), "time", tweenTime / 2, 
                "easetype", iTween.EaseType.linear, "oncomplete", "MoveGrappleBack", "oncompletetarget", gameObject));
            timer = tweenTime / 2;
            tag = "Untagged";
        }

        if (timer > 0 )
        {
            timer -= Time.deltaTime;
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

            Vector3 moveToPos = transform.position;
            moveToPos.z = 10;
            retreatTime = (tweenTime / 2) - timer;
            iTween.MoveTo(currentGrapple, iTween.Hash("position", moveToPos, "time", retreatTime, "easetype", iTween.EaseType.linear,
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
        tag = "GravityFlip";
    }
}
