using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel_ : MonoBehaviour {
    public Transform targetParticle;
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            // Game_.instance.audio_.mainMusicAudioSource.Stop();
            // Game_.instance.audio_.audioSource.PlayOneShot(Game_.instance.audio_.winGameClip);
            // Lean.Pool.LeanPool.Spawn(Game_.instance.vFX_.confettiParticle, targetParticle.position, Quaternion.identity);
            // Game_.instance.rule_.gameStates.ChangeState(GameStates.Win);
        }
    }
}
