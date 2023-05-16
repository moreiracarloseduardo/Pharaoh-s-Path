using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_ : MonoBehaviour {
    public float speed = 10f;
    public float distance = 10f;
    public Vector3 direction = Vector3.forward;
    private Rigidbody rb;
    public GameObject arrowPrefab;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire() {
        LeanTween.move(gameObject, transform.position - direction * distance, distance / speed)
       .setOnComplete(() => {
           Destroy(gameObject);
       });
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            Game_.instance.rule_.gameStates.ChangeState(GameStates.Lose);
            rb.isKinematic = false;
            Debug.Log("Acertou");
        }
    }
}
