using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UpdateSliderValue : MonoBehaviour {

    [SerializeField]
    private Slider slider;

    public int value;

    public LocalizedString myString;

    [SerializeField]
    private Text localizedText;

    private void OnEnable() {
        myString.Arguments = new object[] { this };
        myString.RegisterChangeHandler(UpdateString);
    }

    public void OnSliderChanged() {
        value = (int)slider.value;
        OnEnable();
        myString.RefreshString();
    }

    void UpdateString(string s) {
        localizedText.text = s;
    }

    private void OnDisable() {
        myString.ClearChangeHandler();
    }
}
