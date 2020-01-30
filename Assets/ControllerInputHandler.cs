using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

        // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        float trigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

        GetComponent<Renderer>().material.color = new Color(255 - trigger * 25, 255 - trigger * 25, 255);
        
    }

    void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
