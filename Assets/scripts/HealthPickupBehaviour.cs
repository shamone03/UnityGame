using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupBehaviour : MonoBehaviour {
    void Start() {
        Destroy(this.gameObject, 10);
    }
}
