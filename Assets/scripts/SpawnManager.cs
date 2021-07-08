using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField] private Terrain terrain;

    private float xMin;
    private float xMax;
    private float zMax;
    private float zMin;
    private float randomX;
    private float sampleY;
    private float randomZ;
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(SpawnCoroutine());
        // xMin = terrain.terrainData.size.x;
        // xMax = terrain.transform.position.x + xMin;
        // zMin = terrain.terrainData.size.z;
        // zMax = terrain.transform.position.z + zMin;
        xMin = 235;
        xMax = 375;
        zMin = 158;
        zMax = 263;
    }

    

    IEnumerator SpawnCoroutine() {
        while (true) {
            randomX = Random.Range(xMin, xMax);
            randomZ = Random.Range(zMin, zMax);
            sampleY = terrain.SampleHeight(new Vector3(randomX, 0, randomZ));
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(randomX, sampleY, randomZ), Quaternion.identity);
            
            // if (!enemy.GetComponent<NavMeshAgent>().isOnNavMesh) {
            //     Physics.Raycast(enemy.transform.position, Vector3.down, out RaycastHit hit);
            //     if (hit.transform.CompareTag("Ground")) {
            //         enemy.GetComponent<NavMeshAgent>().Warp(hit.point);
            //     }
            //     else {
            //         Destroy(enemy);
            //     }
            // }

            yield return new WaitForSeconds(1);
        }
    }
}
