using UnityEngine;
using System.Collections;

public class RotatingEnemy : MonoBehaviour {

	[SerializeField]
	int startRotation;

	[SerializeField]
	int rotation;

	Player player;
	// Use this for initialization
	void Start () {
		ResetEnemyRotation ();
		player = GameObject.FindObjectOfType<Player> ();
		player.OnTurn += Rotate;
	}

	public void ResetEnemyRotation(){

		transform.localRotation = Quaternion.Euler(0, startRotation, 0);

	}

	private void Rotate(){
		transform.Rotate (new Vector3 (0, rotation, 0));
	}

}
