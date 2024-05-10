using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementData : MonoBehaviour
{
    public Vector2 position;
    public Rigidbody2D rb;
    public float rotation;

    public PlayerMovementData(Vector2 position, float rotation){
        this.position = position;
        this.rotation = rotation;
    }

    void SendMovementUpdate(){
        PlayerMovementData data = new PlayerMovementData(rb.position, rb.rotation);
    }
}
