﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateTile : MonoBehaviour {
    public GameObject gm;

    // Start is called before the first frame update
    void Start() {
        GameObject tile = Instantiate(gm, new Vector3(0f, 2f, 0f), Quaternion.Euler(270f, 180f, 0f));
        tile.transform.localScale = 
    }

    // Update is called once per frame
    void Update() {

    }
}
