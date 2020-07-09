using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizeTilesLeft : MonoBehaviour {

    public int TilesLeft { get; set; }

    #region Singleton Initialization

    private static LocalizeTilesLeft _instance;

    public static LocalizeTilesLeft Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    #endregion
}
