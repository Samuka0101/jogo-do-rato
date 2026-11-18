using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Samuca
// Ultima alteracao: 19/12 (Tanaka)

public class Void : MonoBehaviour
{    
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<PlayerController>().KillPlayer();
            return;
        }

        Destroy(collision.gameObject);
    }
}
