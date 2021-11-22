using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour{
    
    public GameObject bullet;
    public Transform bulletSpwan;
    public float fireRate = 5;

    void Start(){
        InvokeRepeating("Fire", fireRate, fireRate);
    }

    void Fire(){
        Instantiate(bullet, bulletSpwan.position, bulletSpwan.rotation);
    }
}
