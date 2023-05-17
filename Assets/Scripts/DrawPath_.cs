using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
public enum PlayerStates { Idle, Drawing, Moving };

public class DrawPath_ : MonoBehaviour {

    public GameObject player;
    public LineRenderer lineRenderer;

    public float speed = 2f;
    public float eraseDelay = 1f;
    private List<Vector3> pathPositions = new List<Vector3>();
    private int currentPositionIndex = 0;
    private Animator animator;
    private StateMachine<PlayerStates> playerStates;

    void Start() {
        animator = player.GetComponent<Animator>();
        playerStates = StateMachine<PlayerStates>.Initialize(this, PlayerStates.Idle);
    }

    void Idle_Enter() {
        pathPositions.Clear();
        currentPositionIndex = 0;
        lineRenderer.positionCount = 0;
    }

    void Idle_Update() {
        if (Input.GetMouseButtonDown(0)) {
            playerStates.ChangeState(PlayerStates.Drawing);
        }
    }

    void Drawing_Enter() {

    }

    void Drawing_Update() {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f)) {
            if (hit.collider.gameObject.CompareTag("Ground")) {
                Vector3 adjustedHitPoint = hit.point + new Vector3(0, .1f, 0);
                pathPositions.Add(adjustedHitPoint);
                lineRenderer.positionCount = pathPositions.Count;
                lineRenderer.SetPosition(pathPositions.Count - 1, adjustedHitPoint);
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            playerStates.ChangeState(PlayerStates.Moving);
        }
    }

    void Drawing_Exit() { }

    void Moving_Enter() { }

    void Moving_Update() {
        if (pathPositions.Count > 0 && currentPositionIndex < pathPositions.Count) {
            MovePharaoh();
        }

        if (currentPositionIndex == pathPositions.Count) {
            float travelTime = CalculatePathTravelTime();
            StartCoroutine(ErasePathAfterDelay(travelTime));
        }
    }

    void Moving_Exit() { }

    private float CalculatePathTravelTime() {
        float totalDistance = 0f;

        for (int i = 0; i < pathPositions.Count - 1; i++) {
            totalDistance += Vector3.Distance(pathPositions[i], pathPositions[i + 1]);
        }

        return totalDistance / speed;
    }

    void MovePharaoh() {
        Vector3 targetPosition = new Vector3(pathPositions[currentPositionIndex].x, player.transform.position.y, pathPositions[currentPositionIndex].z);

        Vector3 directionToTarget = (targetPosition - player.transform.position).normalized;

        if (directionToTarget != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, speed * Time.deltaTime);
        }

        player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(player.transform.position, targetPosition) < 0.1f) {
            currentPositionIndex++;
            if (currentPositionIndex >= pathPositions.Count) {
                currentPositionIndex = pathPositions.Count - 1;
                animator.SetBool("IsMoving", false);
                playerStates.ChangeState(PlayerStates.Idle);
            }
        } else {
            animator.SetBool("IsMoving", true);
        }
    }
    IEnumerator ErasePathAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);

        while (pathPositions.Count > 0) {
            pathPositions.RemoveAt(0);
            lineRenderer.positionCount--;
            for (int i = 0; i < pathPositions.Count; i++) {
                lineRenderer.SetPosition(i, pathPositions[i]);
            }
            yield return null;
        }

        currentPositionIndex = 0;
    }
}
