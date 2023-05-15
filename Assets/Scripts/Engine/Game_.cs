using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_ : MonoBehaviour {
    public static Game_ instance;
    public Rule_ rule_;
    public Level_ level_;



    void Awake() {
        instance = this;
    }
}
