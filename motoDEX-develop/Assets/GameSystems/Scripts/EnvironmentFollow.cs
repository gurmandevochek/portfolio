using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float multiplier = 1;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
    }

    void FixedUpdate ()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
            var desiredPosition = new Vector3((target.position.x + offset.x) * multiplier, offset.y, offset.z);
            transform.position = desiredPosition;
        }
    }
}
