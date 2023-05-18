using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel_ : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Game_.instance.rule_.gameStates.ChangeState(GameStates.Win);
        }
    }
}
