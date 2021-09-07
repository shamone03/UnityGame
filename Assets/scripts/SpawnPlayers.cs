using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviourPunCallbacks {
    public GameObject playerPrefab;
    public Terrain terrain;
    
    public float xMin = 235;
    public float xMax = 245;
    public float zMin = 158;
    public float zMax = 168;
    public float randomX;
    public float randomZ;
    public float sampleY;
    

    public void Start() { 
        randomX = Random.Range(xMin, xMax);
        randomZ = Random.Range(zMin, zMax);
        sampleY = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));

        Vector3 randomPosition = new Vector3(randomX, sampleY + 5, randomZ);
        
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
