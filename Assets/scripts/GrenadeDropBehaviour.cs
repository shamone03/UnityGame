using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;

public class GrenadeDropBehaviour : MonoBehaviourPunCallbacks {
    private float waitTime;
    private void Start() {
        waitTime = 10;
    }

    private void Update() {
        waitTime -= Time.deltaTime;
        if (waitTime < 0) {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
