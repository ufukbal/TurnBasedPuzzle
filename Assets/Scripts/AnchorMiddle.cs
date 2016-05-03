using UnityEngine;
using System.Collections;

public class AnchorMiddle : MonoBehaviour {
	Vector3 startPos;
	void Start(){
		startPos = transform.position;
	}


	void LateUpdate () {
	
		transform.position = startPos;

	}
}
