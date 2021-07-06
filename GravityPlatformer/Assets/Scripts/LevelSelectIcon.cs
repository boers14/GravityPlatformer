using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectIcon : MonoBehaviour
{
    public LevelSelectIcon leftConnectedIcon = null, rightConnectedIcon = null, downConnectedIcon = null, upConnectedIcon = null, 
        levelNeccesaryToBeSelected = null;

    public string levelConnectedToIcon = "";

    [System.NonSerialized]
    public bool isCompleted = false, canSelect = false;

    [SerializeField]
    private Color32 cantBeSelectedColor = Color.red, isNotBeatenColor = Color.blue, isBeatenColor = Color.green;

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
        }
        else
        {
            GetComponent<SpriteRenderer>().color = isNotBeatenColor;
        }
    }
}
