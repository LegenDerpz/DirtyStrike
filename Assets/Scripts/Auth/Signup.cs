using UnityEngine;
using System.Net.Http;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class Signup : MonoBehaviour
{

    [SerializeField]
    TMP_InputField username;
    
    [SerializeField]
    TMP_InputField password;

    [SerializeField]
    Button signupBtn;
    string API_URL = "http://localhost:3000/auth/signup";

    void Start()
    {
        signupBtn.onClick.AddListener(_Signup);
    }

    async void _Signup(){
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
