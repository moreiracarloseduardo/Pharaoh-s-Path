using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_ : MonoBehaviour {
    public static Game_ instance;
    public Rule_ rule_;
    public Level_ level_;
    public Player_ player_;



    void Awake() {
        instance = this;
        EventsManager_.instance.OnPlayerDeath += HandlePlayerDeath;
    }
    void HandlePlayerDeath() {
        rule_.gameStates.ChangeState(GameStates.Lose);
        player_.GetComponent<Animator>().SetTrigger("Die");
    }
}
