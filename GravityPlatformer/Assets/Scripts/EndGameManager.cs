using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> endGameObjects = new List<GameObject>();

    [SerializeField]
    private RectTransform canvas = null;

    public void CelebrateEndGame()
    {
        for (int i = 0; i < endGameObjects.Count; i++)
        {
            endGameObjects[i].SetActive(true);
            if (endGameObjects[i].GetComponent<ParticleSystem>() != null)
            {
                ParticleSystem.ShapeModule shapeModule = endGameObjects[i].GetComponent<ParticleSystem>().shape;
                shapeModule.radius = canvas.sizeDelta.x / 180;
            }
        }
    }
}
