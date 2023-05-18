using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_ : MonoBehaviour {
    public static Game_ instance;
    public Rule_ rule_;
    public Level_ level_;
    public Player_ player_;
    public UI_ uI_;
    public VFX_ vFX_;


    private float _inkAmount = 10;
    public float inkAmount {
        get { return _inkAmount; }
        set {
            _inkAmount = Mathf.Clamp(value, 0, 100);
            uI_.UpdateInkBar(_inkAmount / 10);
            if (_inkAmount <= 0) {
                HandlePlayerDeath();
            }
        }
    }

    public float inkUsageRate = 1;

    void Awake() {
        instance = this;
    }
    void Start() {
        uI_.UpdateInkBar(inkAmount / 10);
        EventsManager_.instance.OnPlayerDeath += HandlePlayerDeath;
        EventsManager_.instance.OnGameStart += HandleGameStart;
        EventsManager_.instance.OnNextLevel += HandleNextLevel;
        EventsManager_.instance.OnGameRestart += HandleGameRestart;
        EventsManager_.instance.OnPlayerDeath += HandlePlayerDeath;
    }
    void HandleGameStart() {
        rule_.gameStates.ChangeState(GameStates.Game);
    }

    void HandleNextLevel() {
        Game_.instance.level_.EndLevel(true, 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HandleGameRestart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void HandlePlayerDeath() {
        rule_.gameStates.ChangeState(GameStates.Lose);
        player_.GetComponent<Animator>().SetTrigger("Die");
    }
    // void UpdateInkAmount(float amount) {
    //     inkAmount += amount;
    //     uI_.UpdateInkBar(inkAmount);
    //     if (inkAmount <= 0) {
    //         HandlePlayerDeath();
    //     }
    // }
}
