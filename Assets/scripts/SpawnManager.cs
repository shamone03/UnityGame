using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(SpawnCoroutine());
    }

    

    IEnumerator SpawnCoroutine() {
        while (true) {
            _ = Instantiate(enemyPrefab, new Vector3(Random.Range(98, 48), -17, Random.Range(10, 110)), Quaternion.identity);

            yield return new WaitForSeconds(3);
        }
    }
}
