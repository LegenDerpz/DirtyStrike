using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        /*
        if(socket.Connected){
            SendMovementUpdate();
        }
        */
    }

    void SendMovementUpdate(){
        PlayerMovementData data = new PlayerMovementData(rb.position, rb.velocity);
        //socket.Emit("playerMovement", data);
    }
}

public class PlayerMovementData{
    public Vector2 position;
    public Vector2 velocity;

    public PlayerMovementData(Vector2 position, Vector2 velocity = default){
        this.position = position;
        this.velocity = velocity;
    }
}