using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrollingEnemy : MonoBehaviour {

	public List<Transform> waypoints;
	public bool isInc = true;
	public float timeToMove = 1f;
	public int currentWaypointIndex = 0;
	Player player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindObjectOfType<Player> ();
		player.OnTurn += Patrol;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Patrol(){
		
			if (isInc)
				currentWaypointIndex++;
			else if (!isInc)
				currentWaypointIndex--;
		
			if (currentWaypointIndex == 0 || currentWaypointIndex == waypoints.Count - 1)
				isInc = !isInc;
	

			//return waypoints [currentWaypointIndex];
		if (this.gameObject.transform.parent.parent.gameObject.activeSelf) { // check if level is active
			StartCoroutine ("PerformMove");
			Debug.Log (waypoints [currentWaypointIndex]);
		}

	}

	IEnumerator PerformMove() {
		
		float elapsedTime = 0f;
		Vector3 initialPosition = transform.position;


		while (elapsedTime < timeToMove) {
			transform.position = Vector3.Lerp(initialPosition, waypoints [currentWaypointIndex].position, (elapsedTime / timeToMove));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.position = waypoints [currentWaypointIndex].position;


	}

		
}



	