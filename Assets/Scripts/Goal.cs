using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	public GameObject[] levels;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadNextLevel(int level){
		if (levels [level] != null) {
			levels [level].SetActive (true);
			levels [level - 1].SetActive (false);
		}
	}

}
