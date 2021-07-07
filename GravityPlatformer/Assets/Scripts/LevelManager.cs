using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    [System.NonSerialized]
    public List<LevelSelectIcon> allLevels = new List<LevelSelectIcon>();

    [System.NonSerialized]
    public List<bool> allLevelCompletedStats = new List<bool>();

    [System.NonSerialized]
    public int currentSelectedLevelIndex = 0;

    [System.NonSerialized]
    public Vector3 playerPos = Vector3.zero;

    private Vector2 originalGravity = Vector2.zero;

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
            return;
        }

        GameObject[] allLevelObjects = GameObject.FindGameObjectsWithTag("LevelSelectIcon");
        for (int i = 0; i < allLevelObjects.Length; i++)
        {
            allLevels.Add(allLevelObjects[i].GetComponent<LevelSelectIcon>());
        }

        if (SaveSytem.CheckIfFileExist())
        {
            PlayerData playerData = SaveSytem.LoadGame();

            for (int  i = 0; i < playerData.allLevelCompletedStats.Count; i++)
            {
                allLevelCompletedStats.Add(playerData.allLevelCompletedStats[i]);
            }

            for (int i = 0; i < allLevels.Count; i++)
            {
                allLevels[i].isCompleted = allLevelCompletedStats[i];
            }

            currentSelectedLevelIndex = playerData.playerLevelIndex;

            playerPos.x = playerData.playerPos[0];
            playerPos.y = playerData.playerPos[1];
            playerPos.z = playerData.playerPos[2];

            SetPlayerStats();
        }
        else
        {
            for (int i = 0; i < allLevelObjects.Length; i++)
            {
                allLevelCompletedStats.Add(false);
            }
        }

        for (int i = 0; i < allLevels.Count; i++)
        {
            allLevels[i].SetTheLevelStats();
        }

        originalGravity = Physics2D.gravity;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (isInOverworld && isOriginalObject)
        {
            FetchAndSetAllStats();
        }
        else if (isOriginalObject)
        {
            Physics2D.gravity = originalGravity;
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

        SetPlayerStats();

        SaveSytem.SaveGame();
    }

    private void SetPlayerStats()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerPos;
        player.GetComponent<OverworldPlayer>().currentlySelectedLevelIcon = allLevels[currentSelectedLevelIndex];
    }

    public void SetCurrentLevelIndex(LevelSelectIcon level, Vector3 pos)
    {
        currentSelectedLevelIndex = allLevels.IndexOf(level);
        playerPos = pos;
        isInOverworld = false;
        playerCompletedLevel = false;
    }
}
