using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    private Rigidbody rb;
    private Vector3 movement;
    public float speed = 6;
    private Animator animator;
    
    private void Awake(){
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning(h, v);
        Animating(h, v);
    }

    void Move(float h, float v){
        movement.Set(h, 0, v);
        movement = movement.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    void Turning(float h, float v){
        Vector3 rot = new Vector3(h, 0, v);

        if(rot != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(rot);
        }
    }
    
    void Animating(float h, float v){
        bool walking = h != 0 || v != 0;
        animator.SetBool("Walking", walking);
    }

}
