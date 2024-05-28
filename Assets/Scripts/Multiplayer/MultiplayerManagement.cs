using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class MultiplayerManagement : MonoBehaviour
{

    Uri uri = new Uri("http://localhost:7000");

    SocketIOUnity socket;

    GameObject playerObj;
    Vector2 playerPosition;
    Quaternion playerRotation;

    void Start()
    {

        string currentUsername = PlayerPrefs.GetString("username");

        socket = new SocketIOUnity(uri, new SocketIOOptions{
            Query = new Dictionary<string, string>
            {
                {"username", currentUsername},
                {"team", "Purifier"},
                {"room", "A"}
            }
        });
        socket.Connect();

        this.socket.JsonSerializer = new NewtonsoftJsonSerializer();

        playerObj = GameObject.FindGameObjectWithTag("Me");

        socket.On("other_players_transform", (data) => {           
            UnityThread.executeInUpdate(() => {

                    string resPlayerTransform = data.GetValue<string>();
                    PlayerTransform playerTransform = JsonConvert.DeserializeObject<PlayerTransform>(resPlayerTransform);
                    GameObject[] playerTerroDirt = GameObject.FindGameObjectsWithTag("TerroDirt");
                    GameObject[] playerPurifier = GameObject.FindGameObjectsWithTag("Purifier");
                    
                    for (int i = 0; i < playerTerroDirt.Length; i++)
                    {
                        if(playerTerroDirt[i].name == playerTransform.username+"(Clone)"){
                            playerTerroDirt[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                            playerTerroDirt[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                        }
                    }

                    for (int i = 0; i < playerPurifier.Length; i++)
                    {
                        if(playerPurifier[i].name == playerTransform.username+"(Clone)"){
                            playerPurifier[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                            playerPurifier[i].transform.position = new Vector2(playerTransform.player_position.x, playerTransform.player_position.y);
                        }
                    }

            });
        });
    }

    void Update()
    {

        playerPosition = playerObj.GetComponent<Transform>().position;
        playerRotation = playerObj.GetComponent<Transform>().rotation;
        
        string currentUsername = PlayerPrefs.GetString("username");

        string playerTransform =  @"{{
            ""username"": ""{5}"",
            ""player_position"": {{""x"": {0}, ""y"": {1}}},
            ""player_rotation"": {{""x"": {2}, ""y"": {3}, ""z"": {4}}}
            }}";

        socket.Emit("player_transform", string.Format(playerTransform, playerPosition.x, playerPosition.y, playerRotation.x, playerRotation.y, playerRotation.z, currentUsername));
    }
}
