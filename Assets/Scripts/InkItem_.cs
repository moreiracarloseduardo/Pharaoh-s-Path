using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InkItem_ : MonoBehaviour {
    public static event Action OnInkItemPicked;
    public float refillAmount = 1;
    public float rotationSpeed = 50f; 
    public float floatSpeed = 0.5f; 
    public float floatRange = 0.5f; 

    private Vector3 initialPosition;

    void Start() {
        initialPosition = transform.position;
        LeanTween.moveY(gameObject, initialPosition.y + floatRange, floatSpeed).setEase(LeanTweenType.pingPong);
    }

    void Update() {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Game_.instance.inkAmount += refillAmount;
            Lean.Pool.LeanPool.Spawn(Game_.instance.vFX_.inkFillParticle, transform.position, Quaternion.identity);
            Game_.instance.audio_.audioSource.PlayOneShot(Game_.instance.audio_.inkFillClip);
            OnInkItemPicked?.Invoke();
            Destroy(gameObject);
        }
    }
}
