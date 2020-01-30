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
        // Reads the right controller trigger button
        var trigger = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);

        // Changes the color of the right hand on click
        var colorVal = (int)(255.0 - trigger * 255.0);

        GetComponent<Renderer>().material.color = new Color(colorVal, 255, 255);
  
        if(trigger > 0.9){
        	if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward))){
	        	Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.yellow, 1);
	        }
	        else{
	        	Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red, 1);
	        }
        }
    }

    void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
