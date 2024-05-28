
using System.Collections.Generic;
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

    string API_URL = "http://192.168.218.201:3000/rooms/A";


    async void Start()
    {
        string resPlayers = await GetPlayers();
        List<Players> players = JsonConvert.DeserializeObject<List<Players>>(resPlayers);
        
        foreach(var player in players){

            if(PlayerPrefs.GetString("username") == player.username){
                continue;
            }

            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            playerPrefab.name = player.username;
            playerPrefab.tag = player.team;

            Instantiate(playerPrefab, new Vector2(963, 535), Quaternion.identity);

        }

    }

    
    async Task<string> GetPlayers(){
        using var httpClient = new HttpClient();

        var res = await httpClient.GetAsync(API_URL);
        return await res.Content.ReadAsStringAsync();
   }

}
