using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ : MonoBehaviour {
    public Slider inkBar;
    public void UpdateInkBar(float percentage) {
        inkBar.value = percentage;
    }
}
