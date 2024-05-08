using UnityEngine;

public class Cam : MonoBehaviour
{
    private Transform target;
	public float smootPosition;

	public float smootSize;

	public float height = 10f;

	public float minSize = 12f;

	public float maxSize = 18f;
    private Camera cam;
    Transform camT;

	[HideInInspector]
	public bool _sizeCamera;
private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
        cam = Camera.main;
        camT = transform;
	}

	private void FixedUpdate()
	{
		if(target != null)
		{
		Vector3 b = new Vector3(target.position.x, height,target.position.z);
		camT.position = Vector3.Lerp(transform.position, b, Time.deltaTime * smootPosition);	
		}
	}
}