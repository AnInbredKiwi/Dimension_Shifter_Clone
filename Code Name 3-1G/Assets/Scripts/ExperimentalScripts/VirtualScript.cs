using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualScript : MonoBehaviour
{   
    public virtual void ToOverride()
    {
        Debug.Log("To override");
    }
}
