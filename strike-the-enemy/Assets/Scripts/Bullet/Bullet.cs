using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{

    public float speed = 10;
    private Rigidbody rb;

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    void Start(){
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision){
        Destroy(gameObject);
    }

}
