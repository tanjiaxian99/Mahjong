using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class KongManager : MonoBehaviour {
    #region Serialize Field

    [SerializeField]
    private GameObject PongCombo;

    [SerializeField]
    private GameObject KongComboZero;

    [SerializeField]
    private GameObject KongComboOne;

    [SerializeField]
    private GameObject KongComboTwo;

    [SerializeField]
    private GameObject scriptManager;

    #endregion

    private GameManager gameManager;

    private PlayerManager playerManager;

    private TilesManager tilesManager;

    private PayAllDiscard payAllDiscard;

    private WinManager winManager;

    private void Start() {
        gameManager = scriptManager.GetComponent<GameManager>();
        playerManager = scriptManager.GetComponent<PlayerManager>();
        tilesManager = scriptManager.GetComponent<TilesManager>();
        payAllDiscard = scriptManager.GetComponent<PayAllDiscard>();
        winManager = scriptManager.GetComponent<WinManager>();
    }


    /// <summary>
    /// Called when the player can Kong
    /// </summary>
    public void KongUI(List<Tile> kongTiles) {

        for (int i = 0; i < kongTiles.Count; i++) {

            // TODO: Might be better to implement a dictionary
            GameObject kongComboGameObject;
            if (i == 0) {
                kongComboGameObject = KongComboZero;
            } else if (i == 1) {
                kongComboGameObject = KongComboOne;
            } else {
                kongComboGameObject = KongComboTwo;
            }

            Transform spritesPanel = kongComboGameObject.transform.GetChild(0);

            // Instantiate the tile sprites
            for (int j = 0; j < 4; j++) {
                Transform imageTransform = spritesPanel.GetChild(j);
                Image image = imageTransform.GetComponent<Image>();
                image.sprite = DictManager.Instance.spritesDict[kongTiles[i]];
            }
            kongComboGameObject.SetActive(true);
        }

        // Disable the ability to interact with hand tiles
        playerManager.canTouchHandTiles = false;
    }


    /// <summary>
    /// Called when "Ok" is clicked for Kong Combo
    /// </summary>
    public void OnKongOk() {
        // Check if the discard tile is a high risk discard
        if (payAllDiscard.shouldPayForAll(playerManager, tilesManager, gameManager.prevailingWind, gameManager.latestDiscardTile, "Kong")) {
            PropertiesManager.SetPayAllPlayer(gameManager.discardPlayer);
        }

        PongCombo.SetActive(false);
        KongComboZero.SetActive(false);
        KongComboOne.SetActive(false);
        KongComboTwo.SetActive(false);

        List<Tile> hand = tilesManager.hand;
        List<Tile> openTiles = tilesManager.openTiles;
        Tile drawnTile = hand[hand.Count - 1];

        GameObject button = EventSystem.current.currentSelectedGameObject;
        GameObject kongComboGameObject = button.transform.parent.parent.gameObject;
        Transform spritesPanel = kongComboGameObject.transform.GetChild(0);
        Transform imageTransform = spritesPanel.GetChild(0);
        Image image = imageTransform.GetComponent<Image>();
        string spriteName = image.sprite.name;
        Tile kongTile = new Tile(spriteName);

        // Going through possibilities of Discard Kong, Exposed Kong and Concealed Kong
        if (tilesManager.CanDiscardKong(kongTile)) {

            // Update MasterClient that the player wants to Kong the discard tile
            EventsManager.EventCanPongKong(true);

            // Update discard tile properties to indicate to all players to remove the latest discard tile
            PropertiesManager.SetDiscardTile(new Tuple<int, Tile, float>(-1, new Tile(0, 0), 0));

            List<Tile> combo = new List<Tile>();
            for (int i = 0; i < 3; i++) {
                // Important! We cannot just add kongTile directly to combo, as that will result in 3 tiles with the same reference. Down the road,
                // when winCombos set one of the tiles' isWinning to true, all 3 tiles will have isWinning == true. This will result in Eye combo having 3 tiles
                Tile temp = new Tile(kongTile.suit, kongTile.rank);
                combo.Add(temp);
                tilesManager.hand.Remove(kongTile);
            }
            Tile markedTile = new Tile(kongTile.suit, kongTile.rank);
            markedTile.kongType = 1;
            combo.Add(markedTile);
            tilesManager.comboTiles.Add(combo);

        } else if (tilesManager.ExposedKongTiles().Contains(kongTile)) {
            foreach (List<Tile> combo in tilesManager.comboTiles) {
                if (combo.Contains(drawnTile)) {
                    drawnTile.kongType = 2;
                    combo.Add(drawnTile);
                }
            }
            hand.Remove(drawnTile);

            // Update discard tile properties to indicate to all players that Robbing the Kong is possible
            PropertiesManager.SetSpecialTile(new Tuple<int, Tile, float>(PhotonNetwork.LocalPlayer.ActorNumber, drawnTile, 2));

        } else if (tilesManager.ConcealedKongTiles().Contains(kongTile)) {
            // The second-last tile will be instantiated above the 3 other Kong tiles
            Tile kongTileSpecial = new Tile(spriteName);
            kongTileSpecial.kongType = 3;
            List<Tile> combo = new List<Tile>();

            combo.Add(new Tile(spriteName));
            combo.Add(new Tile(spriteName));
            combo.Add(kongTileSpecial);
            combo.Add(new Tile(spriteName));

            for (int i = 0; i < 4; i++) {
                tilesManager.hand.Remove(kongTile);
            }

            tilesManager.comboTiles.Add(combo);

            // Update kong tile properties to indicate to all players that Robbing the Kong is possible for Thirteen Wonders
            PropertiesManager.SetSpecialTile(new Tuple<int, Tile, float>(PhotonNetwork.LocalPlayer.ActorNumber, kongTileSpecial, 3));
        }

        playerManager.numberOfKong++;
        playerManager.InstantiateLocalOpenTiles();

        // Always draw a tile regardless of Kong type
        hand.Add(playerManager.DrawTile());
        gameManager.latestDiscardTile = null;
        gameManager.discardPlayer = null;

        playerManager.ConvertLocalBonusTiles();
        playerManager.InstantiateLocalHand();
        playerManager.InstantiateLocalOpenTiles();

        // Return the ability to interact with hand tiles
        playerManager.canTouchHandTiles = true;
        playerManager.myTurn = true;

        // Check to see if the player can win based on the discard tile
        if (winManager.CanWin()) {
            winManager.WinUI();
            return;
        }

        if (tilesManager.ExposedKongTiles().Count != 0 || tilesManager.ConcealedKongTiles().Count != 0) {
            this.KongUI(tilesManager.ExposedKongTiles().Concat(tilesManager.ConcealedKongTiles()).ToList());
        }
    }


    /// <summary>
    /// Called when "Skip" button is clicked for Kong Combo
    /// </summary>
    public void OnKongSkip() {
        List<Tile> hand = tilesManager.hand;
        List<Tile> openTiles = tilesManager.openTiles;
        Tile drawnTile = hand[hand.Count - 1];

        if (!playerManager.myTurn) {
            // Update MasterClient that the player doesn't want to Pong. Only applicable for 3 concealed tiles Kong.
            EventsManager.EventCanPongKong(false);
        }

        PongCombo.SetActive(false);
        KongComboZero.SetActive(false);
        KongComboOne.SetActive(false);
        KongComboTwo.SetActive(false);

        // Return the ability to interact with hand tiles only for 1 and 4 concealed tiles Kong.
        if (tilesManager.ExposedKongTiles().Count != 0 || tilesManager.ConcealedKongTiles().Count != 0) {
            playerManager.canTouchHandTiles = true;
        }
    }
}
