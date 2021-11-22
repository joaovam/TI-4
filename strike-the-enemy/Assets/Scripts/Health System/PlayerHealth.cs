using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health{

    public Slider healthSlider;
    public Image damageImage;
    public Color damageColor;
    public float flashSpeed = 5;

    private bool damaged;
    private Animator anim;
    private PlayerMovement PlayerMovement;

    private void Awake(){
        anim = GetComponent<Animator>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }
    
    void Start(){
        Spawn();
    }

    void Update(){
        if(damaged){
            damageImage.color = damageColor;
        }
        else{
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }

    public override void TakeDamage(int damage, Vector3 hitPoint){
        if(isDead)
            return;

        damaged = true;
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if(currentHealth <= 0){
            Death();
        }
    }

    protected override void Death(){
        isDead = true;
        anim.SetTrigger("Die");
        PlayerMovement.enabled = false;
        LevelController.instance.GameOver();
    }

    protected override void Spawn(){
        currentHealth = startingHealth;
    }

}
