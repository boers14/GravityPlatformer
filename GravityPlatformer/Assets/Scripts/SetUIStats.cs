using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUIStats : MonoBehaviour
{
    [SerializeField]
    private float xSize = 2, ySize = 2, xPos = 0, yPos = 0, xPlacement = 1, yPlacement = 1, extraComperativeYSize = 0;

    private Vector2 originalSize = Vector2.zero;

    private bool hovered = false;

    [SerializeField]
    private bool isInList = false, setComparativeYSize = false, skipIteratingThroughtChilds = true, onlyEditChildWithEditTag = false;

    [SerializeField]
    private float scrollFactor = 2f;

    private Vector3 prevMousePos = Vector3.zero;

    private void Start()
    {
        if (isInList)
        {
            return;
        }

        SetUIPosition(xPos, yPos, xSize, ySize);
    }

    public void SetUIPosition(float xPos, float yPos, float xSize, float ySize,  int index = 0, float addedY = 0)
    {
        GameObject canvas = GameObject.Find("Canvas");

        if (transform.parent == null)
        {
            transform.SetParent(canvas.transform);
        }

        originalSize = GetComponent<RectTransform>().sizeDelta;
        Vector2 size = Vector2.zero;
        size.x = canvas.GetComponent<RectTransform>().sizeDelta.x / xSize;

        if (setComparativeYSize)
        {
            size.y = size.x * extraComperativeYSize;
        } else
        {
            size.y = canvas.GetComponent<RectTransform>().sizeDelta.y / ySize;
        }

        GetComponent<RectTransform>().sizeDelta = size;

        if (!skipIteratingThroughtChilds)
        {
            LoopTroughtChilds(transform, size);
        }

        Vector3 pos = Vector3.zero;

        if (yPos != 0)
        {
            if (isInList)
            {
                pos.y = (canvas.GetComponent<RectTransform>().sizeDelta.y / yPos * yPlacement) - (size.y / 2 * yPlacement) - size.y * index - 
                    canvas.GetComponent<RectTransform>().sizeDelta.y / addedY * index;
            }
            else
            {
                pos.y = (canvas.GetComponent<RectTransform>().sizeDelta.y / yPos * yPlacement) - (size.y / 2 * yPlacement);
            }
        }
        else
        {
            if (isInList)
            {
                pos.y -= size.y * index;
            }
        }

        if (xPos != 0)
        {
            pos.x = (-canvas.GetComponent<RectTransform>().sizeDelta.x / xPos * xPlacement) + (size.x / 2 * xPlacement);
        }

        GetComponent<RectTransform>().localPosition = pos;
    }

    private void LoopTroughtChilds(Transform uiObject, Vector2 size)
    {
        for (int i = 0; i < uiObject.childCount; i++)
        {
            if (onlyEditChildWithEditTag)
            {
                if (uiObject.GetChild(i).tag == "UIStatsEdit")
                {
                    SetComparitiveSize(uiObject.GetChild(i), size);
                }
            } else {
                SetComparitiveSize(uiObject.GetChild(i), size);
            }

            for (int j = 0; j < uiObject.GetChild(i).childCount; j++)
            {
                LoopTroughtChilds(uiObject.GetChild(i), size);
            }
        }
    }

    private void SetComparitiveSize(Transform uiObject, Vector2 size)
    {
        Vector2 decimalGrowth = Vector2.zero;
        decimalGrowth.x = size.x / originalSize.x;
        decimalGrowth.y = size.y / originalSize.y;

        RectTransform uiTransform = uiObject.GetComponent<RectTransform>();
        Vector2 newSize = Vector2.zero;
        newSize.x = uiTransform.sizeDelta.x * decimalGrowth.x;
        newSize.y = uiTransform.sizeDelta.y * decimalGrowth.y;

        uiTransform.sizeDelta = newSize;

        Vector3 newPos = Vector3.zero;
        newPos.x = uiTransform.localPosition.x * decimalGrowth.x;
        newPos.y = uiTransform.localPosition.y * decimalGrowth.y;
        uiTransform.localPosition = newPos;
    }

    public List<Button> CreateButtonSelectionUI(List<Button> allButtons, Button buttonPrefab, List<Sprite> availableOptions)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 mainSize = rectTransform.sizeDelta;

        for (int i = 0; i < availableOptions.Count; i++)
        {
            Button newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(transform);

            Vector2 size = Vector2.zero;
            size.y = mainSize.y;
            size.x = mainSize.x / 3.5f;
            newButton.GetComponent<RectTransform>().sizeDelta = size;

            Vector3 pos = Vector3.zero;
            pos.x = -mainSize.x / 2 + size.x / 2 + (size.x * i);
            newButton.GetComponent<RectTransform>().localPosition = pos;

            newButton.transform.GetChild(0).GetComponent<Image>().sprite = availableOptions[i];

            RectTransform iconTranform = newButton.transform.GetChild(0).GetComponent<RectTransform>();

            Vector2 initialSize = Vector2.zero;
            initialSize.x = newButton.GetComponent<RectTransform>().sizeDelta.y;
            initialSize.y = newButton.GetComponent<RectTransform>().sizeDelta.y;
            iconTranform.sizeDelta = initialSize;

            allButtons.Add(newButton);
        }

        return allButtons;
    }

    public void MoveUI(List<SetUIStats> allUIElements, float timer)
    {
        if (hovered && Input.GetMouseButton(0) && allUIElements.Count > 0 && timer <= 0)
        {
            Vector3 currentMousePos = Input.mousePosition;
            float movement = prevMousePos.y - currentMousePos.y;
            if (allUIElements[allUIElements.Count - 1].GetComponent<RectTransform>().localPosition.y - movement >
                    GetComponent<RectTransform>().sizeDelta.y / 2 - allUIElements[allUIElements.Count - 1].GetComponent<RectTransform>().sizeDelta.y / 2 ||
                    allUIElements[0].GetComponent<RectTransform>().localPosition.y -
                    movement < -GetComponent<RectTransform>().sizeDelta.y / 2 + allUIElements[0].GetComponent<RectTransform>().sizeDelta.y / 2)
            {
                return;
            }

            for (int i = 0; i < allUIElements.Count; i++)
            {
                Vector3 newPos = allUIElements[i].GetComponent<RectTransform>().localPosition;
                newPos.y -= movement * scrollFactor;
                allUIElements[i].GetComponent<RectTransform>().localPosition = newPos;
            }
        }

        prevMousePos = Input.mousePosition;
    }

    public void PointerEnter()
    {
        hovered = true;
    }

    public void PointerExit()
    {
        hovered = false;
    }
}
