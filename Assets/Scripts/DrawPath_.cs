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
    private bool canDraw = false;
    private List<Vector3> pathPositions = new List<Vector3>();
    private int currentPositionIndex = 0;
    private Animator animator;
    private StateMachine<PlayerStates> playerStates;

    void Start() {
        animator = player.GetComponent<Animator>();
        playerStates = StateMachine<PlayerStates>.Initialize(this, PlayerStates.Idle);
        Game_.instance.rule_.gameStates.Changed += HandleGameStateChange;
    }
    void HandleGameStateChange(GameStates state) {
        switch (state) {
            case GameStates.Game:
                canDraw = true;
                break;
            case GameStates.Start:
            case GameStates.Lose:
                canDraw = false;
                break;
        }
    }

    void Idle_Enter() {
        pathPositions.Clear();
        currentPositionIndex = 0;
        lineRenderer.positionCount = 0;
    }

    void Idle_Update() {
        if (canDraw && Input.GetMouseButtonDown(0)) {
            Debug.Log("Changing to Drawing state");
            playerStates.ChangeState(PlayerStates.Drawing);
        }
    }

    void Drawing_Enter() {
        Debug.Log("Entered Drawing state");
    }

    void Drawing_Update() {
        if (canDraw && Game_.instance.rule_.gameStates.State == GameStates.Game) {
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
            if (Input.GetMouseButton(0)) {
                Game_.instance.inkAmount -= Game_.instance.inkUsageRate * Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0)) {
                playerStates.ChangeState(PlayerStates.Moving);
            }
        }
    }

    void Drawing_Exit() { }

    void Moving_Enter() { }

    void Moving_Update() {
        if (pathPositions.Count > 0 && currentPositionIndex < pathPositions.Count) {
            if (Game_.instance.rule_.gameStates.State == GameStates.Game) {
                MovePharaoh();
            }
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
