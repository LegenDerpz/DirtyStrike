using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public int purifierScore = 0;
    public int terrodirtScore = 0;


    void Awake()
    {
        //Debug.Log("Game Loop Initialized.");
    }
    public void RoundStart()
    {
        //Add Credits
        //Players go back to spawnpoint
        //Restart Round Timer
    }

    //If bomb is defused || Purifier team is eliminated
    //Bomb explodes || TerroDirt team is eliminated while bomb is not planted
    public void RoundEnd()
    {
        //Save Player Data (e.g. current weapons)
        for (int i = 0; i < FindObjectsOfType<PlayerData>().Length; i++)
        {
            FindObjectsOfType<PlayerData>()[i].SaveWeaponData();
        }
        //Restart Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //Add Score


        //Calculate Additional Credits Based On Performance
    }

    public void FindWinCondition()
    {
        for (int i = 0; i < FindObjectsOfType<PlayerStats>().Length; i++)
        {
            if ((FindObjectsOfType<PlayerStats>()[i].isDead && FindObjectsOfType<PlayerData>()[i].tag == "Purifier")
                || FindObjectOfType<DirtBomb>().hasExploded)
            {
                AddTerroDirtScore();
                RoundEnd();
            }
            else if ((FindObjectsOfType<PlayerStats>()[i].isDead && FindObjectsOfType<PlayerData>()[i].tag == "TerroDirt"
                && !FindObjectOfType<DirtBomb>().isPlanted) || FindObjectOfType<DirtBomb>().defused)
            {
                AddPurifierScore();
                RoundEnd();
            }
        }
    }

    public void AddTerroDirtScore()
    {
        terrodirtScore++;
    }

    public void AddPurifierScore()
    {
        purifierScore++;
    }

    //First to 3 Points Wins
    public void GameEnd()
    {
        //Show Game Summary GUI
        //Main Menu Button
    }
}
