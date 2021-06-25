using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    [SerializeField]
    private PlayerCam playerCam = null;

    [SerializeField]
    private PlayerMovement playerMovement = null;

    [SerializeField]
    private float gravityFlipCooldown = 3f;

    private float timer = 0;

    public bool reverseGravity { get; set; } = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && timer <= 0)
        {
            reverseGravity = !reverseGravity;
            timer = gravityFlipCooldown;
            playerMovement.FlipJumpSpeed();
            playerMovement.setRotationConstraint = false;
            Physics2D.gravity *= -1;
            StartCoroutine(playerCam.FlipYOffset());

            iTween.RotateTo(gameObject, iTween.Hash("z", transform.eulerAngles.z + 180, "time", 0.4f, "easetype", iTween.EaseType.linear,
                "oncomplete", "SwitchRotationConstraint", "oncompletetarget", gameObject));

            GameObject[] objectInfluencedByGravity = GameObject.FindGameObjectsWithTag("GravityFlip");
            for (int i = 0; i < objectInfluencedByGravity.Length; i++)
            {
                iTween.RotateTo(objectInfluencedByGravity[i], iTween.Hash("z", objectInfluencedByGravity[i].transform.eulerAngles.z + 180, 
                    "time", 0.4f, "easetype", iTween.EaseType.linear));
            }
        }

        timer -= Time.deltaTime;
    }

    private void SwitchRotationConstraint()
    {
        playerMovement.setRotationConstraint = true;
    }
}
