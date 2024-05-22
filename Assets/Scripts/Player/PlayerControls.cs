using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.U2D.Animation;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControls : MonoBehaviour
{
    public FieldOfView fieldOfView;
    public Inventory inventory;
    Weapon currentWeapon;

    public Animator animator;
    public GameObject equippedWeapon;
    public GameObject leftHandSprite;
    public GameObject leftArmSprite;
    Sprite weaponSprite;

    public float moveSpeed = 3f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;
    Vector2 lookDir;
    bool isAiming = false;

    bool shopOpened = false;

    void Start(){
        if(inventory.weapons[0] != null){
            SelectWeapon(0);
        }else{
            SelectWeapon(1);
        }
        animator.SetBool("IsGun", true);
        animator.SetBool("IsMelee", false);
        ChangeWeaponSprite(inventory.GetWeapon());
    }

    void Update()
    {
        if(!isAiming && inventory.GetWeapon(inventory.currentWeaponIndex) != null){
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

        if(Input.GetKeyDown(KeyCode.B) && !shopOpened && !FindObjectOfType<RoundTimer>().buyPhaseEnded){
            StartCoroutine(OpenWeaponShop());
        }
        if(Input.GetKeyDown(KeyCode.B) && shopOpened){
            CloseWeaponShop();
        }
    }

    void LateUpdate(){
        if(inventory.currentWeaponIndex < 0){
                SelectWeapon(0);
        }
        if(inventory.currentWeaponIndex > inventory.weapons.Count - 1){
                SelectWeapon(inventory.weapons.Count - 1);
        }
        if(inventory.GetWeapon(inventory.currentWeaponIndex) == null){
               SelectWeapon(inventory.weapons.Count - 2);
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

    public void SetMoveSpeed(float moveSpeed){
        this.moveSpeed = moveSpeed;
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
        try{
            currentWeapon = inventory.GetWeapon(inventory.currentWeaponIndex);
            inventory.currentWeaponIndex = index;
            CameraZoomOut(currentWeapon.cameraUnscoped, 1.5f, 1.5f);

            SpriteResolver spriteResolver = leftArmSprite.GetComponent<SpriteResolver>();

            if((inventory.GetWeapon().weaponClass == WeaponClass.Melee || inventory.GetWeapon().weaponClass == WeaponClass.Bomb) && inventory.GetWeapon() != null){
                animator.SetBool("IsMelee", true);
                animator.SetBool("IsGun", false);

                ChangeWeaponSprite(inventory.GetWeapon());
                equippedWeapon.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                leftHandSprite.gameObject.SetActive(false);

                if(gameObject.CompareTag("Purifier")){
                    spriteResolver.SetCategoryAndLabel("L_Arm", "Purifier Left Arm Rested");
                }else if(gameObject.CompareTag("TerroDirt")){
                    spriteResolver.SetCategoryAndLabel("L_Arm", "TerroDirt Left Arm Rested");
                }
            }else if(inventory.IsGun() && inventory.GetWeapon() != null){
                animator.SetBool("IsGun", true);
                animator.SetBool("IsMelee", false);

                ChangeWeaponSprite(inventory.GetWeapon());
                equippedWeapon.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
                leftHandSprite.gameObject.SetActive(true);

                if(gameObject.CompareTag("Purifier")){
                    spriteResolver.SetCategoryAndLabel("L_Arm", "Purifier Left Arm");
                }else if(gameObject.CompareTag("TerroDirt")){
                    spriteResolver.SetCategoryAndLabel("L_Arm", "TerroDirt Left Arm");
                }
            }
            
        }catch(IndexOutOfRangeException){}catch(NullReferenceException){}catch(ArgumentOutOfRangeException){}
    }

    public void ChangeWeaponSprite(Weapon weapon){
        weaponSprite = weapon.sprite;
        equippedWeapon.gameObject.GetComponent<SpriteRenderer>().sprite = weaponSprite;
    }

    IEnumerator OpenWeaponShop(){
        SceneManager.LoadSceneAsync("WeaponShop", LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.2f);
        GetComponent<Combat>().enabled = false;
        FindObjectOfType<WeaponShop>().SetShopOwnerObject(gameObject);
        shopOpened = true;
    }
    public void CloseWeaponShop(){
        SceneManager.UnloadSceneAsync("WeaponShop");
        GetComponent<Combat>().enabled = true;
        //FindObjectOfType<WeaponShop>().SetShopOwnerObject(null);
        shopOpened = false;
    }

    public bool GetShopState(){
        return shopOpened;
    }
}