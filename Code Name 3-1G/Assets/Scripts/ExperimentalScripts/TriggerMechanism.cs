using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerMechanism : MonoBehaviour
{
    protected bool triggered;

    public virtual void Start()
    {
    }

    protected virtual void Trigger()
    {
        triggered = true;
    }

    protected virtual void Untrigger()
    {
        triggered = false;
    }
}
