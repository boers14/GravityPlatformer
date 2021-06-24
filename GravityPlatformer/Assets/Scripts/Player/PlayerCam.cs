using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    private float yOffset = 3, smoothPos = 3;

    [SerializeField]
    private GameObject player = null;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + yOffset, -10), 
            Time.fixedDeltaTime * smoothPos);
    }

    public void FlipYOffset()
    {
        yOffset *= -1;
    }
}
