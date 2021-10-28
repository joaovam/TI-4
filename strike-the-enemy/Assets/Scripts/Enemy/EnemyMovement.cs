using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour{

    private Transform player;
    private NavMeshAgent nav;

    private void Awake(){
        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(Vector3.zero);
    }

    private void Start(){
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update(){
        nav.SetDestination(player.position);
    }
}
