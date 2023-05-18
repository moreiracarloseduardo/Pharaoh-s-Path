using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ : MonoBehaviour {
    public Slider inkBar;
    public GameObject StartUi;
    public GameObject inGameUi;
    public GameObject winUi;
    public GameObject loseUi;
    public Button startButton;
    public Button nextButton;
    public Button restartButton;
    public void UpdateInkBar(float percentage) {
        inkBar.value = percentage;
    }
    void Start() {
        startButton.onClick.AddListener(EventsManager_.instance.GameStart);
        nextButton.onClick.AddListener(EventsManager_.instance.NextLevel);
        restartButton.onClick.AddListener(EventsManager_.instance.GameRestart);
    }
}
