using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Dono do script: Allan
// Ultima alteração: 8/12 (Allan)
// O que foi alterado: Tudo

public class Ratoeira : MonoBehaviour
{
    PlayerController player;
    public GameObject slownessDisplay; // Recebe o objeto que contem o icone de lentidão

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // Atribui o componente "PlayerController" a variavel "player"
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) // Ativa a ratoeira assim que o player passa por ela
        {
            slownessDisplay.SetActive(true); // Mostra o icone de lentidão na interface

            player.maxSpeed /= 2; // Reduz a velocidade do player pela metade
            player.jumpForce /= 2; // Reduz a altura de pulo do player pela metade
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponent<BoxCollider2D>().enabled = false; // Desativa a colisão da ratoeira

            StartCoroutine(RemoveSlowness(2)); // Remove o efeito de lentidão após alguns segundos

            StartCoroutine(ActiveRatoeira(5)); // Ativa a ratoeira novamente após alguns segundos
        }
    }

    IEnumerator RemoveSlowness(float time) // Função que espera segundos para executar certo codigo
    {
        yield return new WaitForSeconds(time); // Espera os segundos determinado

        slownessDisplay.SetActive(false); // Oculta o icone de lentidão na interface

        player.maxSpeed *= 2; // Velocidade do player volta ao padrão
        player.jumpForce *= 2; // Altura de pulo do player volta ao padrão
    }

    IEnumerator ActiveRatoeira(float time) // Função que espera segundos para executar certo codigo
    {
        yield return new WaitForSeconds(time); // Espera os segundos determinado

        GetComponent<BoxCollider2D>().enabled = true; // Ativa a colisão com a ratoeira novamente
    }
}
