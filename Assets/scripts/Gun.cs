using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public bool isAutomatic;
    public float timeBetweenShots = 0.1f;
    public float heatPerShot = 1;
    public float damage;
    public float maxHeat;
    public float heatCounter = 0;
    public bool overheated = false;
}