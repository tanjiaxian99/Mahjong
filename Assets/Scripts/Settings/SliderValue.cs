using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {

    [SerializeField]
    private Slider slider;

    public int value;

    public LocalizeStringEvent stringEvent;

    public void OnSliderChanged() {
        value = (int)slider.value;
        stringEvent.StringReference.RefreshString();
    }
}
