using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Samuca
// Ultima alteracao: 12/01

public class Trash : MonoBehaviour
{
    public GameObject cheesePrefab;
    private GameObject cheeseObject;

    private bool interact = false; // Verifica se o jogador pode interagir
    private bool open = false;

    private bool hasCheese = false;
    public bool HasCheese {
        get { return hasCheese; }
        set { 
            hasCheese = value; 
            if(hasCheese)
            {
                GetCheese();
            }
        }
    }

    private SpriteRenderer sprite;
    private Animator animator;

    void Start() 
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update() 
    {
        interagir();
    }
    
    void interagir()
    {
        if (interact && Input.GetKeyDown(KeyCode.E)) // Se o jogador pressionar 'E'
        {
            if(!open)
            {
                Open();
            }
        }
    }

    private void GetCheese() 
    {
        cheeseObject = Instantiate(cheesePrefab, transform.position, Quaternion.identity);
        cheeseObject.SetActive(false);
    }

    private void Open() 
    {
        animator.SetTrigger("Open");
        open = true;
        
        if(!hasCheese) return;

        cheeseObject.SetActive(true);
        cheeseObject.GetComponent<Animator>().SetTrigger("Jump");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Verifica se o jogador entrou na área
        {
            if(open) return;
            interact = true;

            sprite.color = new Color(0.9f, 0.9f, 0.9f, 1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Verifica se o jogador saiu da área
        {
            interact = false;
            
            sprite.color = new Color(1, 1, 1, 1);
        }
    }
}

