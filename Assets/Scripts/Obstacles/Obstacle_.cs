using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_ : MonoBehaviour {
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
        }
    }
}
