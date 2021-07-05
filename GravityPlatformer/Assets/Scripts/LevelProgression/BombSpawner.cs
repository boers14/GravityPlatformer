using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bomb = null;

    private float placementDist = 0;

    private void Start()
    {
        placementDist = GetComponent<SpriteRenderer>().size.x / 2 + bomb.GetComponent<SpriteRenderer>().size.x / 2 + 0.05f;
        SpawnBomb();
    }

    public void SpawnBomb()
    {
        GameObject newBomb = Instantiate(bomb, transform.position + transform.right * placementDist, transform.rotation);
        Vector3 rot = transform.eulerAngles;
        rot.z = 0;
        newBomb.transform.eulerAngles = rot;
        newBomb.GetComponent<Bomb>().bombSpawner = this;
    }
}
