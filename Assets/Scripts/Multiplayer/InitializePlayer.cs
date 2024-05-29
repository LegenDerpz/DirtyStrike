
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class Players {
    public string username {get; set;}
    public string team {get; set;}
    public string room {get; set;}
}

public class InitializePlayer : MonoBehaviour
{

    [SerializeField]
    string API_URL = "http://localhost:3000/rooms/";

    async void Start()
    {        
        
        string resPlayers = await GetPlayers();
        Players[] players = JsonConvert.DeserializeObject<Players[]>(resPlayers).ToArray();
                
        GameObject[] TSpawns = GameObject.FindGameObjectsWithTag("TSpawn");
        GameObject[] PSpawns = GameObject.FindGameObjectsWithTag("PSpawn");

        string meUsername = PlayerPrefs.GetString("username");

        for(int i = 0, p = 0, t = 0; i < players.Length; i++){           
            
            Players player = players[i];

            GameObject playerPrefab;

            if(player.username == meUsername){
                playerPrefab = Resources.Load<GameObject>("Prefabs/Me");
            } else {
                playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            }

            playerPrefab.name = player.username;
            playerPrefab.tag = player.team;

            if(player.team.Equals("Purifier")){
                Instantiate(playerPrefab, PSpawns[p].transform.position, Quaternion.identity);
                p++;
            }
            
            if(player.team.Equals("TerroDirt")){
                Instantiate(playerPrefab, TSpawns[t].transform.position, Quaternion.identity);
                t++;
            }

        }       
    }

    
    async Task<string> GetPlayers(){

        string room = PlayerPrefs.GetString("currentJoinedRoom");
        API_URL += room;

        using var httpClient = new HttpClient();

        var res = await httpClient.GetAsync(API_URL);
        return await res.Content.ReadAsStringAsync();
   }

}
