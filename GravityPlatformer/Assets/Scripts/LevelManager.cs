using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    [System.NonSerialized]
    public List<LevelSelectIcon> allLevels = new List<LevelSelectIcon>();

    public List<bool> allLevelCompletedStats = new List<bool>();

    private int currentSelectedLevelIndex = 0;

    [System.NonSerialized]
    public Vector3 playerPos = Vector3.zero;

    [System.NonSerialized]
    public bool isInOverworld = true, playerCompletedLevel = false, isOriginalObject = false;

    private void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            isOriginalObject = true;
        } else
        {
            Destroy(gameObject);
        }

        GameObject[] allLevelObjects = GameObject.FindGameObjectsWithTag("LevelSelectIcon");
        allLevels.Clear();
        for (int i = 0; i < allLevelObjects.Length; i++)
        {
            allLevelCompletedStats.Add(false);
            allLevels.Add(allLevelObjects[i].GetComponent<LevelSelectIcon>());
        }

        for (int i = 0; i < allLevels.Count; i++)
        {
            allLevels[i].SetTheLevelStats();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (isInOverworld && isOriginalObject)
        {
            FetchAndSetAllStats();
        }
    }

    private void FetchAndSetAllStats()
    {
        GameObject[] allLevelObjects = GameObject.FindGameObjectsWithTag("LevelSelectIcon");
        allLevels.Clear();
        for (int i = 0; i < allLevelObjects.Length; i++)
        {
            allLevels.Add(allLevelObjects[i].GetComponent<LevelSelectIcon>());
        }

        if (playerCompletedLevel)
        {
            allLevelCompletedStats[currentSelectedLevelIndex] = true;
        }

        for (int i = 0; i < allLevels.Count; i++)
        {
            allLevels[i].isCompleted = allLevelCompletedStats[i];
        }

        for (int i = 0; i < allLevels.Count; i++)
        {
            allLevels[i].SetTheLevelStats();
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerPos;
    }

    public void SetCurrentLevelIndex(LevelSelectIcon level, Vector3 pos)
    {
        currentSelectedLevelIndex = allLevels.IndexOf(level);
        playerPos = pos;
        isInOverworld = false;
        playerCompletedLevel = false;
    }
}
