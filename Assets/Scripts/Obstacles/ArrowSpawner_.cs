using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CubeType {
    Manual, 
    Automatic 
}


public class ArrowSpawner_ : MonoBehaviour {
    public Arrow_ arrowPrefab;
    public CubeType cubeType;
    public float fireInterval = 5f;
    public Quaternion arrowRotation;
    private float timer;
    private bool isFiring = false;

    private void Start() {
        if (cubeType == CubeType.Automatic) {
            timer = fireInterval;
        }
    }

    private void Update() {
        if (cubeType == CubeType.Automatic) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                FireArrow();
                timer = fireInterval;
            }
        }
    }

    public void FireArrow() {
        if (isFiring) return;

        isFiring = true;
        Arrow_ arrow = Instantiate(arrowPrefab, transform.position, arrowRotation); 
        arrow.Fire();
        isFiring = false;
    }

}
