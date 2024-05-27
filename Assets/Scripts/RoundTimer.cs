using UnityEngine;
using TMPro;
using System;

public class RoundTimer : MonoBehaviour
{
    float buyPhaseTime = 20f;
    float roundTime = 100f;
    public float remainingTime;
    public TextMeshProUGUI timer;
    GameObject bombPlantedIndicator;

    public bool buyPhaseEnded = false;
    public bool timeRanOut = false;
    bool checkedShopState = false;

    void Start()
    {
        remainingTime = buyPhaseTime;
        bombPlantedIndicator = GameObject.Find("BombPlantedIndicator");
        bombPlantedIndicator.SetActive(false);
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

        if(!checkedShopState){
            if(buyPhaseEnded){
                foreach(PlayerControls player in FindObjectsOfType<PlayerControls>()){
                    if(player.GetComponent<PlayerControls>().GetShopState()){
                        player.GetComponent<PlayerControls>().CloseWeaponShop();
                    }
                }
                checkedShopState = true;
            }
        }
    }

    void buyPhaseEnd(){
        buyPhaseEnded = true;
        remainingTime = roundTime;
    }

    public void SetRemainingTime(float time){
        remainingTime = time;
    }

    public void SwitchTimerToBombPlanted(){
        timer.enabled = false;
        bombPlantedIndicator.SetActive(true);
    }
}
