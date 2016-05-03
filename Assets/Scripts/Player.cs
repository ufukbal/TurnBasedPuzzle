using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public bool drawDebugGizmos = false;
    public float timeToMove = 1f;

    Vector3 invalidPosition = new Vector3(9999, 9999, 9999);
    Vector3 positionToMoveTo;
    Vector3 currentMove;
    Vector3 checkDirection;
    bool checkedDiagonal = false;
    bool isMoving = false;
    Vector3 transformUp;
    Vector3 currentMoveCenter;
    Ray currentMoveRay;
    Ray wallCheckRay;
	Ray checkObstacleRay;

	int currentLevel = 0;

	Goal goal;

	public ParticleSystem enemyDeathFx;
	public ParticleSystem playerDeathFx;

	public event System.Action OnMove; //turn

    void Start() {
		goal = FindObjectOfType<Goal> ();

		for (int i = currentLevel+1; i < goal.levels.Length; i++) {
			goal.levels [i].SetActive (false);
		}

        positionToMoveTo = invalidPosition;

    }

	void Update () {
        if (isMoving) {
            return;
        }

		if (Input.GetKeyDown(KeyCode.W)) {
            ResetChecks();
            currentMove = transform.forward;
            Move(currentMove);
            if (positionToMoveTo != invalidPosition) {
				ProcessAIMove ();
                StartCoroutine("PerformMove");
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            ResetChecks();
            currentMove = transform.right * -1;
            Move(currentMove);
            if (positionToMoveTo != invalidPosition) {
				ProcessAIMove ();
                StartCoroutine("PerformMove");
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            ResetChecks();
            currentMove = transform.forward * -1;
            Move(currentMove);
            if (positionToMoveTo != invalidPosition) {
				ProcessAIMove ();
                StartCoroutine("PerformMove");
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            ResetChecks();
            currentMove = transform.right;
            Move(currentMove);
            if (positionToMoveTo != invalidPosition) {
				ProcessAIMove ();
                StartCoroutine("PerformMove");
            }
        }
    }



    IEnumerator PerformMove() {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 inititalUp = transform.up;

        while (elapsedTime < timeToMove) {
            transform.position = Vector3.Lerp(initialPosition, positionToMoveTo, (elapsedTime / timeToMove));
            transform.up = Vector3.Lerp(inititalUp, transformUp, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = positionToMoveTo;
        transform.up = transformUp;

        positionToMoveTo = invalidPosition;
		checkObstacle ();
        isMoving = false;
    }

    void Move(Vector3 offset) {
        Vector3 pointToCheck = transform.position + offset;

        currentMoveCenter = pointToCheck;
        currentMoveRay = new Ray(currentMoveCenter, checkDirection);

        RaycastHit hit;
        if (Physics.Raycast(currentMoveRay, out hit, 1f)) {
			if (hit.collider.tag == "Walkable" ) { 
				
				transformUp = hit.normal;
				positionToMoveTo = pointToCheck;
			} 
			else if (hit.collider.tag == "Enemy") {
				Debug.Log ("Kill");
				transformUp = hit.normal;
				positionToMoveTo = pointToCheck;
				KillEnemy (hit.transform.parent, offset);
			}
			else if (hit.collider.tag == "Deadly") {
				Debug.Log ("Die");
				transformUp = hit.normal;
				positionToMoveTo = pointToCheck;
			}
			else if (hit.collider.tag == "Goal") {
				Debug.Log ("Goal");
				int nextLevel;
				int.TryParse(hit.collider.name, out nextLevel);
				transformUp = hit.normal;
				positionToMoveTo = pointToCheck;
				goal.LoadNextLevel (nextLevel+1);
			}
			else if (hit.collider.tag == "Obstacle") {
				Debug.Log ("Can't move");
			}

        }

        if (positionToMoveTo != invalidPosition) {
            return;
        }

        if (WallCheck()) {
			ProcessAIMove ();
            StartCoroutine("ChangeOrientation");
            return;
        }
        else if (!checkedDiagonal) {
            checkedDiagonal = true;
            checkDirection = currentMove * -1;
            Move(currentMove + transform.up * -1);
        }
    }

    IEnumerator ChangeOrientation() {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 inititalUp = transform.up;

        while (elapsedTime < timeToMove) {
            transform.up = Vector3.Lerp(inititalUp, transformUp, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.up = transformUp;
		checkObstacle ();
        isMoving = false;
    }

    void ResetChecks() {
        checkDirection = -1 * transform.up;
        checkedDiagonal = false;
    }

    bool WallCheck() {
        wallCheckRay = new Ray(transform.position, currentMove);
        RaycastHit hit;
        if (Physics.Raycast(wallCheckRay, out hit, 1f)) {
            if (hit.collider.tag == "Walkable") {
                transformUp = hit.normal;
                return true;
            }
			else if (hit.collider.tag == "Goal") {
				transformUp = hit.normal;
				Debug.Log ("Goal");
				return true;
			}
			else if (hit.collider.tag == "Enemy") {
				transformUp = hit.normal;
				Debug.Log ("Kill Enemy");
				KillEnemy (hit.collider.transform.parent, hit.normal);
				return true;
			}
			else if (hit.collider.tag == "Deadly") {
				transformUp = hit.normal;
				Debug.Log ("Dies");
				Die (-hit.normal);
				return true;
			}
			else if (hit.collider.tag == "Obstacle") {
				Debug.Log ("Can't move");
				return false;
			}
        }

        return false;
    }

	public void GameOver(){
		
	}

	public void ProcessAIMove(){
		if (OnMove != null)
			OnMove (); 
	
	}

	public void checkObstacle(){ //check after movement for overlapping objects
		ResetChecks ();
		Vector3 pointToCheck = transform.position;


		checkObstacleRay = new Ray(pointToCheck,checkDirection);

		RaycastHit hit;
		if (Physics.Raycast(checkObstacleRay, out hit, 1f)) {
			if (hit.collider.tag == "Obstacle") {
				
				Die(-checkDirection);
				Debug.Log ("die check obstacle");
			}
				
			else if (hit.collider.tag == "Deadly") {
				Debug.Log ("die check obstacle");
				transformUp = hit.normal;
				positionToMoveTo = pointToCheck;
				Debug.Log ("Dies");
				Die (-checkDirection);
			}

		}
			
	}

	public void KillEnemy(Transform obj, Vector3 impactDirection){
		Instantiate (enemyDeathFx, obj.transform.position, Quaternion.FromToRotation(Vector3.forward, impactDirection));
		Debug.Log (impactDirection);
		obj.gameObject.SetActive (false);
	}
	public void Die(Vector3 impactDirection){
		Debug.Log (impactDirection);
		Instantiate (playerDeathFx, transform.position, Quaternion.FromToRotation(Vector3.forward, impactDirection));
		this.gameObject.SetActive (false);
	}

    void OnDrawGizmos() {
        if (!drawDebugGizmos) {
            return;
        }

        // Draw a gizmo at the center point for our current move. This is where
        // we raycast from etc.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(currentMoveCenter, 0.1f);

        // Draw the ray we use to check if there's a walkable node beneath our feet.
       Gizmos.color = Color.red;
        Gizmos.DrawRay(currentMoveRay);

        // Draw the ray we use to check if there is a wall in the direction we're
        // trying to move.
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallCheckRay);

		Gizmos.color = Color.green;
		Gizmos.DrawRay(checkObstacleRay);

    }
}