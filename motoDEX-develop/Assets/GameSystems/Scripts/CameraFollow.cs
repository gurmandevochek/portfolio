using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
	}

	void FixedUpdate ()
	{
		
		if(GameObject.FindGameObjectWithTag("Player"))
		{
			target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
			Vector3 desiredPosition = target.position + offset;
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
			transform.position = smoothedPosition;
		}

		//transform.LookAt(target);
	}

}