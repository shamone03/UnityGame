using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class CreateAndJoin : MonoBehaviourPunCallbacks {
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField nameInput;
    
    public void CreateRoom() {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom() {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public void SetName() {
        PhotonNetwork.LocalPlayer.NickName = nameInput.text;
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.LoadLevel("firstscene");
    }
    // Start is called before the first frame update
    
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
