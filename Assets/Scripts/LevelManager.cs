using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject[] levels;
	public int currentLevel = 0;

	Player player;

	void Awake () {
		player = GameObject.FindObjectOfType<Player> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void LoadNextLevel(int level){
		if (levels [level] != null) {
			levels [level].SetActive (true);
			levels [level - 1].SetActive (false);
			currentLevel = level;
		}
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
		}

		//reset patrolling enemies
		foreach (PatrollingEnemy child in levels [currentLevel].transform.GetChild (3).transform.GetComponentsInChildren<PatrollingEnemy>()) {
			child.gameObject.SetActive (true);
			child.ResetPatrol ();
		}

		player.gameObject.transform.rotation = Quaternion.identity;
		player.currentMoveCenter = player.transform.position;
		player.gameObject.SetActive (true);
		player.ResetRays ();

	}

}
