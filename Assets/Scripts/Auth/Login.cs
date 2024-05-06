using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class Login : MonoBehaviour
{

    [SerializeField]
    TMP_InputField username;
    
    [SerializeField]
    TMP_InputField password;
    
    [SerializeField]
    Button loginBtn;

    string API_URL = "http://localhost:3000/auth/login";


    void Start()
    {
        loginBtn.onClick.AddListener(_Login);
    }

    async void _Login(){
        using var httpClient = new HttpClient();

        var res = await httpClient.PostAsync(API_URL, new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["username"] = username.text,
            ["password"] = password.text
        }));

        if(res.IsSuccessStatusCode){
            SceneManager.LoadScene(1);
        }

   }
}
