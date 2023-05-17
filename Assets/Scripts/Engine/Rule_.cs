using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public enum GameStates { Start, Game, Win, Lose };
public class Rule_ : MonoBehaviour {
    public StateMachine<GameStates> gameStates;

    void Awake() {
        gameStates = StateMachine<GameStates>.Initialize(this);
        gameStates.ChangeState(GameStates.Game);
    }

    IEnumerator Lose_Enter() {
        
        yield return null;
    }


}
