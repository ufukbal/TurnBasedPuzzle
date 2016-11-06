using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrollingEnemy : MonoBehaviour {
	[SerializeField]
	public List<Transform> waypoints;
	[SerializeField]
	private bool startInc;
	private bool isInc = true;
	private float timeToMove = 0.3f;
	private int currentWaypointIndex = 0;
	[SerializeField]
	private int startWaypointIndex;
	Player player;

	void Start () {
		ResetPatrol ();
		player = GameObject.FindObjectOfType<Player> ();
		player.OnTurn += Patrol;
	}

	public void ResetPatrol(){
		transform.position = waypoints [startWaypointIndex].transform.position;
		currentWaypointIndex = startWaypointIndex;
		isInc = startInc;
	}

	void Patrol(){
		
			if (isInc)
				currentWaypointIndex++;
			else if (!isInc)
				currentWaypointIndex--;
		
			if (currentWaypointIndex == 0 || currentWaypointIndex == waypoints.Count - 1)
				isInc = !isInc;
	
		if (this.gameObject.transform.parent.parent.gameObject.activeSelf) { // check if level is active
			StartCoroutine ("PerformMove");
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



	
