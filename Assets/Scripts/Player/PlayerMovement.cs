using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    public float moveSpeed = 4f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookDir;
    bool isAiming = false;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(1)){
            isAiming = true;
            AimDownSights();
        }else if(Input.GetMouseButtonUp(1)){
            isAiming = false;
            AimDownSights();
        }
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        fieldOfView.SetAimDirection(lookDir);
        fieldOfView.SetOrigin(transform.position);
    }

    void SendMovementUpdate(){
        PlayerMovementData data = new PlayerMovementData(rb.position, rb.velocity);
    }
    public float SetMoveSpeed(float moveSpeed){
        this.moveSpeed = moveSpeed;
        return moveSpeed;
    }
    public void AimDownSights(){
        CinemachineVirtualCamera cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        CinemachineFramingTransposer camBody = FindObjectOfType<CinemachineFramingTransposer>();

        if(isAiming){
            if(cinemachine != null){
                cinemachine.m_Lens.OrthographicSize = 3.5f;
                camBody.m_XDamping += 1.5f;
                camBody.m_YDamping += 1.5f;
            }
            fieldOfView.SetFoV(30f);
            fieldOfView.SetViewDistance(9f);
            SetMoveSpeed(1.5f);
        }else{
            if(cinemachine != null){
                cinemachine.m_Lens.OrthographicSize = 2.8f;
                camBody.m_XDamping -= 1.5f;
                camBody.m_YDamping -= 1.5f;
            }
            fieldOfView.SetFoV(60f);
            fieldOfView.SetViewDistance(5.5f);
            SetMoveSpeed(4f);
        }
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