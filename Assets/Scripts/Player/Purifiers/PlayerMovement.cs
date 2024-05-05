using UnityEngine;
using Cinemachine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private FieldOfView fieldOfView;
    public Inventory inventory;
    Weapon currentWeapon;

    public float moveSpeed = 3f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookDir;
    bool isAiming = false;


    void Update()
    {
        if(!isAiming){
            moveSpeed = inventory.GetWeapon(inventory.currentWeaponIndex).moveSpeed;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(inventory.IsGun()){
            if(Input.GetMouseButtonDown(1)){
                isAiming = true;
                AimDownSights();
            }else if(Input.GetMouseButtonUp(1)){
                isAiming = false;
                AimDownSights();
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            SelectWeapon(0);
        }else if(Input.GetKeyDown(KeyCode.Alpha2)){
            SelectWeapon(1);
        }else if(Input.GetKeyDown(KeyCode.Alpha3)){
            SelectWeapon(2);
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f){
            SelectWeapon(--inventory.currentWeaponIndex);
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f){
            SelectWeapon(++inventory.currentWeaponIndex);
        }
        
    }

    void LateUpdate(){
        if(inventory.currentWeaponIndex < 0){
            SelectWeapon(0);
        }
        if(inventory.currentWeaponIndex > inventory.weapons.Count - 1){
            SelectWeapon(inventory.weapons.Count - 1);
        }
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * inventory.GetWeapon(inventory.currentWeaponIndex).moveSpeed * Time.fixedDeltaTime);

        lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        fieldOfView.SetAimDirection(lookDir);
        fieldOfView.SetOrigin(transform.position);
    }

    public float SetMoveSpeed(float moveSpeed){
        this.moveSpeed = moveSpeed;
        return moveSpeed;
    }

    public void CameraZoomIn(float lensOrthoSize, float m_XDamping, float m_YDamping){
        CinemachineVirtualCamera cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        CinemachineFramingTransposer camBody = FindObjectOfType<CinemachineFramingTransposer>();
        if(cinemachine != null){
            cinemachine.m_Lens.OrthographicSize = lensOrthoSize;
            camBody.m_XDamping += m_XDamping;
            camBody.m_YDamping += m_YDamping;
        }
    }
    public void CameraZoomOut(float lensOrthoSize, float m_XDamping, float m_YDamping){
        CinemachineVirtualCamera cinemachine = FindObjectOfType<CinemachineVirtualCamera>();
        CinemachineFramingTransposer camBody = FindObjectOfType<CinemachineFramingTransposer>();
        if(cinemachine != null){
            cinemachine.m_Lens.OrthographicSize = lensOrthoSize;
            camBody.m_XDamping -= m_XDamping;
            camBody.m_YDamping -= m_YDamping;
        }
    }
    public void AimDownSights(){
        currentWeapon = inventory.GetWeapon(inventory.currentWeaponIndex);

        if(isAiming){
            CameraZoomIn(currentWeapon.cameraScoped, 1.5f, 1.5f);
            fieldOfView.SetFoV(currentWeapon.scopeFov);
            fieldOfView.SetViewDistance(currentWeapon.scopeRange);
            SetMoveSpeed(inventory.GetWeapon(inventory.currentWeaponIndex).moveSpeed / 2.5f);
        }else{
            CameraZoomOut(currentWeapon.cameraUnscoped, 1.5f, 1.5f);
            fieldOfView.SetFoV(60f);
            fieldOfView.SetViewDistance(5.5f);
            SetMoveSpeed(inventory.GetWeapon(inventory.currentWeaponIndex).moveSpeed);
        }
    }

    public void SelectWeapon(int index){
        /*
        if(index < 0 || index >= inventory.weapons.Count){
            throw new IndexOutOfRangeException("Index out of bounds!");
        }*/
        try{
            currentWeapon = inventory.GetWeapon(inventory.currentWeaponIndex);
            inventory.currentWeaponIndex = index;
            CameraZoomOut(currentWeapon.cameraUnscoped, 1.5f, 1.5f);

            Debug.Log("Current Weapon: " + currentWeapon.name);
        }catch(IndexOutOfRangeException){}catch(NullReferenceException){}catch(ArgumentOutOfRangeException){}
        
    }
    void SendMovementUpdate(){
        PlayerMovementData data = new PlayerMovementData(rb.position, rb.velocity);
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