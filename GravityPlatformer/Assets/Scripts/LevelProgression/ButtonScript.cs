using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject connectedGameObject = null;

    [SerializeField]
    private Sprite pressedSprite = null, unpressedSprite = null;

    private bool isPressed = false;

    private int amountOfCollidingObjects = 0;

    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (amountOfCollidingObjects > 0)
        {
            if (!isPressed)
            {
                isPressed = true;
                connectedGameObject.SetActive(false);
                spriteRenderer.sprite = pressedSprite;
            }
        }
        else
        {
            if (isPressed)
            {
                isPressed = false;
                connectedGameObject.SetActive(true);
                spriteRenderer.sprite = unpressedSprite;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        amountOfCollidingObjects++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        amountOfCollidingObjects--;
    }
}
