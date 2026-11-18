using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Leandro
// Ultima alteração: 11/01 (Leandro)
// O que foi alterado: subir e descer

public class AguaSuja : MonoBehaviour
{
    private float highMax = 1.5f;
    private float tempoMax = 1.8f;
    private float highInicial;
    private float tempoAtual = 0.0f;
    private bool aumento = true;
    PlayerController player;
    public GameObject slownessDisplay; // Recebe o objeto que contem o icone de lentidão


    void Start()
    {
        highInicial = transform.localScale.y;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // Atribui o componente "PlayerController" a variavel "player"
    }

    void Update()
    {
        if (tempoAtual < tempoMax)//se o tempo decorrido for menor que o tempo maximo, continua a animaçao
        {
            float novaAltura;//guarda a altura calculada

            if (aumento)// ve se ta aumentando ou voltand a altura inicial
            {
                novaAltura = Mathf.Lerp(highInicial, highMax, tempoAtual / tempoMax);
            }
            else
            {
                novaAltura = Mathf.Lerp(highMax, highInicial, tempoAtual / tempoMax);
            }
            transform.localScale = new Vector3(transform.localScale.x, novaAltura, transform.localScale.z);// muda a nova altura sem mudar as outras

            tempoAtual += Time.deltaTime ;// atualiza o tempo
        }
        else//se chega no tempo max ele inverte a equaçao
        {
            tempoAtual = 0.0f;
            aumento = !aumento; //diminui a altura
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) // Ativa a lentidao assim que o player passa por ela
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
            StartCoroutine(RemoveSlowness(2)); // Remove o efeito de lentidão após alguns segundos
        }
    }
    IEnumerator RemoveSlowness(float time) // Função que espera segundos para executar certo codigo
    {
        yield return new WaitForSeconds(time); // Espera os segundos determinado

        slownessDisplay.SetActive(false); // Oculta o icone de lentidão na interface

        player.maxSpeed *= 2; // Velocidade do player volta ao padrão
        player.jumpForce *= 2; // Altura de pulo do player volta ao padrão
    }

    
}