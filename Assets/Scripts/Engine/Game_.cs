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
    public Audio_ audio_;
    public EndLevel_ endLevel;

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

    public float inkUsageRate {
        get {
            if (level_ != null && level_.currentLevel != null) {
                return level_.currentLevel.inkUsageRate;
            } else {
                return 1;  
            }
        }
    }

    void Awake() {
        instance = this;
    }
    void Start() {
        SetRefreshRate();
        uI_.UpdateInkBar(inkAmount / 10);
        EventsManager_.instance.OnPlayerDeath += HandlePlayerDeath;
        EventsManager_.instance.OnGameStart += HandleGameStart;
        EventsManager_.instance.OnNextLevel += HandleNextLevel;
        EventsManager_.instance.OnGameRestart += HandleGameRestart;
        EventsManager_.instance.OnPlayerDeath += HandlePlayerDeath;
    }
    void SetRefreshRate() {
        Resolution[] resolutions = Screen.resolutions;
        int maxRefreshRate = 60;
        foreach (var res in resolutions) {
            if (res.refreshRate > maxRefreshRate) maxRefreshRate = res.refreshRate;
        }
        Application.targetFrameRate = maxRefreshRate;
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

}
