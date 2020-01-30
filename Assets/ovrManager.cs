using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ovrManager : MonoBehaviour
{	public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        if(OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.5f){
        	Instantiate(prefab, new Vector3(375, 170, -419), Quaternion.identity);
        }
    }

    void FixedUpdate()
    {
        OVRInput.FixedUpdate();
    }
}
