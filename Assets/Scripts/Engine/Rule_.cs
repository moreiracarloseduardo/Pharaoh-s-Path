using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public enum GameStates { Start, Game, Win, Lose };
public class Rule_ : MonoBehaviour {
    public StateMachine<GameStates> gameStates;

    void Awake() {
        gameStates = StateMachine<GameStates>.Initialize(this);
        gameStates.ChangeState(GameStates.Start);
    }

    void Start_Enter() {
        Game_.instance.uI_.StartUi.SetActive(true);
        Game_.instance.uI_.inGameUi.SetActive(false);
        Game_.instance.uI_.winUi.SetActive(false);
        Game_.instance.uI_.loseUi.SetActive(false);
    }

    void Game_Enter() {
        Game_.instance.uI_.StartUi.SetActive(false);
        Game_.instance.uI_.inGameUi.SetActive(true);
        Game_.instance.uI_.winUi.SetActive(false);
        Game_.instance.uI_.loseUi.SetActive(false);
    }

    void Win_Enter() {
        Game_.instance.uI_.StartUi.SetActive(false);
        Game_.instance.uI_.inGameUi.SetActive(false);
        Game_.instance.uI_.winUi.SetActive(true);
        Game_.instance.uI_.loseUi.SetActive(false);
    }

    IEnumerator Lose_Enter() {
        Game_.instance.uI_.StartUi.SetActive(false);
        Game_.instance.uI_.inGameUi.SetActive(false);
        Game_.instance.uI_.winUi.SetActive(false);
        Game_.instance.uI_.loseUi.SetActive(true);

        yield return null;
    }

}
