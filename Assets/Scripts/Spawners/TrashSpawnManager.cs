using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// Dono do script: Samuca
// Ultima alteracao: 12/01

public class TrashSpawnManager : MonoBehaviour
{
    public GameObject trash; // Prefab do lixo
    private int totalTrashes = 5, totalCheese = 2; // Total de lixos e queijos
    private Vector2 cheesePosition; // Posição do queijo
    private List<Vector2> spawnPositions = new List<Vector2>(); // Posições possíveis para spawn

    void Start()
    {
        inicializaPo(); // Inicializa as posições possíveis de spawn
        RandomPo(); // Spawna os lixos
    }

    void inicializaPo()
    {
        // Inicializa as posições possíveis de spawn com base nos filhos do objeto
        int i = 0;
        foreach (Transform child in transform)
        {
            spawnPositions.Add(child.position);
            i++;
        }
    }

    void RandomPo()
    {
        List<GameObject> trashList = new List<GameObject>();
        for(int i = 0; i < totalTrashes; i++)
        {
            int randomIndex = Random.Range(0, spawnPositions.Count);
            Vector2 spawnPosition = spawnPositions[randomIndex];

            spawnPositions.RemoveAt(randomIndex); // Remove da lista de posicoes ocupaveis, pois vai ser ocupada

            trashList.Add(Instantiate(trash, spawnPosition, Quaternion.identity)); // Spawna o lixo e coloca objeto na lista de lixos
        }

        for(int i = 0; i < totalCheese; i++) 
        { 
            int randomCheeseIndex = Random.Range(0, trashList.Count);
            trashList[randomCheeseIndex].gameObject.GetComponent<Trash>().HasCheese = true;

            trashList.RemoveAt(randomCheeseIndex); // Remove da lista de lixos, pois ja ganhou queijo
        }
    }
}
