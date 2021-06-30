using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBlock : MonoBehaviour
{
    private Vector3 startPosOfPlayer = Vector3.zero;

    private void Start()
    {
        startPosOfPlayer = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.transform.position = startPosOfPlayer;
            collision.rigidbody.velocity = Vector3.zero;
        }
    }
}
