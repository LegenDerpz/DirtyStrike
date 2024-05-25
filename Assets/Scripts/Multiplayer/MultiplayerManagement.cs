using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient.Newtonsoft.Json;
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
        socket = new SocketIOUnity(uri);
        socket.Connect();

        this.socket.JsonSerializer = new NewtonsoftJsonSerializer();

        playerObj = GameObject.FindGameObjectWithTag("Me");

        socket.On("other_players_transform", (data) => {
            UnityThread.executeInUpdate(() => {

            });
        });
    }

    void Update()
    {

        playerPosition = playerObj.GetComponent<Transform>().position;
        playerRotation = playerObj.GetComponent<Transform>().rotation;

        string playerTransform =  @"{{
            username: ""awit"",
            player_position: {{x: {0}, y: {1}}}
            player_rotation: {{x: {2}, y: {3}, z: {4}}}
            }}";

        socket.Emit("player_transform", string.Format(playerTransform, playerPosition.x, playerPosition.y, playerRotation.x, playerRotation.y, playerRotation.z));
    }
}
