using UnityEngine;
using TMPro;
using System;

public class RoundTimer : MonoBehaviour
{
    float startTime = 10f;
    public float remainingTime;
    public TextMeshProUGUI timer;

    void Start()
    {
        remainingTime = startTime;
    }

    
    void Update()
    {
        remainingTime -= Time.deltaTime;
        timer.text = Math.Round(remainingTime, 2).ToString().Replace(".", ":");

        if(remainingTime <= 0){
            timer.enabled = false;
        }
    }
}
