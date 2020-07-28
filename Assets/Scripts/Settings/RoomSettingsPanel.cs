using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
using UnityEngine.UI;
using System.Xml.Serialization;

public class RoomSettingsPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject roomSettingsPanel;

    [SerializeField]
    private Transform content;

    private Dictionary<string, int> settings;

    public override void OnEnable() {
        base.OnEnable();
        settings = PropertiesManager.GetRoomSettings();

        foreach (Transform child in content) {
            string settingsName = child.name;
            child.GetComponent<RoomSetting>().settingsName = settingsName;

            Slider slider = child.GetComponentInChildren<Slider>();
            slider.value = settings[settingsName];

            if (PhotonNetwork.IsMasterClient) {
                slider.interactable = true;
            } else {
                slider.interactable = false;
            }
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
        if (propertiesThatChanged.ContainsKey(PropertiesManager.ChangedRoomSettingPropKey)) {
            Dictionary<string, int> changedRoomSetting = PropertiesManager.GetChangedRoomSetting();
            string settingsName = changedRoomSetting.Keys.First();
            int value = changedRoomSetting.Values.First();

            if (PhotonNetwork.IsMasterClient) {
                settings[settingsName] = value;
                return;
            }

            Transform roomSetting = content.Find(settingsName);
            Slider slider = roomSetting.GetComponentInChildren<Slider>();
            slider.value = value;
        }
    }

    public void OnClickBackToRoom() {
        PropertiesManager.UpdateRoomSettings(settings);
        roomSettingsPanel.SetActive(false);
        roomPanel.SetActive(true);
    }
}
