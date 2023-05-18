using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDespawnParticle_ : MonoBehaviour {
    // Start is called before the first frame update
    void OnEnable() {
        Invoke("Despawn", GetComponent<ParticleSystem>().main.duration);

    }

    void Despawn() {
        Lean.Pool.LeanPool.Despawn(this);
    }
}
