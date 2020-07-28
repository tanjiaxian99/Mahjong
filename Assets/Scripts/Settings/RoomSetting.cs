using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEditor;
using UnityEngine.SocialPlatforms;
using Photon.Pun;

public class RoomSetting : MonoBehaviour {

    #region Fields

    [SerializeField]
    private GameObject roomSetting;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private LocalizeStringEvent stringEvent;

    public string settingsName;

    #endregion

    #region Methods

    private void OnEnable() {
        //TableEntryReference tableEntryReference = stringEvent.StringReference.TableEntryReference;
        //var locale = LocalizationSettings.AvailableLocales.Locales[0];

        //var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Winning Combos", tableEntryReference, locale);
        //if (op.IsDone) {
        //    SettingsName = op.Result;
        //} else {
        //    op.Completed += (x) => SettingsName = op.Result;
        //}

        //SettingsName = roomSetting.name;
    }

    public void UpdateSettings() {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        PropertiesManager.SetChangedRoomSetting(settingsName, (int)slider.value);
    }

    #endregion
}
