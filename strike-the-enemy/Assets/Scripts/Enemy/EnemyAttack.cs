using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttack : MonoBehaviour{

    public float attackRate = 1;
    public int damage = 10;

    private GameObject player;
    private PlayerHealth playerHealth; 
    private bool playerInRange;
    private float timer;

    public GameObject damegeImpact;
    
    void Start(){
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update(){
        timer += Time.deltaTime;

        if(timer > attackRate && playerInRange){
            Attack();
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject == player){
            playerInRange = false;
        }
    }

    void Attack(){
        timer = 0;

        playerHealth.TakeDamage(damage, Vector3.zero);
    }

    public void StartAttack(){
        damegeImpact.SetActive(true);
    }

    public void FinishAttack(){
        damegeImpact.SetActive(false);
    }
}
