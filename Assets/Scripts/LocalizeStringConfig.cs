using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class LocalizeStringConfig : MonoBehaviour {

    [SerializeField]
    private Text primaryText;

    [SerializeField]
    private Text secondaryText;

    private LocalizeStringEvent primaryTextString;

    private LocalizeStringEvent secondaryTextString;

    #region Singleton Initialization

    private static LocalizeStringConfig _instance;

    public static LocalizeStringConfig Instance { get { return _instance; } }

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
        secondaryTextString = secondaryText.GetComponent<LocalizeStringEvent>();
    }

    public void SetPrimaryText(string entry) {
        primaryTextString.StringReference.SetReference("Primary Text", entry);
    }

    public void SetSecondaryText(string entry) {
        secondaryTextString.StringReference.SetReference("Secondary Text", entry);
    }
}
