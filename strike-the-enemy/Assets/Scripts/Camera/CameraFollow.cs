using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{

    public Transform target;
    public float smoothing = 5;
    private Vector3 offset;

    void Start(){
        offset = transform.position - target.position;
    }


    private void FixedUpdate(){
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
    }
}
