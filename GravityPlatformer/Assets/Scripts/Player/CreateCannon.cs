using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCannon : MonoBehaviour
{
    [SerializeField]
    private GameObject cannon = null;

    [SerializeField]
    private int maxAmountOfCannons = 3;

    private List<GameObject> grappleCannons = new List<GameObject>();

    private float placementDist = 0;

    void Start()
    {
        placementDist = GetComponent<BoxCollider2D>().bounds.size.x / 2 + 0.02f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject grappleCannon = Instantiate(cannon, transform.position + transform.right * placementDist, transform.rotation);
            grappleCannons.Add(grappleCannon);
            if (grappleCannons.Count > maxAmountOfCannons)
            {
                GameObject removedCannon = grappleCannons[0];
                Destroy(removedCannon);
                grappleCannons.RemoveAt(0);
            }
        }
    }
}
