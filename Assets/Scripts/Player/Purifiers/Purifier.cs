using UnityEngine;
using Cinemachine;
using System;

public class Purifier : MonoBehaviour
{
    public bool GetDefuseInput(){
        while(Input.GetKey(KeyCode.F)){
            return true;
        }
        return false;
    }
}