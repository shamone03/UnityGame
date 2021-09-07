using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Photon.Pun;

public class NavMeshAgentBrain : MonoBehaviourPunCallbacks {
    
    public bool shouldMove = true;
    public GameObject player;
    public float health = 15;
    public LayerMask playerMask;
    [SerializeField]
    float timeBetweenHits = 1.5f;
    float counter;
    public GameObject healthDrop;
    public GameObject grenadeDrop;
    public Animator animator;
    NavMeshAgent navMeshAgent;
    private int maxTargets = 2;
    private Collider[] results = new Collider[2];
    

    // Start is called before the first frame update
    void Start() { 
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerMask = LayerMask.GetMask("Player");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update() {
        //counter += Time.deltaTime;
        //navMeshAgent.SetDestination(player.transform.position);
        //if (Physics.CheckSphere(this.transform.position + new Vector3(0, 2, 0), 2, playerMask)) {
        //    shouldMove = false;
        //    navMeshAgent.isStopped = true;
        //    animator.Play("Z_Attack");
        //    if (counter >= timeBetweenHits) {

        //        player.transform.GetComponent<PlayerMovement>().Damage(5);
        //        counter = 0;
        //    }

        //} 
        //if (Physics.CheckSphere(this.transform.position + new Vector3(0, 2, 0), 15, playerMask) && !Physics.CheckSphere(this.transform.position + new Vector3(0, 2, 0), 2, playerMask)) {
        //    shouldMove = true;
        //    navMeshAgent.isStopped = false;
        //} else {
        //    animator.Play("Z_Idle");
        //    shouldMove = false;
        //    navMeshAgent.isStopped = true;
        //}
        //if (shouldMove) {

        //    navMeshAgent.isStopped = false;
        //    animator.Play("Z_Run_InPlace");
        //}

        

        if (navMeshAgent.isOnNavMesh) {
            
            // int numFound = Physics.OverlapSphereNonAlloc(this.transform.position + new Vector3(0, 2, 0), 50, results, playerMask);
            if (Physics.OverlapSphere(this.transform.position + new Vector3(0, 2, 0), 50, playerMask).Length > 0  && !Physics.CheckSphere(this.transform.position + new Vector3(0, 2, 0), 3, playerMask)) {
                
                shouldMove = true;
                animator.Play("Z_Run_InPlace");
            } else if (Physics.OverlapSphere(this.transform.position + new Vector3(0, 2, 0), 2, playerMask).Length > 0) {
                shouldMove = false;
                animator.Play("Z_Attack");
                counter += Time.deltaTime;
                if (counter >= timeBetweenHits) {
                    player.transform.GetComponent<PlayerController>().Damage(3);
                    counter = 0;
                }
            } else {
                shouldMove = false;
                navMeshAgent.isStopped = true;
                animator.Play("Z_Idle");
            }

            if (shouldMove) {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.transform.position);
            }
        }


    }

    public void Damage(float damage) {
        health -= damage;
        if (health <= 0) {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= 0.25f) {
                PhotonNetwork.Instantiate(grenadeDrop.name, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            } else {
                PhotonNetwork.Instantiate(healthDrop.name, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }

            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}