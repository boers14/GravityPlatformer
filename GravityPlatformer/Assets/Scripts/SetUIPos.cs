using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUIPos : MonoBehaviour
{
    [SerializeField]
    private float xPos = 0, yPos = 0, xPlacement = 1, yPlacement = 1;

    private void Start()
    {
        SetUIPosition();
    }

    private void SetUIPosition()
    {
        GameObject canvas = GameObject.Find("Canvas");

        Vector2 size = GetComponent<RectTransform>().sizeDelta;

        Vector3 pos = Vector3.zero;

        if (yPos != 0)
        {
            pos.y = (canvas.GetComponent<RectTransform>().sizeDelta.y / yPos * yPlacement) - (size.y / 2 * yPlacement);
        }

        if (xPos != 0)
        {
            pos.x = (-canvas.GetComponent<RectTransform>().sizeDelta.x / xPos * xPlacement) + (size.x / 2 * xPlacement);
        }

        GetComponent<RectTransform>().localPosition = pos;
    }
}
