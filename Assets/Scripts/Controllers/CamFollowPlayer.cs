using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dono do script: Tanaka
// Ultima alteracao: 01/01 (Tanaka)

public class CamFollowPlayer : MonoBehaviour
{
    Transform player;
    Vector3 offset = new Vector3(0, 1, -10);

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = player.position + offset;
    }
}
