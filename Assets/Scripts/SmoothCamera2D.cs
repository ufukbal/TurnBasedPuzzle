using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Camera cam;
	public bool isCameraMoving;

	void Start(){
		
	}
	void Update () 
	{
		if (target)
		{
			Vector3 point = cam.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = cam.transform.position + delta;
			cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, dampTime);
		}

	}
	void OnTriggerEnter(Collider other) {
		if (target)
		{
		//	if(other.tag == "CameraTrack")
			//	StartCoroutine ("SmoothCamera");

		}
	}

	IEnumerator SmoothCamera() {
		isCameraMoving = true;
		float elapsedTime = 0f;
		Vector3 inititalUp = transform.up;

		while (elapsedTime < dampTime) {
			Vector3 point = cam.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = cam.transform.position + delta;
			cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, elapsedTime/dampTime);
			elapsedTime += Time.deltaTime;
			yield return null;
		}


		isCameraMoving = false;
	}

}