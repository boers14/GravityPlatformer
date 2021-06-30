using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravitySwitch : MonoBehaviour
{
    [SerializeField]
    private PlayerCam playerCam = null;

    private PlayerMovement playerMovement = null;

    [SerializeField]
    private float gravityFlipCooldown = 3f;

    [SerializeField]
    private Image cooldownImage = null;

    private float timer = 0;

    public bool reverseGravity { get; set; } = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cooldownImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && timer <= 0)
        {
            cooldownImage.gameObject.SetActive(true);
            reverseGravity = !reverseGravity;
            timer = gravityFlipCooldown;
            playerMovement.FlipJumpSpeed();
            playerMovement.setRotationConstraint = false;
            Physics2D.gravity *= -1;
            StartCoroutine(playerCam.FlipYOffset());
            GetComponent<GrappleCannon>().enabled = false;

            iTween.RotateTo(gameObject, iTween.Hash("z", transform.eulerAngles.z + 180, "time", 0.4f, "easetype", iTween.EaseType.linear,
                "oncomplete", "SwitchRotationConstraint", "oncompletetarget", gameObject));

            GameObject[] objectInfluencedByGravity = GameObject.FindGameObjectsWithTag("GravityFlip");
            for (int i = 0; i < objectInfluencedByGravity.Length; i++)
            {
                iTween.RotateTo(objectInfluencedByGravity[i], iTween.Hash("z", objectInfluencedByGravity[i].transform.eulerAngles.z + 180, 
                    "time", 0.4f, "easetype", iTween.EaseType.linear));
            }
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownImage.fillAmount = timer / gravityFlipCooldown;

            if (timer <= 0)
            {
                cooldownImage.gameObject.SetActive(false);
            }
        }
    }

    private void SwitchRotationConstraint()
    {
        playerMovement.setRotationConstraint = true;
        GetComponent<GrappleCannon>().enabled = true;
    }
}
