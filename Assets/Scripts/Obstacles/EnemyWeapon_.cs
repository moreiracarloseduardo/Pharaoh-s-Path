using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon_ : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            EventsManager_.instance.PlayerDeath();
        }
    }
}
