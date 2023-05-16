using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_ : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "FireParticle") {
            Game_.instance.rule_.gameStates.ChangeState(GameStates.Lose);
            Debug.Log("Fire");
        }
    }
}
