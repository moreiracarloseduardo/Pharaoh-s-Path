using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkItem_ : MonoBehaviour {
    public float refillAmount = 1;  

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Game_.instance.inkAmount += refillAmount;
            Destroy(gameObject);  
        }
    }
}
