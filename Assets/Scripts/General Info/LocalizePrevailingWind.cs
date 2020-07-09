using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizePrevailingWind : MonoBehaviour {

    private string prevailingWind;

    public string PrevailingWind {
        get { return prevailingWind; }
        set {
            var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Info", value);
            prevailingWind = op.Result;
        }
    }

    #region Singleton Initialization

    private static LocalizePrevailingWind _instance;

    public static LocalizePrevailingWind Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion

}
