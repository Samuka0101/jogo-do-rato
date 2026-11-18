using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Allan
// Ultima alteração: 05/01 (Tanaka)
// O que foi alterado: Bugs de seguir o player consertados

public class Human : MonoBehaviour
{
    private Vector3 lastPosition; // Eh usada para verificar se o humano esta parado

    float humanSpeed = 6, distancePlayer, distanceSpawn, viewMax = 8;
    private int flipped = 1;

    bool onVision, wandering, facingRight;

    Transform player;

    Rigidbody2D rbHuman;

    Animator animHuman;

    SpriteRenderer spriteHuman;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Atribui o componente "Transform" do player para a variavel "player"

        rbHuman = GetComponent<Rigidbody2D>(); // Atribui o componente "Rigidbody" do inimigo para a variavel "rbHuman"
        animHuman = GetComponent<Animator>(); // Atribui o componente "Animator" do inimigo para a variavel "animHuman"
        spriteHuman = GetComponent<SpriteRenderer>(); // Atribui o componente "SpriteRenderer" do inimigo para a variavel "spriteHuman"

        lastPosition = transform.position; 
    }

    void Update()
    {
        distancePlayer = Vector2.Distance(transform.position, player.position); // Verefica a distancia entre o inimigo e o player

        if(distancePlayer > viewMax) // Verefica se o player esta fora da visão do inimigo
        {
            onVision = false;
        }
        else if(distancePlayer < viewMax) // Verefica se o player esta na visão do inimigo e inicia a perseguição
        {
            onVision = true;
        }

        Animation();
    }

    void FixedUpdate()
    {
        if(onVision == true) // Inicia a perseguição caso "startHunt" e "onVision" forem true
        {
            FollowPlayer();
        }
        else 
        {
            Wandering();
        }
    }

    void FollowPlayer()
    {
        if(player.gameObject != null) // Inicia a perseguição caso o player esteja ativo
        {
            Vector2 target = new Vector2(player.position.x, transform.position.y); 
            transform.position = Vector2.MoveTowards(transform.position, target, humanSpeed * Time.fixedDeltaTime); // Altera a posição do inimigo para ir atrás do player
        }
    }

    void Wandering()
    {
        transform.position += new Vector3((humanSpeed / 2) * flipped * Time.fixedDeltaTime, 0, 0); // Estado "vagando" do inimigo
        if(transform.position == lastPosition) // Verefica se o inimigo conseguiu se mover
        {
            flipped *= -1;
        }
        lastPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        flipped *= -1;
    }

    void Animation()
    {
        animHuman.SetBool("startRun", onVision);

        if((onVision && transform.position.x < player.position.x) || (!onVision && flipped > 0)) // se esta indo para a direita
        {
            spriteHuman.flipX = true;
        } 
        else 
        {
            spriteHuman.flipX = false;
        }
    }
}
