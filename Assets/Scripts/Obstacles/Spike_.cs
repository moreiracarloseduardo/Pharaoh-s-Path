using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_ : MonoBehaviour {
    public void PlaySpikeSound() {
        if (!Game_.instance.audio_.spikeTrapAudioSource.isPlaying) {
            Game_.instance.audio_.spikeTrapAudioSource.Play();
        }
    }
}
