using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player_ : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "FireParticle" || other.gameObject.tag == "Spike") {
            EventsManager_.instance.PlayerDeath();
        }
        if (other.gameObject.tag == "EndLevel") {
            Game_.instance.audio_.mainMusicAudioSource.Stop();
            Game_.instance.audio_.audioSource.PlayOneShot(Game_.instance.audio_.winGameClip);
            Lean.Pool.LeanPool.Spawn(Game_.instance.vFX_.confettiParticle, Game_.instance.endLevel.targetParticle.position, Quaternion.identity);
            Game_.instance.rule_.gameStates.ChangeState(GameStates.Win);
        }
    }
}
