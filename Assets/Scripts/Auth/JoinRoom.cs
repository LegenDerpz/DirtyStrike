using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinRoom : MonoBehaviour
{

    [SerializeField]
    string API_URL = "http://192.168.218.201:3000/rooms";

    public void Username(TMP_InputField inputField){
        string username = inputField.text;
        PlayerPrefs.SetString("username", username);
    }

    public void Team(TMP_Text inputField){
        string team = inputField.text;
        PlayerPrefs.SetString("selectedTeam", team);
    }

    public async void Join(TMP_InputField inputField){
        
        string currentJoinedRoom = inputField.text;
        PlayerPrefs.SetString("currentJoinedRoom", currentJoinedRoom);
        
        await joinRoom();
    }

    async Task joinRoom(){
        using var httpClient = new HttpClient();

        string username = PlayerPrefs.GetString("username");
        string room = PlayerPrefs.GetString("currentJoinedRoom");
        string selectedTeam = PlayerPrefs.GetString("selectedTeam");

        var res = await httpClient.PostAsync(API_URL, new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["username"] = username,
            ["team"] = selectedTeam,
            ["room"] = room
        }));

        if(res.IsSuccessStatusCode){
            SceneManager.LoadScene("TestMap");
        }

    }
}
