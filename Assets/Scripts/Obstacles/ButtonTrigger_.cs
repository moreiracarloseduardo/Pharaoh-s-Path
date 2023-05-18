using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger_ : MonoBehaviour {
    public ArrowSpawner_ arrowSpawner;
    public GameObject buttonTrigger;
    public float buttonCooldown = 2f;
    private bool buttonActive = true;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && arrowSpawner.cubeType == CubeType.Manual && buttonActive) {
            arrowSpawner.FireArrow();
            StartCoroutine(DeactivateButton());
        }
    }

    IEnumerator DeactivateButton() {
        Vector3 originalPos = buttonTrigger.transform.position;

        Vector3 newPos = originalPos + new Vector3(0, -0.1f, 0);

        LeanTween.move(buttonTrigger, newPos, 0.2f);
        Game_.instance.audio_.audioSource.PlayOneShot(Game_.instance.audio_.buttonArrowTriggerClip);

        buttonActive = false;
        yield return new WaitForSeconds(buttonCooldown);

        LeanTween.move(buttonTrigger, originalPos, 0.2f);

        yield return new WaitForSeconds(0.2f); 

        buttonActive = true;
    }
}

