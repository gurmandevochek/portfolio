using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelIgnoreCollision : MonoBehaviour
{

    public GameObject aiwheels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) {
        
        Debug.Log(other.gameObject.name);
        if(other.gameObject.tag == "AI Wheels")
        {
            aiwheels = other.gameObject;
            Physics.IgnoreCollision(aiwheels.GetComponent<Collider>(), GetComponent<Collider>());
        }

    }
}
