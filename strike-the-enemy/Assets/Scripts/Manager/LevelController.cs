using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour{

    public static LevelController instance;

    public Animator gameOverCanvas;

    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Start(){
        
    }

    public void GameOver(){
        gameOverCanvas.SetTrigger("GameOver");
    }
}
