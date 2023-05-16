using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTotem_ : MonoBehaviour {
    public GameObject fireParticleObject;
    public ParticleSystem fireParticles;
    public GameObject totemCollider;
    public float interval = 2f;
    private bool isActive = false;

    void Start() {
        fireParticleObject.SetActive(false);
        StartCoroutine(FireLoop());
        totemCollider.SetActive(false);
    }

    IEnumerator FireLoop() {
        while (true) {
            yield return new WaitForSeconds(interval);

            if (isActive) {
                fireParticles.Stop();
                fireParticleObject.SetActive(false);
                isActive = false;
                totemCollider.SetActive(false);
            } else {
                fireParticles.Play();
                fireParticleObject.SetActive(true);
                isActive = true;
                totemCollider.SetActive(true);
            }
        }
    }

}
