using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon_ : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Player atingido pela arma");
            // Implemente aqui o código para matar o jogador.
        }
    }
}
