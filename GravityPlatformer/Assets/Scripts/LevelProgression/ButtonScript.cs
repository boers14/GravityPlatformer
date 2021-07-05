using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private enum LockColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [SerializeField]
    private LockColor lockColor = LockColor.Red;

    private List<GameObject> connectedGameObjects = new List<GameObject>();

    [SerializeField]
    private Sprite pressedSprite = null, unpressedSprite = null;

    private bool isPressed = false;

    private List<ButtonScript> connectedButtons = new List<ButtonScript>();

    private int amountOfCollidingObjects = 0;

    private SpriteRenderer spriteRenderer = null;

    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();

        switch (lockColor)
        {
            case LockColor.Red:
                FillConnectedGameObjectsList("RedLock");
                break;
            case LockColor.Blue:
                FillConnectedGameObjectsList("BlueLock");
                break;
            case LockColor.Green:
                FillConnectedGameObjectsList("GreenLock");
                break;
            case LockColor.Yellow:
                FillConnectedGameObjectsList("YellowLock");
                break;
        }

        ButtonScript[] buttonScripts = (ButtonScript[])FindObjectsOfType(typeof(ButtonScript));
        for (int i = 0; i < buttonScripts.Length; i++)
        {
            if (buttonScripts[i].lockColor == lockColor && buttonScripts[i] != this)
            {
                connectedButtons.Add(buttonScripts[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        if (amountOfCollidingObjects > 0)
        {
            if (!isPressed)
            {
                isPressed = true;
                for (int i = 0; i < connectedGameObjects.Count; i++)
                {
                    connectedGameObjects[i].SetActive(false);
                }
                spriteRenderer.sprite = pressedSprite;
            }
        }
        else
        {
            if (isPressed)
            {
                isPressed = false;
                bool otherPressedButtons = false;

                for (int i = 0; i < connectedButtons.Count; i++)
                {
                    if (connectedButtons[i].isPressed)
                    {
                        otherPressedButtons = true;
                    }
                }

                if (!otherPressedButtons)
                {
                    SwapListActiveState(true);
                }
                spriteRenderer.sprite = unpressedSprite;
            }
        }
    }

    private void SwapListActiveState(bool activeState)
    {
        for (int i = 0; i < connectedGameObjects.Count; i++)
        {
            connectedGameObjects[i].SetActive(activeState);
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

    private void FillConnectedGameObjectsList(string colorTag)
    {
        GameObject[] allConnectedLocks = GameObject.FindGameObjectsWithTag(colorTag);
        for (int i = 0; i < allConnectedLocks.Length; i++)
        {
            connectedGameObjects.Add(allConnectedLocks[i]);
        }
    }
}
