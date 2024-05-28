using TMPro;
using UnityEngine;

public class GetWinner : MonoBehaviour
{
    public TextMeshProUGUI winner;

    void Awake(){
        winner.text = FindObjectOfType<GameLoop>().GetLobbyWinner() + " win!";
    }
}
