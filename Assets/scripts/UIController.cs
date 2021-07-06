using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIController : MonoBehaviour {
    public static UIController instance;
    [SerializeField] public TMP_Text overheatedMessage;
    [SerializeField] public Slider weaponTempSlider;
    [SerializeField] public Slider healthSlider;
    [SerializeField] public TMP_Text deadMessage;
    [SerializeField] public Image crosshair;
    [SerializeField] public TMP_Text score;
    [SerializeField] public TMP_Text grenades;
    // Start is called before the first frame update
    private void Awake() {
        instance = this;
    }
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
