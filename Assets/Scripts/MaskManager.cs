using UnityEngine;
using System.Collections;

public class MaskManager : MonoBehaviour {
	public float rateOfMovement = 5500f;

	Vector3 leftPosition =  new Vector3 (-812,0,0);
	Vector3 rightPosition =  new Vector3 (812,0,0);

	public void OpenRightMask(){
		StartCoroutine (MoveMask (leftPosition));
	}
	public void OpenLeftMask(){
		StartCoroutine (MoveMask (rightPosition));
	}

	IEnumerator MoveMask(Vector3 newPos){
		while (true) {
			if (transform.localPosition.x == newPos.x) {
				rateOfMovement = 5500;
				break;
			}
			transform.localPosition = Vector3.MoveTowards (transform.localPosition, newPos, Time.deltaTime * rateOfMovement);
		//	rateOfMovement/=1.05f;

			yield return new WaitForSeconds (0.0001f);
		}




	}

}
