using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	[SerializeField]
	Button RestartButton;
	[SerializeField]
	Text playerMoveLabel;
	[SerializeField]
	Text levelGoalLabel;
	[SerializeField]
	Text killedEnemiesLabel;
	[SerializeField]
	Text aliveEnemiesLabel;
	[SerializeField]
	GameObject levelFinishedPanel;
	int killCount;
	int aliveEnemies;
	Player player;
	LevelManager levelManager;

	void Start () {
		player = GameObject.FindObjectOfType<Player> ();
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		player.OnGameOver += GameOverUI;
		player.LevelFinished += LevelFinished;
	}

	void GameOverUI(){
		RestartButton.gameObject.SetActive (true);
	}

	public void RestartButtonClick(){
		levelManager.RestartLevel ();
		RestartButton.gameObject.SetActive (false);
	}

	public void LevelFinished(){
		
		player.enabled = false;
		levelFinishedPanel.SetActive (true);
		playerMoveLabel.text = player.moveCount.ToString();
		levelGoalLabel.text = levelManager.moveLimits[levelManager.currentLevel].ToString();

		//count enemies alive and dead
		foreach (Transform child in levelManager.levels [levelManager.currentLevel].transform.GetChild (2).transform) {
			if (!child.gameObject.activeSelf) {
				killCount++;
			} else
				aliveEnemies++;
		}

		killedEnemiesLabel.text = killCount.ToString ();
		aliveEnemiesLabel.text = aliveEnemies.ToString ();
		//save to play prefs
		Debug.Log (killCount);
	}
	public void CloseMenu(GameObject menu){
		menu.SetActive (false);
		killCount = 0;
		aliveEnemies = 0;
	}



}
