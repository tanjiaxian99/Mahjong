using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizePrimaryText : MonoBehaviour {

    [SerializeField]
    private Text primaryText;

    private LocalizeStringEvent primaryTextString;

    #region Singleton Initialization

    private static LocalizePrimaryText _instance;

    public static LocalizePrimaryText Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

    void Start() {
        primaryTextString = primaryText.GetComponent<LocalizeStringEvent>();
    }

    public void SetPrimaryText(string entry) {
        primaryTextString.StringReference.SetReference("Primary Text", entry);
    }
}
