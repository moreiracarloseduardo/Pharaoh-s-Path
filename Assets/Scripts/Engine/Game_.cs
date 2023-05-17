using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_ : MonoBehaviour {
    public static Game_ instance;
    public Rule_ rule_;
    public Level_ level_;
    public Player_ player_;
    public UI_ UI_;


    private float _inkAmount = 10; 
    public float inkAmount {
        get { return _inkAmount; }
        set {
            _inkAmount = Mathf.Clamp(value, 0, 100); 
            UI_.UpdateInkBar(_inkAmount / 10);  
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
        UI_.UpdateInkBar(inkAmount / 10);
        EventsManager_.instance.OnPlayerDeath += HandlePlayerDeath;
        // EventsManager_.instance.OnInkUsed += UpdateInkAmount;
        // EventsManager_.instance.OnInkRefilled += UpdateInkAmount;
    }
    void HandlePlayerDeath() {
        rule_.gameStates.ChangeState(GameStates.Lose);
        player_.GetComponent<Animator>().SetTrigger("Die");
    }
    void UpdateInkAmount(float amount) {
        inkAmount += amount;
        UI_.UpdateInkBar(inkAmount);
        if (inkAmount <= 0) {
            HandlePlayerDeath();
        }
    }
}
