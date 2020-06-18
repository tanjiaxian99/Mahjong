using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public static class CustomTypes {

    /// <summary>
    /// Serialize List<Tile> into a byteStream
    /// </summary>
    public static byte[] SerializeTilesList(object customType) {
        var tilesList = (List<Tile>)customType;
        byte[] byteArray = new byte[tilesList.Count * 2];

        for (int i = 0; i < tilesList.Count; i++) {
            byteArray[i * 2] = tilesList[i].Id[0];
            byteArray[i * 2 + 1] = tilesList[i].Id[1];
        }
        return byteArray;
    }


    /// <summary>
    /// Deserialize the byteStream into a List<Tile>
    /// </summary>
    public static object DeserializeTilesList(byte[] data) {
        List<Tile> tilesList = new List<Tile>();

        for (int i = 0; i < data.Length / 2; i++) {
            Tile tile = new Tile(0, 0);
            tile.Id = new byte[2] { data[i * 2], data[i * 2 + 1] };
            tilesList.Add(tile);
        }

        return tilesList;
    }


    /// <summary>
    /// Serialize Tuple<int, Tile, float> into a byteStream. sizeof(memTuple) = sizeof(int) + sizeof(short) + sizeof(short) + sizeof(int)
    /// </summary>
    public static readonly byte[] memTuple = new byte[12];
    public static short SerializeDiscardTileInfo(StreamBuffer outStream, object customobject) {
        var tuple = (Tuple<int, Tile, float>)customobject;

        lock (memTuple) {
            byte[] bytes = memTuple;
            int index = 0;

            Protocol.Serialize(tuple.Item1, bytes, ref index);
            Protocol.Serialize(tuple.Item2.Id[0], bytes, ref index);
            Protocol.Serialize(tuple.Item2.Id[1], bytes, ref index);
            Protocol.Serialize(tuple.Item3, bytes, ref index);
            outStream.Write(bytes, 0, 12);
        }

        return 12;
    }


    /// <summary>
    /// Deserialize the byteStream into a Tuple<int, Tile, float>
    /// </summary>
    public static object DeserializeDiscardTileInfo(StreamBuffer inStream, short length) {
        Tuple<int, Tile, float> tuple = new Tuple<int, Tile, float>(0, new Tile(0, 0), 0f);
        int actorNumber;
        short tileIdOne;
        short tileIdTwo;
        Tile tile = new Tile(0, 0);
        float pos;

        lock (memTuple) {
            inStream.Read(memTuple, 0, 12);
            int index = 0;

            Protocol.Deserialize(out actorNumber, memTuple, ref index);
            Protocol.Deserialize(out tileIdOne, memTuple, ref index);
            Protocol.Deserialize(out tileIdTwo, memTuple, ref index);
            Protocol.Deserialize(out pos, memTuple, ref index);


            tile.Id = new byte[2] { (byte)tileIdOne, (byte)tileIdTwo };
        }
        return new Tuple<int, Tile, float>(actorNumber, tile, pos);
    }
}
