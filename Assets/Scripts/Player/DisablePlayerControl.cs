using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerControl : MonoBehaviour
{   

    [SerializeField]
    PlayerControls playerControls;
    void Start()
    {
        if(gameObject.tag != "Me"){
            playerControls.enabled = false;
        }
    }

}
