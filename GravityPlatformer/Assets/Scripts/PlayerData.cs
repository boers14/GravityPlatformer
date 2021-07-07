using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<bool> allLevelCompletedStats = new List<bool>();

    public float[] playerPos;

    public int playerLevelIndex = 0;

    public PlayerData()
    {
        for (int i = 0; i < LevelManager.instance.allLevelCompletedStats.Count; i++)
        {
            allLevelCompletedStats.Add(LevelManager.instance.allLevelCompletedStats[i]);
        }

        playerPos = new float[3];
        playerPos[0] = LevelManager.instance.playerPos.x;
        playerPos[1] = LevelManager.instance.playerPos.y;
        playerPos[2] = LevelManager.instance.playerPos.z;

        playerLevelIndex = LevelManager.instance.currentSelectedLevelIndex;
    }
}
