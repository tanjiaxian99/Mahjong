using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
using UnityEngine.UI;
using System.Xml.Serialization;
using UnityEngineInternal;

public class RoomSettingsPanel : MonoBehaviourPunCallbacks {

    [SerializeField]
    private GameObject roomPanel;

    [SerializeField]
    private GameObject roomSettingsPanel;

    [SerializeField]
    private Transform content;

    private Dictionary<string, int> settings;

    private List<string> maxValueSettings;

    private void Awake() {
        InitializeMaxValueSettings();
    }

    public override void OnEnable() {
        base.OnEnable();
        settings = PropertiesManager.GetRoomSettings();
        int maxValue = settings["Fan Limit"];

        foreach (Transform child in content) {            
            Slider slider = child.GetComponentInChildren<Slider>();
            if (PhotonNetwork.IsMasterClient) {
                slider.interactable = true;
            } else {
                slider.interactable = false;
            }

            string settingsName = child.name;
            child.GetComponent<RoomSetting>().settingsName = settingsName;

            if (child.name != "Fan Limit") {
                slider.maxValue = maxValue;
            }

            slider.value = settings[settingsName];

            //if (maxValueSettings.Contains(child.name)) {
            //    slider.value = maxValue;
            //}
            
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {

        if (propertiesThatChanged.ContainsKey(PropertiesManager.ChangedRoomSettingPropKey)) {
            Dictionary<string, int> changedRoomSetting = PropertiesManager.GetChangedRoomSetting();
            string settingsName = changedRoomSetting.Keys.First();
            int value = changedRoomSetting.Values.First();

            if (settingsName == "Fan Limit") {
                foreach (Transform child in content) {
                    UpdateValues(child, value);
                }
            }

            if (PhotonNetwork.IsMasterClient) {
                settings[settingsName] = value;
                PropertiesManager.UpdateRoomSettings(settings);
                return;
            }
            
            Transform roomSetting = content.Find(settingsName);
            Slider slider = roomSetting.GetComponentInChildren<Slider>();
            slider.value = value;

            //Debug.LogErrorFormat("Changed {0} to {1}", settingsName, value);
        }
    }

    private void UpdateValues(Transform child, int value) {
        if (child.name == "Fan Limit") {
            return;
        }

        Slider slider = child.GetComponentInChildren<Slider>();
        slider.maxValue = value;
        if (maxValueSettings.Contains(child.name)) {
            slider.value = value;
            settings[child.name] = value;
        }
    }

    public void OnClickBackToRoom() {
        roomSettingsPanel.SetActive(false);
        roomPanel.SetActive(true);
    }

    private void InitializeMaxValueSettings() {
        maxValueSettings = new List<string>() {
            "Heavenly Hand",
            "Earthly Hand",
            "Humanly Hand",
            "Robbing the Eighth",
            "All Flowers and Seasons",
            "Pure Terminals",
            "All Honour",
            "Hidden Treasure",
            "Full Flush Triplets",
            "Full Flush Full Sequence",
            "Nine Gates",
            "Four Great Blessings",
            "Three Great Scholars",
            "Eighteen Arhats",
            "Thirteen Wonders",
            "Kong on Kong"
        };
    }

    public override void OnDisable() {
        base.OnDisable();
    }
}
