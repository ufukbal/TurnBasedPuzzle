using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public bool drawDebugGizmos = false;
	[SerializeField]
    float timeToMove = 1f;

    Vector3 invalidPosition = new Vector3(9999, 9999, 9999);
    Vector3 positionToMoveTo;
    Vector3 currentMove;
    Vector3 checkDirection;
    bool checkedDiagonal = false;
    bool isMoving = false;
	bool isGoal = false;
    Vector3 transformUp;
    Vector3 currentMoveCenter;
    Ray currentMoveRay;
    Ray wallCheckRay;

    void Start() {
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
                StartCoroutine("PerformMove");
            }
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            ResetChecks();
            currentMove = transform.right * -1;
            Move(currentMove);
            if (positionToMoveTo != invalidPosition) {
                StartCoroutine("PerformMove");
            }
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            ResetChecks();
            currentMove = transform.forward * -1;
            Move(currentMove);
            if (positionToMoveTo != invalidPosition) {
                StartCoroutine("PerformMove");
            }
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            ResetChecks();
            currentMove = transform.right;
            Move(currentMove);
            if (positionToMoveTo != invalidPosition) {
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
        isMoving = false;
		//do ai movement
		if (isGoal)
			Debug.Log ("yey");
    }

    void Move(Vector3 offset) {
        Vector3 pointToCheck = transform.position + offset;

        currentMoveCenter = pointToCheck;
        currentMoveRay = new Ray(currentMoveCenter, checkDirection);

        RaycastHit hit;
        if (Physics.Raycast(currentMoveRay, out hit, 1f)) {
            if (hit.collider.tag == "Walkable") {
                transformUp = hit.normal;
                positionToMoveTo = pointToCheck;
            }
			if (hit.collider.tag == "Goal") {
				transformUp = hit.normal;
				positionToMoveTo = pointToCheck;
				isGoal = true;
			}
        }

        if (positionToMoveTo != invalidPosition) {
            return;
        }
//
        if (WallCheck()) {
			Debug.Log ("wall check");
            StartCoroutine("ChangeOrientation");
            return;
        }
        else if (!checkedDiagonal) {
            checkedDiagonal = true;
            checkDirection = currentMove * -1;
			Debug.Log ("diagonal");
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
        }

        return false;
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
    }
}
