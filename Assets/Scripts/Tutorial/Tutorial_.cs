using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
public enum TutorialStates { Start, Tutorial01, Tutorial02, Tutorial03, End };
public class Tutorial_ : MonoBehaviour {
    public GameObject handTutorial;
    public StateMachine<TutorialStates> tutorialStates;
    [Header("Tutorial01")]
    public Transform startPoint1;
    public Transform endPoint1;
    public GameObject textTutorial01;
    [Header("Tutorial02")]
    public Transform startPoint2;
    public Transform endPoint2;
    public GameObject textTutorial02;

    [Header("Tutorial03")]
    public Transform startPoint3;
    public Transform endPoint3;
    public GameObject textTutorial03;

    private bool tutorialComplete;
    private LTDescr handAnimation;

    void Start() {
        tutorialComplete = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;
        tutorialStates = StateMachine<TutorialStates>.Initialize(this, TutorialStates.Start);
        InkItem_.OnInkItemPicked += () => {
            if (tutorialStates.State == TutorialStates.Tutorial02)
                tutorialStates.ChangeState(TutorialStates.Tutorial03);
        };

        Game_.instance.rule_.gameStates.Changed += (state) => {
            if (state == GameStates.Game && !tutorialComplete) {
                tutorialStates.ChangeState(TutorialStates.Tutorial01);
                handTutorial.SetActive(true);
            }
        };
    }

    IEnumerator Tutorial01_Enter() {
        handTutorial.transform.position = startPoint1.position;
        StartHandAnimation(startPoint1, endPoint1);
        yield return new WaitForSeconds(1);
        textTutorial01.SetActive(true);
        textTutorial02.SetActive(false);
        textTutorial03.SetActive(false);

    }

    void Tutorial01_Update() {
        if (Game_.instance.player_.GetComponent<DrawPath_>().playerStates.State == PlayerStates.Moving) {
            if (handAnimation != null) LeanTween.cancel(handAnimation.id);
            tutorialStates.ChangeState(TutorialStates.Tutorial02);
        }
    }

    IEnumerator Tutorial02_Enter() {
        StartHandAnimation(startPoint2, endPoint2);
        yield return new WaitForSeconds(1);
        textTutorial01.SetActive(false);
        textTutorial02.SetActive(true);
        textTutorial03.SetActive(false);
    }

    IEnumerator Tutorial03_Enter() {
        StartHandAnimation(startPoint3, endPoint3);
        yield return new WaitForSeconds(1);
        textTutorial01.SetActive(false);
        textTutorial02.SetActive(false);
        textTutorial03.SetActive(true);
    }

    void Tutorial03_Update() {
        if (Game_.instance.rule_.gameStates.State == GameStates.Win) {
            tutorialStates.ChangeState(TutorialStates.End);
        }
    }

    void End_Enter() {
        PlayerPrefs.SetInt("TutorialComplete", 1);
        PlayerPrefs.Save();
        tutorialComplete = true;
    }

    private void StartHandAnimation(Transform startPoint, Transform endPoint) {
        if (handAnimation != null) LeanTween.cancel(handAnimation.id);
        handTutorial.transform.position = startPoint.position;
        handAnimation = LeanTween.move(handTutorial, endPoint.position, 2f).setOnComplete(() => {
            handTutorial.transform.position = startPoint.position;
            StartHandAnimation(startPoint, endPoint);
        });
    }

}
