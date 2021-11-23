using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverrideScript : VirtualScript
{
    // Start is called before the first frame update
    void Start()
    {
        ToOverride();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void ToOverride()
    {
        Debug.Log("And add");
    }
}
