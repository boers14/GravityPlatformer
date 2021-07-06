using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectIcon : MonoBehaviour
{
    public LevelSelectIcon leftConnectedIcon = null, rightConnectedIcon = null, downConnectedIcon = null, upConnectedIcon = null, 
        levelNeccesaryToBeSelected = null;

    public string levelConnectedToIcon = "";

    [System.NonSerialized]
    public bool isCompleted = false, canSelect = false;

    [SerializeField]
    private Color32 cantBeSelectedColor = Color.red, isNotBeatenColor = Color.blue, isBeatenColor = Color.green;

    [SerializeField]
    private Sprite UFOPartCollectedSprite = null;

    [SerializeField]
    private Image blackUFOPart = null;

    public void SetTheLevelStats()
    {
        if (levelNeccesaryToBeSelected != null)
        {
            if (!levelNeccesaryToBeSelected.isCompleted)
            {
                canSelect = false;
                GetComponent<SpriteRenderer>().color = cantBeSelectedColor;
            } else
            {
                CheckIfLevelCompleted();
            }
        } else
        {
            CheckIfLevelCompleted();
        }
    }

    private void CheckIfLevelCompleted()
    {
        canSelect = true;
        if (isCompleted)
        {
            GetComponent<SpriteRenderer>().color = isBeatenColor;
            blackUFOPart.sprite = UFOPartCollectedSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = isNotBeatenColor;
        }
    }
}
