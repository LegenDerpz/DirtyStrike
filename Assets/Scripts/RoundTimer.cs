using UnityEngine;
using TMPro;
using System;

public class RoundTimer : MonoBehaviour
{
    float buyPhaseTime = 10f;
    float roundTime = 20f;
    public float remainingTime;
    public TextMeshProUGUI timer;

    public bool buyPhaseEnded = false;
    public bool timeRanOut = false;

    void Start()
    {
        remainingTime = buyPhaseTime;
    }

    
    void Update()
    {
        if(remainingTime <= 0 && !buyPhaseEnded){
            buyPhaseEnd();
        }

        if(!timeRanOut){
            remainingTime -= Time.deltaTime;
        }

        timer.text = Math.Round(remainingTime, 2).ToString().Replace(".", ":");

        if(remainingTime <= 0 && buyPhaseEnded){
            timeRanOut = true;
            timer.enabled = false;
            FindObjectOfType<GameLoop>().FindWinCondition();
        }
    }

    void buyPhaseEnd(){
        buyPhaseEnded = true;
        remainingTime = roundTime;
        //roundStart();
    }
}
