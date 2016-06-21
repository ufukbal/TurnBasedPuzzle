using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	[SerializeField]
	Button RestartButton;

	Player player;
	LevelManager levelManager;

	void Start () {
		player = GameObject.FindObjectOfType<Player> ();
		levelManager = GameObject.FindObjectOfType<LevelManager> ();
		player.OnGameOver += GameOverUI;
	}

	void GameOverUI(){
		RestartButton.gameObject.SetActive (true);
	}

	public void RestartButtonClick(){
		levelManager.RestartLevel ();
		RestartButton.gameObject.SetActive (false);
	}



}
