using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {
    float lifeTime;
    LayerMask enemyMask;
    [SerializeField] ParticleSystem explosion;
    float particleSystemLength;
    // Start is called before the first frame update
    void Awake() {
        enemyMask = LayerMask.GetMask("Enemy");
        lifeTime = 1.5f;
        particleSystemLength = 0.33f;
        explosion = GetComponentInChildren<ParticleSystem>();
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
                foreach (Collider enemy in enemies) {
                    enemy.transform.GetComponent<NavMeshAgentBrain>().Damage(100);
                }
                Destroy(this.gameObject);
            }
            
        }
        
    }
}
