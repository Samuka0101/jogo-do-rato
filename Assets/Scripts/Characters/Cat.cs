using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Allan
// Ultima alteração: 12/01 (Allan)

public class Cat : MonoBehaviour
{
    private Vector3 lastPosition; // Eh usada para verificar se o gato esta parado (preso)

    float catSpeed = 7, catJumpForce = 800, distancePlayer, distanceSpawn, viewMax = 10;
    bool startHunt, onVision, catJump, wake;

    Transform player;

    Vector2 spawn;

    Rigidbody2D rbCat;

    Animator animCat;

    SpriteRenderer spriteCat;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Atribui o componente "transform" do player para a variavel "playerPosition"

        spawn = transform.position; // Recebe a posição inicial do inimigo

        rbCat = GetComponent<Rigidbody2D>(); // Atribui o componente "rigidbody" do inimigo para a variavel "rbCat"

        animCat = GetComponent<Animator>();

        spriteCat = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        distancePlayer = Vector2.Distance(transform.position, player.position); // Verefica a distancia entre o inimigo e o player

        if(distancePlayer > viewMax) // Verefica se o player esta fora da visão do inimigo
        {
            onVision = false;
        }
        else if(distancePlayer < viewMax) // Verefica se o player esta na visão do inimigo
        {
            onVision = true;
        }

        Animation();
    }

    void FixedUpdate()
    {
        if(startHunt == true && onVision == true) // Inicia a perseguição caso "startHunt" e "onVision" forem true
        {
            FollowPlayer();
        }
        else if(onVision == false) // Faz o inimigo retornar a posição inicial caso "onVision" for false
        {
            ReturnToSpawn();
        }

        if(catJump == true) // Faz o inimigo pular caso "catJump" for true
        {
            JumpObstacle();
        }
    }

    void FollowPlayer()
    {
        if(player.gameObject != null) // Inicia a perseguição caso o player esteja ativo
        {
            Vector2 targetPosition = new Vector2(player.position.x, transform.position.y); 
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, catSpeed * Time.fixedDeltaTime); // Altera a posição do inimigo para ir atrás do player
        }
    }

    void ReturnToSpawn()
    {
        transform.position = Vector2.MoveTowards(transform.position, spawn, catSpeed * Time.fixedDeltaTime); // Altera a posição do inimigo para voltar a posição inicial

        distanceSpawn = Vector2.Distance(transform.position, spawn); // Verefica a distancia entre o inimigo e a posição inicial dele

        if((distanceSpawn <= 0 || Vector2.Distance(transform.position, lastPosition) < 0.1f) && startHunt == true) // Volta a dormir
        {
            GetComponent<BoxCollider2D>().isTrigger = true; // Ativa o trigger do collider
            GetComponent<Rigidbody2D>().gravityScale = 0; // Ajusta a gravidade

            onVision = false; // Player não esta na visão do inimigo
            startHunt = false; // Para a perseguição pelo player

            transform.position = spawn;
        }

        lastPosition = transform.position;
    }

    void JumpObstacle()
    {
        rbCat.velocity = new Vector2(rbCat.velocity.x, catJumpForce * Time.fixedDeltaTime); // Altera a velocidade no eixo "y" do inimigo, fazendo-o pular
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Box")) // Deixa "catJump" true assim que o inimigo encontra um obstaculo
        {
            catJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Box")) // Deixa "catJump" false assim que o inimigo ultrapassa o obstaculo
        {
            catJump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player")) // Deixa "startHunt" true assim que o player passa pelo inimigo
        {
            StartCoroutine(StartHunt(1)); // Chama a função "StartHunt" através de um "Coroutine"
            animCat.SetTrigger("wakeUp");
        }
    }

    IEnumerator StartHunt(float time) // Função que espera segundos para executar certo codigo
    {
        yield return new WaitForSeconds(time); // Espera os segundos determinado

        GetComponent<BoxCollider2D>().isTrigger = false; // Desativa o trigger do collider
        GetComponent<Rigidbody2D>().gravityScale = 5; // Ajusta a gravidade

        onVision = true; // Player esta na visão do inimigo
        startHunt = true; // Inicia a perseguição pelo player
    }

    void Animation()
    {
        animCat.SetBool("onHunt", startHunt);

        if((onVision && transform.position.x < player.position.x) && startHunt == true) // se esta indo para a direita
        {
            spriteCat.flipX = false;
        } 
        else 
        {
            spriteCat.flipX = true;
        }
    }
}
