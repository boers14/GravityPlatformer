using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField]
    private float yOffset = 3, smoothPos = 3, yOffsetSwitchDuration = 1f;

    [SerializeField]
    private GameObject player = null;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + yOffset, -10), 
            Time.fixedDeltaTime * smoothPos);
    }

    public IEnumerator FlipYOffset()
    {
        float elapsed = 0.0f;
        float neededOffset = yOffset * -1;
        int sign = 1;
        if (neededOffset < 0)
        {
            sign = -1;
        }
        float differenceBetweenOffset = yOffset * 2;
        if (differenceBetweenOffset < 1)
        {
            differenceBetweenOffset *= -1;
        }

        while (elapsed < yOffsetSwitchDuration)
        {
            float addedOffset = (differenceBetweenOffset * (Time.deltaTime / yOffsetSwitchDuration)) * sign;
            yOffset += addedOffset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        yOffset = neededOffset;
    }
}
