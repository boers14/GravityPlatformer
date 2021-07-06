using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isDoorBeforeOverWorld = false, isAlwaysOpen = false;

    [System.NonSerialized]
    public bool isOpen = false;

    [SerializeField]
    private string nextLevel = "";

    private void Start()
    {
        if (isAlwaysOpen)
        {
            isOpen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GoToNextLevel(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GoToNextLevel(collision);
    }

    private void GoToNextLevel(Collider2D collision)
    {
        if (isOpen && collision.tag == "Player")
        {
            if (isDoorBeforeOverWorld)
            {
                LevelManager.instance.isInOverworld = true;
                LevelManager.instance.playerCompletedLevel = true;
            }

            SceneManager.LoadScene(nextLevel);
        }
    }
}
