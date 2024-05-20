using UnityEngine;

public class Barrier : MonoBehaviour
{
    void Update()
    {
        if(FindObjectOfType<RoundTimer>().buyPhaseEnded){
            Destroy(gameObject);
        }
    }
}
