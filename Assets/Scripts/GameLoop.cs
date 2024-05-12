using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public int score = 0;


    public void RoundStart(){
        //Add Credits
        //Players go back to spawnpoint
        //Restart Round Timer
    }

    //If bomb is defused || Purifier team is eliminated
    //Bomb explodes || TerroDirt team is eliminated while bomb is not planted
    public void RoundEnd(){
        int i;
        for(i = 0; i < FindObjectsOfType<PlayerData>().Length; i++){
            FindObjectsOfType<PlayerData>()[i].SaveWeaponData();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log(i);
        //Add Score
        //Save Player Data (e.g. current weapons)
        //Calculate Additional Credits Based On Performance
    }

    //First to 3 Points Wins
    public void GameEnd(){
        //Show Game Summary GUI
        //Main Menu Button
    }
}
