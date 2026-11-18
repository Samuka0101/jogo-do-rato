using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Samuca
// Ultima alteracao: 16/12 (Samuca)

public class CheeseScript : MonoBehaviour
{
    public GameObject cheese;
    private Vector2[] spawnPositions = new Vector2[20];
    private int maxCheese = 12;
    private int cheeseCount;
    private bool stop = true;
    private List<Vector2> occupiedPositions = new List<Vector2>(); //Lista de posições

    // Start is called before the first frame update
    void Start()
    {
        InicializaPosicao();
    }

    // Update is called once per frame
    void Update()
    {
        if (stop)
        {
            RandomSpawn();
        }
    }

    private void InicializaPosicao()
    {
        int i = 0;
        foreach(Transform child in transform) 
        {
            spawnPositions[i] = child.position;
            i++;
        }
    }

    void RandomSpawn()
    { 
        //Verifica se a quantidade de queijo no mapa é valida (menor que a quantidade maxima)
        if (cheeseCount < maxCheese) 
        {
            int randomIndex = Random.Range(0, spawnPositions.Length);
            Vector2 spawnPosition = spawnPositions[randomIndex];

            // Verifica se a posição já está ocupada
            if (!occupiedPositions.Contains(spawnPosition)) 
            {
                Instantiate(cheese, spawnPosition, Quaternion.identity); 
                occupiedPositions.Add(spawnPosition); //Adiciona a posição a lista
                cheeseCount++; //Adiciona ao contador de queijos no mapa
            } 
        } 
        else 
        {
            stop = false;
        }
    }
}