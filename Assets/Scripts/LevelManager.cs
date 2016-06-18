using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject[] levels;
	int currentLevel = 0;

	Player player;

	void Start () {
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
		player.gameObject.transform.rotation = Quaternion.identity;
		player.gameObject.SetActive (true);
	}

}
