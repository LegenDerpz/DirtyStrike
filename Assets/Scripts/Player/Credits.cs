using UnityEngine;

public class Credits : MonoBehaviour
{
    public Inventory inv;
    public PlayerData playerData;
    public static int startingCredits = 9000;

    public static int killReward = 200;
    public static int plantReward = 300;
    public static int roundWinReward = 1200;
    public static int roundLossReward = 800;

    public void AddCredits(int creditsToAdd){
        PlayerPrefs.SetInt(playerData.username + "_" + "Credits", GetCredits() + creditsToAdd);
    }

    public void AddTeamCredits(int creditsToAdd, string tagName){
        foreach(GameObject player in GameObject.FindGameObjectsWithTag(tagName)){
            if(player.GetComponent<PlayerData>() != null){
                PlayerPrefs.SetInt(player.GetComponent<PlayerData>().username + "_" + "Credits", GetCredits() + creditsToAdd);
            }
        }
    }

    public void RemoveCredits(int creditsToRemove){
        PlayerPrefs.SetInt(playerData.username + "_" + "Credits", GetCredits() - creditsToRemove);
    }

    public int GetCredits(){
        return PlayerPrefs.GetInt(playerData.username + "_" + "Credits");
    }
}
