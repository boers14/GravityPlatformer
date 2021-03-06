using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldPlayer : MonoBehaviour
{
    public LevelSelectIcon currentlySelectedLevelIcon = null;

    private string currentlySelectedLevel = "";

    private bool isMoving = false;

    [SerializeField]
    private Vector3 offsetOfPos = Vector3.zero;

    [SerializeField]
    private float tweenTime = 2;

    private Animator animator = null;

    private float cantMoveTimer = 0.25f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetLevelString();
    }

    private void Update()
    {
        if (cantMoveTimer >= 0)
        {
            cantMoveTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isMoving)
        {
            LevelManager.instance.SetCurrentLevelIndex(currentlySelectedLevelIcon, transform.position);
            SceneManager.LoadScene(currentlySelectedLevel);
        }

        if (currentlySelectedLevelIcon.upConnectedIcon != null)
        {
            if (!isMoving && currentlySelectedLevelIcon.upConnectedIcon.canSelect && Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveToLevel(currentlySelectedLevelIcon.upConnectedIcon);
            }
        }
        
        if (currentlySelectedLevelIcon.downConnectedIcon != null)
        {
            if (!isMoving && currentlySelectedLevelIcon.downConnectedIcon.canSelect && Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveToLevel(currentlySelectedLevelIcon.downConnectedIcon);
            }
        }
        
        if (currentlySelectedLevelIcon.leftConnectedIcon != null)
        {
            if (!isMoving && currentlySelectedLevelIcon.leftConnectedIcon.canSelect && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveToLevel(currentlySelectedLevelIcon.leftConnectedIcon);
            }
        }
        
        if (currentlySelectedLevelIcon.rightConnectedIcon != null)
        {
            if (!isMoving && currentlySelectedLevelIcon.rightConnectedIcon.canSelect && Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveToLevel(currentlySelectedLevelIcon.rightConnectedIcon);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isMoving)
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.D) && !isMoving)
        {
            SaveSytem.DeleteGame();
        }
    }

    private void MoveToLevel(LevelSelectIcon level)
    {
        animator.SetBool("isWalking", true);
        currentlySelectedLevelIcon = level;
        currentlySelectedLevel = currentlySelectedLevelIcon.levelConnectedToIcon;
        isMoving = true;
        iTween.MoveTo(gameObject, iTween.Hash("position", level.transform.position + offsetOfPos, "time", tweenTime,
            "easetype", iTween.EaseType.easeInOutSine, "oncomplete", "ReachedDestination", "oncompletetarget", gameObject));
    }

    private void ReachedDestination()
    {
        isMoving = false;
        animator.SetBool("isWalking", false);
    }

    public void SetLevelString()
    {
        currentlySelectedLevel = currentlySelectedLevelIcon.levelConnectedToIcon;
    }
}
