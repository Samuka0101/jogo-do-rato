using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Dono do script: Nitsa

public class Scoreboard : MonoBehaviour
{
    private int totalCheese = 15;

    public float timeRemaining = 0;
    public bool timeIsRunning = true;

    public Text cheeseText;
    public Text timerText;

    public GameObject player;
    public GameObject gate;

    void Start()
    {
        timeIsRunning = true;
    }

    void Update()
    {
        if (timeIsRunning){

            if(timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;  
                DisplayTime(timeRemaining);
            }
        }
    }
    
    void DisplayTime (float timeToDisplay)
    {
        timeToDisplay +=1;
        float minutes = Mathf.FloorToInt (timeToDisplay / 60);
        float seconds = Mathf.FloorToInt (timeToDisplay % 60);
        timerText.text = string.Format ("{0:00} : {1:00}",minutes, seconds);
    }

    public void UpdateCheese(int cheeseCount)
    {
        cheeseText.text = cheeseCount.ToString() + " / " + totalCheese.ToString();

        if(cheeseCount >= totalCheese-1) 
        {
            gate.SetActive(false);
        }
        
        if(cheeseCount >= totalCheese)
        {
            print("Voce ganhou!");
            print("Seu tempo foi: " + timerText.text);
        }
    }    
}
