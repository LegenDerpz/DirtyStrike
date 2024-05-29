using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerManagement : MonoBehaviour
{

    [SerializeField]
    Uri uri = new Uri("http://localhost:7000");

    SocketIOUnity socket;

    GameObject playerObj;
    Vector2 playerPosition;
    float playerRotation;

    string currentUsername;
    string team;
    string room;

    void Awake(){

        playerObj = GameObject.FindGameObjectWithTag("Me");
        
        currentUsername = PlayerPrefs.GetString("username");
        team = PlayerPrefs.GetString("selectedTeam");
        room  = PlayerPrefs.GetString("currentJoinedRoom");

        playerObj.name = currentUsername;
        playerObj.tag = team;

    }

    void Start()
    {

        socket = new SocketIOUnity(uri, new SocketIOOptions{
            Query = new Dictionary<string, string>
            {
                {"username", currentUsername},
                {"team", team},
                {"room", room}
            }
        });
        socket.Connect();

        this.socket.JsonSerializer = new NewtonsoftJsonSerializer();

        socket.On("other_players_transform", (data) => {           
            UnityThread.executeInUpdate(() => {
                    
                    string resPlayerTransform = data.GetValue<string>();
                    PlayerTransform playerTransform = JsonConvert.DeserializeObject<PlayerTransform>(resPlayerTransform);
                    
                    GameObject[] playerTerroDirt = GameObject.FindGameObjectsWithTag("TerroDirt");
                    GameObject[] playerPurifier = GameObject.FindGameObjectsWithTag("Purifier");
                    
                    for (int i = 0; i < playerTerroDirt.Length; i++)
                    {
                        if(playerTerroDirt[i].name.Equals(playerTransform.username + "(Clone)")){
                            playerTerroDirt[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                            playerTerroDirt[i].GetComponent<Rigidbody2D>().rotation = playerTransform.player_rotation;
                        }
                    }

                    for (int i = 0; i < playerPurifier.Length; i++)
                    {
                        if(playerPurifier[i].name.Equals(playerTransform.username + "(Clone)")){
                            playerPurifier[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                            playerPurifier[i].GetComponent<Rigidbody2D>().rotation = playerTransform.player_rotation;
                        }
                    }

            });
        });
    }

    void Update()
    {

        string currentUsername = PlayerPrefs.GetString("username");

        playerPosition = playerObj.GetComponent<Transform>().position;
        playerRotation = playerObj.GetComponent<Rigidbody2D>().rotation;
        
        string playerTransform =  @"{{
            ""username"": ""{0}"",
            ""player_position"": {{""x"": {1}, ""y"": {2}}},
            ""player_rotation"": {3}
            }}";
        
        socket.Emit("player_transform", string.Format(playerTransform,  currentUsername, playerPosition.x, playerPosition.y, playerRotation));
    }
}
