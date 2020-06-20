using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public static class RemotePlayer {

    /// <summary>
    /// Called by the remote player to instantiate the hand of remotePlayer on the local client.
    /// </summary>
    public static void InstantiateRemoteHand(GameManager gameManager, Player remotePlayer) {
        int remoteHandSize = PropertiesManager.GetHandTilesCount(remotePlayer);
        PlayerManager.Wind wind = (PlayerManager.Wind)DictManager.Instance.windsAllocation[remotePlayer.ActorNumber];

        // Represents the tiles currently on the GameTable which the remote player had
        GameObject[] taggedRemoteHand = GameObject.FindGameObjectsWithTag(wind + "_" + "Hand");
        // Destroy the remote player's hand tiles
        foreach (GameObject tileGameObject in taggedRemoteHand) {
            UnityEngine.Object.Destroy(tileGameObject);
        }

        List<Tile> remoteHand = new List<Tile>();
        for (int i = 0; i < remoteHandSize; i++) {
            remoteHand.Add(new Tile(0, 0));
        }
        InstantiateRemoteTiles(gameManager, wind, remoteHand, RelativePlayerPosition(remotePlayer), "Hand");
    }


    /// <summary>
    /// Called by the remote player to instantiate the hand of remotePlayer on the local client.
    /// </summary>
    public static void InstantiateRemoteOpenTiles(GameManager gameManager, Payment payment, Player remotePlayer) {
        List<Tile> remoteOpenTiles = PropertiesManager.GetOpenTiles(remotePlayer);
        PlayerManager.Wind remotePlayerWind = (PlayerManager.Wind)DictManager.Instance.windsAllocation[remotePlayer.ActorNumber];
        gameManager.UpdateAllPlayersOpenTiles(remotePlayer, remoteOpenTiles);
        
        payment.InstantPayout(remotePlayer, remoteOpenTiles, gameManager.turnManager.Turn, gameManager.numberOfTilesLeft, gameManager.isFreshTile, gameManager.discardPlayer, remotePlayerWind);

        // Represents the tiles currently on the GameTable which the remote player had
        GameObject[] taggedRemoteOpenTiles = GameObject.FindGameObjectsWithTag(remotePlayerWind + "_" + "Open");

        // Destroy the remote player's hand tiles
        foreach (GameObject tileGameObject in taggedRemoteOpenTiles) {
            UnityEngine.Object.Destroy(tileGameObject);
        }

        InstantiateRemoteTiles(gameManager, remotePlayerWind, remoteOpenTiles, RelativePlayerPosition(remotePlayer), "Open");
    }


    /// <summary>
    /// Helper function for InstantiateRemoteHand and InstantiateRemoteOpenTiles
    /// </summary>
    private static void InstantiateRemoteTiles(GameManager gameManager, PlayerManager.Wind wind, List<Tile> remoteTiles, string remotePosition, string tileType) {
        // Starting position to instantiate the tiles
        float pos;

        // Separation between tiles
        float sep = 0.83f * 0.5f;

        // Offset for the drawn tile
        float offset = 0.30f * 0.5f;

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        int negativeConversion;
        int remoteTilesSize = remoteTiles.Count;
        int remoteTilesSizeWithoutConcealedKongTile = remoteTiles.Count;

        // Determine whether negativeConversion is -1 or 1
        if (remotePosition.Equals("Left") || remotePosition.Equals("Opposite")) {
            negativeConversion = 1;
        } else if (remotePosition.Equals("Right")) {
            negativeConversion = -1;
        } else {
            Debug.LogError("Invalid remote position. Only accepted remote positions are 'Left', 'Right' and 'Opposite'");
            return;
        }


        foreach (Tile tile in remoteTiles) {
            if (tile.kongType == 3) {
                remoteTilesSizeWithoutConcealedKongTile -= 1;
            }
        }


        // Calculating the position of the first tile
        if (tileType.Equals("Hand") && new[] { 2, 5, 8, 11, 14 }.Contains(remoteTilesSize)) {
            pos = negativeConversion * 0.83f * 0.5f * (remoteTilesSizeWithoutConcealedKongTile - 2) / 2;
        } else {
            pos = negativeConversion * 0.83f * 0.5f * (remoteTilesSizeWithoutConcealedKongTile - 1) / 2;
        }


        // General formula for instantiating remote tiles
        for (int i = 0; i < remoteTilesSize; i++) {

            // Instantiate the last hand tile with an offset
            if (tileType.Equals("Hand") && new[] { 2, 5, 8, 11, 14 }.Contains(remoteTilesSize) && remoteTilesSize - 1 == i) {
                pos += -negativeConversion * offset;
            }


            // Calculate the position and rotation of each tile
            if (tileType.Equals("Hand")) {
                if (remotePosition.Equals("Left")) {
                    position = new Vector3(-gameManager.tableWidth / 2 + 0.5f, 1f, pos);
                    rotation = Quaternion.Euler(0f, -90f, 0f);

                } else if (remotePosition.Equals("Right")) {
                    position = new Vector3(gameManager.tableWidth / 2 - 0.5f, 1f, pos);
                    rotation = Quaternion.Euler(0f, 90f, 0f);

                } else if (remotePosition.Equals("Opposite")) {
                    position = new Vector3(pos, 1f, 4.4f);
                    rotation = Quaternion.Euler(0f, 0f, 0f);

                }

            } else if (tileType.Equals("Open")) {
                float yPos = 1f;
                if (remoteTiles[i].kongType == 3) {
                    pos -= -negativeConversion * sep;
                    yPos = 1f + 0.3f;
                }

                if (remotePosition.Equals("Left")) {
                    position = new Vector3(-gameManager.tableWidth / 2 + 0.5f + 0.7f, yPos, pos);
                    rotation = Quaternion.Euler(-90f, -90f, 0f);

                } else if (remotePosition.Equals("Right")) {
                    position = new Vector3(gameManager.tableWidth / 2 - 0.5f - 0.7f, yPos, pos);
                    rotation = Quaternion.Euler(-90f, 90f, 0f);

                } else if (remotePosition.Equals("Opposite")) {
                    position = new Vector3(pos, yPos, 4.4f - 0.7f);
                    rotation = Quaternion.Euler(-90f, 0f, 0f);
                }

            } else {
                Debug.LogError("Invalid tile type. Only accepted tile types are 'Hand' and 'Open'");
                return;
            }
            GameObject newTile = UnityEngine.Object.Instantiate(DictManager.Instance.tilesDict[remoteTiles[i]], position, rotation);
            newTile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            newTile.tag = wind + "_" + tileType;

            pos += -negativeConversion * sep;
        }

        return;
    }


    /// <summary>
    /// Determine the relative position of the remotePlayer with respect to the local player.
    /// </summary>
    private static string RelativePlayerPosition(Player remotePlayer) {
        Player[] playOrder = PropertiesManager.GetPlayOrder();

        // Retrieve the local and remote players' positions
        int localPlayerPos = 0;
        int remotePlayerPos = 0;
        for (int i = 0; i < playOrder.Length; i++) {
            if (playOrder[i] == PhotonNetwork.LocalPlayer) {
                localPlayerPos = i;
            }

            if (playOrder[i] == remotePlayer) {
                remotePlayerPos = i;
            }
        }

        // If the remote player is sitting on the left, the (localPlayerPos, remotePlayerPos) combinations are (1, 4), (2, 1), (3, 2), (4, 3)
        if (remotePlayerPos - localPlayerPos == 3 || localPlayerPos - remotePlayerPos == 1) {
            return "Left";
        }

        // If the remote player is sitting on the right, the (localPlayerPos, remotePlayerPos) combinations are (1, 2), (2, 3), (3, 4), (4, 1)
        if (localPlayerPos - remotePlayerPos == 3 || remotePlayerPos - localPlayerPos == 1) {
            return "Right";
        }

        // If the remote player is sitting on the opposite side
        // (localPlayerPos, remotePlayerPos) combinations are (1, 3), (2, 4), (3, 1), (4, 2)
        if (Math.Abs(localPlayerPos - remotePlayerPos) == 2) {
            return "Opposite";
        }

        Debug.LogErrorFormat("Invalid combination of localPlayerPos({0}) and remotePlayerPos({1})", localPlayerPos, remotePlayerPos);
        return "";
    }


    /// <summary>
    /// Called by the remote player to instantiate the discarded tile.
    /// </summary>
    /// <param name="hPos">The horizontal position of the tile from the perspective of the remote player</param>
    public static void InstantiateRemoteDiscardTile(GameManager gameManager, Player remotePlayer, Tile discardedTile, float pos) {
        // Scale down discard tile discard position 
        double hPos = pos / 2d;
        double vPos = 0;
        double rForce = 0;

        // v and h represents vertical and horizontal directions with respect to the perspective of the remote player
        // tan(α) = vPos / hPos = vForce / hForce; hForce ** 2 + vforce ** 2 = rForce ** 2
        // Small offsets have been added to xForce and zForce to give more force to tiles tossed from the sides
        if (RelativePlayerPosition(remotePlayer).Equals("Left") || RelativePlayerPosition(remotePlayer).Equals("Right")) {
            vPos = gameManager.tableWidth / 2 - 0.5f - 0.7f - 0.6f;
            rForce = 9 * Math.Sqrt(gameManager.tableWidth / 10f);
        } else if (RelativePlayerPosition(remotePlayer).Equals("Opposite")) {
            vPos = 4.4f - 0.7f - 0.6f;
            rForce = 9;
        }

        double tanα = vPos / (hPos + 0.1);
        double hForce = Math.Sqrt(Math.Pow(rForce, 2) / (1 + Math.Pow(tanα, 2))) + Math.Abs(hPos / 3f);
        double vForce = 0;
        if (RelativePlayerPosition(remotePlayer).Equals("Left") || RelativePlayerPosition(remotePlayer).Equals("Right")) {
            vForce = Math.Abs(hForce * tanα);
        } else if (RelativePlayerPosition(remotePlayer).Equals("Opposite")) {
            vForce = Math.Abs(hForce * tanα) + Math.Pow(hPos / 3.5d, 2);
        }

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        // Instantiation position and rotation depends on where the remote player is sitting relative to the local player
        if (RelativePlayerPosition(remotePlayer).Equals("Left")) {
            position = new Vector3((float)-vPos, 0.65f, (float)-hPos);
            rotation = Quaternion.Euler(-90f, -90f, 0f);
            if (hPos < 0) {
                hForce = -hForce;
            }

        } else if (RelativePlayerPosition(remotePlayer).Equals("Right")) {
            position = new Vector3((float)vPos, 0.65f, (float)hPos);
            rotation = Quaternion.Euler(-90f, 90f, 0f);
            vForce = -vForce;
            if (hPos > 0) {
                hForce = -hForce;
            }

        } else if (RelativePlayerPosition(remotePlayer).Equals("Opposite")) {
            position = new Vector3((float)-hPos, 0.65f, 4.4f - 0.7f - 0.6f);
            rotation = Quaternion.Euler(-90f, 0f, 0f);
            vForce = -vForce;
            if (hPos < 0) {
                hForce = -hForce;
            }

        }

        // Remove the tag of the tile discarded before the current tile
        GameObject previousDiscard = GameObject.FindGameObjectWithTag("Discard");
        if (previousDiscard != null) {
            previousDiscard.tag = "Untagged";
        }


        GameObject tileGameObject = UnityEngine.Object.Instantiate(DictManager.Instance.tilesDict[discardedTile], position, rotation);
        tileGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        // Tagging is necessary to reference the last tile discarded when Chow/Pong/Kong is called
        tileGameObject.tag = "Discard";

        foreach (Transform child in tileGameObject.transform) {
            child.GetComponent<MeshCollider>().convex = true;
        }

        Rigidbody rb = tileGameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        // The application of hForce and VForce on the x-z axes depends on the remote player's position
        if (RelativePlayerPosition(remotePlayer).Equals("Left") || RelativePlayerPosition(remotePlayer).Equals("Right")) {
            rb.AddForce(new Vector3((float)vForce, 0f, (float)hForce), ForceMode.VelocityChange);

        } else if (RelativePlayerPosition(remotePlayer).Equals("Opposite")) {
            rb.AddForce(new Vector3((float)hForce, 0f, (float)vForce), ForceMode.VelocityChange);
        }
    }
}
