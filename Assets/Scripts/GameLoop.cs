using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public int purifierScore = 0;
    public int terrodirtScore = 0;

    string lobbyName = "lobbyX";
    int deadPurifierPlayers;
    int deadTerroDirtPlayers;

    void Start(){
        GameStart();
    }
    
    public void GameStart()
    {
        if(GetPurifierScore() == 0 && GetTerroDirtScore() == 0){
            //Add Credits
            foreach(PlayerData player in FindObjectsOfType<PlayerData>()){
                PlayerPrefs.SetInt(player.username + "_" + "Credits", Credits.startingCredits);
                PlayerPrefs.Save();
            }
            ResetKills();
            Debug.Log("Game Start!");
        }
        
    }

    //If bomb is defused || Purifier team is eliminated
    //Bomb explodes || TerroDirt team is eliminated while bomb is not planted || Time runs out and bomb is not planted
    public void RestartRound()
    {
        //Save Player Data (e.g. current weapons)
        for (int i = 0; i < FindObjectsOfType<PlayerData>().Length; i++)
        {
            FindObjectsOfType<PlayerData>()[i].SaveWeaponData();
        }

        
        //Restart Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //Calculate Additional Credits Based On Performance
    }

    public void FindWinCondition()
    {
        foreach(PlayerStats player in FindObjectsOfType<PlayerStats>()){
            if(player.CompareTag("Purifier") && player.isDead){
                deadPurifierPlayers++;
            }else if(player.CompareTag("TerroDirt") && player.isDead){
                deadTerroDirtPlayers++;
            }
        }

        if(deadPurifierPlayers >= GameObject.FindGameObjectsWithTag("Purifier").Length
            || (GameObject.FindGameObjectWithTag("DirtBomb") != null && FindObjectOfType<DirtBomb>().hasExploded))
        {
            Debug.Log("Wazzup");
            AddTerroDirtScore();
            StartCoroutine(Delay());
            if(GetPurifierScore() < 3 || GetTerroDirtScore() < 3){
                RestartRound();
            }
        }else if((deadTerroDirtPlayers >= GameObject.FindGameObjectsWithTag("TerroDirt").Length && !FindObjectOfType<DirtBomb>().isPlanted && FindObjectOfType<DirtBomb>() != null)
            || FindObjectOfType<DirtBomb>().defused
            || (!FindObjectOfType<DirtBomb>().isPlanted && FindObjectOfType<RoundTimer>().timeRanOut)){
            Debug.Log("Hallooo");
            AddPurifierScore();
            StartCoroutine(Delay());
            if(GetPurifierScore() < 3 || GetTerroDirtScore() < 3){
                RestartRound();
            }
        }

        if(GetPurifierScore() >= 3 || GetTerroDirtScore() >= 3){
            StartCoroutine(Delay());
            GameEnd();
        }

    }

    IEnumerator Delay(){
        yield return new WaitForSeconds(10f);
        Debug.Log("Delay");
    }

    public void AddTerroDirtScore()
    {
        //terrodirtScore++;
        PlayerPrefs.SetInt(lobbyName + "_" + "TerroDirt Score", GetTerroDirtScore() + 1);
    }

    public void AddPurifierScore()
    {
        //purifierScore++;
        PlayerPrefs.SetInt(lobbyName + "_" + "Purifier Score",  GetPurifierScore() + 1);
        PlayerPrefs.Save();
    }

    //First to 3 Points Wins
    public void GameEnd()
    {
        //Show Game Summary GUI
        //Main Menu Button
        ResetScore();
        ResetKills();
    }

    public void ResetScore(){
        PlayerPrefs.SetInt(lobbyName + "_" + "Purifier Score", 0);
        PlayerPrefs.SetInt(lobbyName + "_" + "TerroDirt Score", 0);
    }
    public void ResetKills(){
        foreach(PlayerData player in FindObjectsOfType<PlayerData>()){
            PlayerPrefs.SetInt(player.username + "_" + "Kills", 0);
        }
    }

    public int GetPurifierScore(){
        return PlayerPrefs.GetInt(lobbyName + "_" + "Purifier Score");
    }
    public int GetTerroDirtScore(){
        return PlayerPrefs.GetInt(lobbyName + "_" + "TerroDirt Score");
    }

}
