using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath_ : MonoBehaviour {
    public float speed = 2f;
    public float eraseDelay = 1f;
    private LineRenderer lineRenderer;
    private List<Vector3> pathPositions = new List<Vector3>();
    private int currentPositionIndex = 0;
    private bool isDrawing = false;
    private bool isErasingPath = false;
    Animator animator;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            pathPositions.Clear();
            currentPositionIndex = 0;
            lineRenderer.positionCount = 0;
        }
        if (Input.GetMouseButton(0)) {
            isDrawing = true;
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f)) {
                if (hit.collider.gameObject.CompareTag("Ground")) {
                    pathPositions.Add(hit.point);
                    lineRenderer.positionCount = pathPositions.Count;
                    lineRenderer.SetPosition(pathPositions.Count - 1, hit.point);
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            isDrawing = false;
        }

        if (pathPositions.Count > 0 && currentPositionIndex < pathPositions.Count) {
            MovePharaoh();
        }

        if (!isDrawing && !isErasingPath && currentPositionIndex == pathPositions.Count) {
            float travelTime = CalculatePathTravelTime();
            StartCoroutine(ErasePathAfterDelay(travelTime));
        }
    }
    private float CalculatePathTravelTime() {
        float totalDistance = 0f;

        for (int i = 0; i < pathPositions.Count - 1; i++) {
            totalDistance += Vector3.Distance(pathPositions[i], pathPositions[i + 1]);
        }

        return totalDistance / speed;
    }

    void MovePharaoh() {
        Vector3 targetPosition = new Vector3(pathPositions[currentPositionIndex].x, transform.position.y, pathPositions[currentPositionIndex].z);

        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f) {
            currentPositionIndex++;
            if (currentPositionIndex >= pathPositions.Count) {
                currentPositionIndex = pathPositions.Count - 1;
                animator.SetBool("IsMoving", false);
            }
        } else {
            animator.SetBool("IsMoving", true);
        }
    }

    IEnumerator ErasePathAfterDelay(float delay) {
        isErasingPath = true;
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
        isErasingPath = false;
    }
}
