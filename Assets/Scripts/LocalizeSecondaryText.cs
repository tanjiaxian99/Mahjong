using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class LocalizeSecondaryText : MonoBehaviour {

    public int fanTotal;

    #region Singleton Initialization

    private static LocalizeSecondaryText _instance;

    public static LocalizeSecondaryText Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion
}
