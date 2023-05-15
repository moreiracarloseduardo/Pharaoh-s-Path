using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath_ : MonoBehaviour {
    public float speed = 2f;
    private LineRenderer lineRenderer;
    private List<Vector3> pathPositions = new List<Vector3>();
    private int currentPositionIndex = 0;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetMouseButton(0)) {
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

        if (pathPositions.Count > 0) {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(pathPositions[currentPositionIndex].x, transform.position.y, pathPositions[currentPositionIndex].z), speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, new Vector3(pathPositions[currentPositionIndex].x, transform.position.y, pathPositions[currentPositionIndex].z)) < 0.1f) {
                currentPositionIndex++;
            }

            if (currentPositionIndex >= pathPositions.Count) {
                currentPositionIndex = pathPositions.Count - 1;
            }
        }
    }
}
