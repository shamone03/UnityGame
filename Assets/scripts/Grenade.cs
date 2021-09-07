using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Grenade : MonoBehaviourPunCallbacks {
    float lifeTime;
    LayerMask enemyMask;
    [SerializeField] ParticleSystem explosion;
    float particleSystemLength;

    private int scoreIncrease;

    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Awake() {
        enemyMask = LayerMask.GetMask("Enemy");
        lifeTime = 1.5f;
        particleSystemLength = 0.33f;
        explosion = GetComponentInChildren<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) {
            explosion.Play();
            particleSystemLength -= Time.deltaTime;
            
            if (particleSystemLength <= 0) {
                explosion.Stop();
                Collider[] enemies = Physics.OverlapSphere(this.transform.position, 5, enemyMask);
                if (enemies.Length > 0) {
                    scoreIncrease = enemies.Length * 5;
                }
                foreach (Collider enemy in enemies) {
                    enemy.transform.GetComponent<NavMeshAgentBrain>().Damage(100);
                }
                player.gameObject.transform.GetComponent<PlayerController>().ScoreIncrease(scoreIncrease);
                PhotonNetwork.Destroy(this.gameObject);
            }
            
        }
        
    }
}
