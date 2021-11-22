using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public float velocidade;
    private Vector3 posicao;
    private CharacterController cc;
    void Start()
    {
        cc = GetComponent<CharacterController>();
        posicao = this.transform.position;
        velocidade = 0.2f;

    }

    // Update is called once per frame
    void Update()
    {
        posicao = new Vector3(Input.GetAxis("Horizontal") * velocidade, 0, Input.GetAxis("Vertical") * velocidade);
        _ = cc.Move(posicao);

    }

}
