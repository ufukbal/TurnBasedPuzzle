using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject[] levels;
	public int[] moveLimits;
	public int currentLevel = 0;

	Player player;

	void Awake () {
		player = GameObject.FindObjectOfType<Player> ();
		//player.LevelFinished += LoadNextLevel;
	}

	// Update is called once per frame
	void Update () {

	}

	public void LoadNextLevel(){
		if (levels [currentLevel+1] != null) {
			levels [currentLevel+1].SetActive (true);
			levels [currentLevel +1 - 1].SetActive (false);
			currentLevel = currentLevel +1;
		}
		player.enabled = true;
		player.moveCount = 0;
	}

	public void RestartLevel(){
		for (int i = 0; i < levels.Length; i++) {
			levels [i].SetActive (false);
		}

		levels [currentLevel].SetActive (true);
		player.gameObject.transform.position = levels [currentLevel].transform.GetChild (1).GetChild (0).transform.position;

		//reset enemies
		foreach (Transform child in levels [currentLevel].transform.GetChild (2).transform) {
			child.gameObject.SetActive (true);

			if(child.GetComponentInChildren<RotatingEnemy>()!= null)
				child.GetComponentInChildren<RotatingEnemy>().ResetEnemyRotation ();

		}

		//reset patrolling enemies
		foreach (PatrollingEnemy child in levels [currentLevel].transform.GetChild (3).transform.GetComponentsInChildren<PatrollingEnemy>()) {
			child.gameObject.SetActive (true);
			child.ResetPatrol ();
		}


		player.gameObject.transform.rotation = Quaternion.identity;
		player.currentMoveCenter = player.transform.position;
		player.moveCount = 0;
		player.gameObject.SetActive (true);
		player.ResetRays ();

	}

}
