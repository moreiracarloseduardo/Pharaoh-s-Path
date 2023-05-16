using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTrigger_ : MonoBehaviour {
    public ArrowSpawner_ arrowSpawner;
    public float buttonCooldown = 2f; // Tempo que o botão ficará desativado
    private bool buttonActive = true;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && arrowSpawner.cubeType == CubeType.Manual && buttonActive) {
            arrowSpawner.FireArrow();
            StartCoroutine(DeactivateButton());
        }
    }

    IEnumerator DeactivateButton() {
        buttonActive = false;
        yield return new WaitForSeconds(buttonCooldown);
        buttonActive = true;
    }
}

