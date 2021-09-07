using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class HealthPickupBehaviour : MonoBehaviourPunCallbacks {
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
