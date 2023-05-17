using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager_ : MonoBehaviour {
    public static EventsManager_ instance;
    public event Action OnPlayerDeath;
    public delegate void InkAction(float amount);
    public event InkAction OnInkUsed;
    public event InkAction OnInkRefilled;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    public void PlayerDeath() {
        OnPlayerDeath?.Invoke();
    }
    
}

