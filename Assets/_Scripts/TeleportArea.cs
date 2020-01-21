using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportArea : MonoBehaviour
{
    bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) // RMB
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
            {
                //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 10);
                //Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance;
                transform.position = new Vector3(hit.point.x, 0.5f, hit.point.z);
                Debug.Log("Obj - distance: " + hit.distance);

                if (!GetComponent<Renderer>().enabled) 
                    GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
            }
        }
        else if (GetComponent<Renderer>().enabled)
        {
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        }
    }
}
