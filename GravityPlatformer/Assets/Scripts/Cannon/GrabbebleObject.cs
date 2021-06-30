using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabbebleObject : MonoBehaviour
{
    public enum InteractState
    {
        Grab,
        Push
    }

    public InteractState interactState = InteractState.Grab;

    public UnityEvent pushEvent = null;
}
