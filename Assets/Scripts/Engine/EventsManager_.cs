using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager_ : MonoBehaviour {
    public static EventsManager_ instance;
    public event Action OnPlayerDeath;
    public event Action OnGameStart;
    public event Action OnNextLevel;
    public event Action OnGameRestart;
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
    public void GameStart() {
        OnGameStart?.Invoke();
    }

    public void NextLevel() {
        OnNextLevel?.Invoke();
    }

    public void GameRestart() {
        OnGameRestart?.Invoke();
    }

    public void PlayerDeath() {
        OnPlayerDeath?.Invoke();
    }

}

