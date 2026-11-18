using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Samuca
// Ultima alteracao: 17/12 (Tanaka)

public class Cheese : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Cheese++;
            
            // A implementar animacao de pegar o queijo
            
            Destroy(gameObject);
        }
    }
}
