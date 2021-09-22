using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
   public void carregaCena(string nome){
       SceneManager.LoadScene(nome);
   }
}
