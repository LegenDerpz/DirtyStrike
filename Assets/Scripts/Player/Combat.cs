using System;
using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Transform firePoint;
    public Inventory inventory;
    public Rigidbody2D rb;
    public AnimationHandler anim;
    public AudioManager audioManager;

    //General
    float nextAttackTime = 0f;
    public bool isAttacking = false;

    //Shooting
    public GameObject waterBulletPrefab;
    public float bulletForce = 30f;

    int reloadAmount;
    public bool isReloading = false;
    int previousWeapon;
    public bool reloadInterrupted = false;
    public float reloadProgress;

    //Melee
    public LayerMask layerMask;
    public float attackRange = 0.5f;
    Collider2D[] hitEnemies;

    //Transform
    Vector2 lastPosition;
    public bool isMoving = false;

    void Update()
    {        
        try{
            if(inventory.primaryMagTotalSize > 0){
                if(inventory.GetWeapon().weaponClass == WeaponClass.Primary && inventory.primaryMagTotalSize >= inventory.primaryMagAmmo){
                    reloadAmount = inventory.primaryMagAmmo - inventory.primaryCurrentAmmo;
                }else if(inventory.GetWeapon().weaponClass == WeaponClass.Primary && inventory.primaryMagTotalSize < inventory.primaryMagAmmo){
                    reloadAmount = Mathf.Min(inventory.primaryMagAmmo - inventory.primaryCurrentAmmo, inventory.primaryMagTotalSize);
                }
            }

            if(inventory.secondaryMagTotalSize > 0){
                if(inventory.GetWeapon().weaponClass == WeaponClass.Secondary && inventory.secondaryMagTotalSize >= inventory.secondaryMagAmmo){
                    reloadAmount = inventory.secondaryMagAmmo - inventory.secondaryCurrentAmmo;
                }else if(inventory.GetWeapon().weaponClass == WeaponClass.Secondary && inventory.secondaryMagTotalSize < inventory.secondaryMagAmmo){
                    reloadAmount = Mathf.Min(inventory.secondaryMagAmmo - inventory.secondaryCurrentAmmo, inventory.secondaryMagTotalSize);
                }
            }

            if(Time.time >= nextAttackTime && inventory.GetWeapon().weaponType != WeaponType.Bomb){
                if(Input.GetButton("Fire1") && !isReloading){
                    if(inventory.IsGun() && ((inventory.GetWeapon().weaponClass == WeaponClass.Primary && inventory.primaryCurrentAmmo > 0)
                        || (inventory.GetWeapon().weaponClass == WeaponClass.Secondary && inventory.secondaryCurrentAmmo > 0))){
                        Shoot();
                    }else if(inventory.GetWeapon().weaponType == WeaponType.Melee){
                        Attack();
                        anim.animator.SetTrigger("Attack");
                    }
                    isAttacking = true;
                    nextAttackTime = Time.time + 1f / inventory.GetWeapon(inventory.currentWeaponIndex).fireRate;
                }else{
                    isAttacking = false;
                }
            }
        
            if((Input.GetKeyDown(KeyCode.R) || inventory.GetWeapon().weaponClass == WeaponClass.Primary && inventory.primaryCurrentAmmo <= 0) && inventory.IsGun() && !isReloading){
                if(inventory.primaryMagTotalSize > 0 && !isReloading && inventory.primaryCurrentAmmo < inventory.primaryMagAmmo){
                    StartCoroutine(Reload());
                }
            }
            if((Input.GetKeyDown(KeyCode.R) || inventory.GetWeapon().weaponClass == WeaponClass.Secondary && inventory.secondaryCurrentAmmo <= 0) && inventory.IsGun() && !isReloading){
                if(inventory.secondaryMagTotalSize > 0 && !isReloading && inventory.secondaryCurrentAmmo < inventory.secondaryMagAmmo){
                    StartCoroutine(Reload());
                }
            }

            if(previousWeapon != inventory.currentWeaponIndex && isReloading){
                if(isReloading){
                    InterruptReload();
                }
            }
            previousWeapon = inventory.currentWeaponIndex;
            
            if(lastPosition != rb.position){
                isMoving = true;
            }else{
                isMoving = false;
            }

            if(anim.animator.GetBool("IsMoving")){
                audioManager.Play("Footsteps_P");
                Debug.Log("Moving");
            }else{
                //audioManager.Stop("Footsteps_P");
            }
        }catch(NullReferenceException){}
    }

    void FixedUpdate(){
        lastPosition = rb.position;
    }

    void Attack(){
        hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, attackRange, layerMask);

        foreach(Collider2D enemy in hitEnemies){
            Debug.Log("Hit " + enemy.tag);
            enemy.GetComponent<PlayerStats>().TakeDamage(inventory.GetWeapon().damage);
            enemy.GetComponent<PlayerStats>().SetAttackerInfo(gameObject);
        }
    }

    void OnDrawGizmosSelected(){
        if(firePoint == null){
            return;
        }
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }
        
    void Shoot(){
        float bulletSpread = UnityEngine.Random.Range(0f, inventory.GetWeapon(inventory.currentWeaponIndex).bulletSpread);
        float movementSpreadModifier = inventory.GetWeapon(inventory.currentWeaponIndex).bulletSpreadMovingModifier;

        if(isMoving){
            bulletSpread *= movementSpreadModifier;
        }else{
            bulletSpread = UnityEngine.Random.Range(0f, inventory.GetWeapon(inventory.currentWeaponIndex).bulletSpread);
        }

        Vector2 baseDirection = firePoint.up;

        float spreadAngle = bulletSpread * Mathf.Deg2Rad;
        Vector2 spreadOffset = UnityEngine.Random.insideUnitCircle * spreadAngle;

        Vector2 targetDirection = baseDirection + spreadOffset;
        targetDirection.Normalize();

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion spreadRotation = Quaternion.Euler(0, 0, angle-90f);

        waterBulletPrefab.GetComponent<Bullet>().bulletOwner = GetComponent<PlayerData>().username;
        waterBulletPrefab.GetComponent<Bullet>().bulletOwnerBody = GetComponent<PlayerData>();

        GameObject waterBullet = Instantiate(waterBulletPrefab, firePoint.position, spreadRotation);
        Rigidbody2D rb = waterBullet.GetComponent<Rigidbody2D>();

        if(inventory.GetWeapon().weaponType == WeaponType.Sniper){
            bulletForce = 60f;
        }else{
            bulletForce = 30f;
        }
        rb.AddForce(targetDirection * bulletForce, ForceMode2D.Impulse);

        if(inventory.GetWeapon().weaponClass == WeaponClass.Primary){
            inventory.primaryCurrentAmmo--;
        }else if(inventory.GetWeapon().weaponClass == WeaponClass.Secondary){
            inventory.secondaryCurrentAmmo--;
        }
        //audioManager.Play("Gunshot");
        StartCoroutine(audioManager.PlayCoroutine("Gunshot"));
    }

    IEnumerator Reload(){
        isReloading = true;
        Debug.Log("Reloading...");

        float reloadTime = inventory.GetWeapon().reloadTime;
        float elapsedTime = 0f;

        while(elapsedTime < reloadTime){
            if(reloadInterrupted){
                isReloading = false;
                Debug.Log("Reload Cancelled.");
                reloadInterrupted = false;
                yield break;
            }

            reloadProgress = elapsedTime / reloadTime;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isReloading = false;
        if(inventory.GetWeapon().weaponClass == WeaponClass.Primary){
            inventory.primaryMagTotalSize -= reloadAmount;
            inventory.primaryCurrentAmmo += reloadAmount;
        }else if(inventory.GetWeapon().weaponClass == WeaponClass.Secondary){
            inventory.secondaryMagTotalSize -= reloadAmount;
            inventory.secondaryCurrentAmmo += reloadAmount;
        }
        Debug.Log("Reloaded!");
    }

    public void InterruptReload(){
        reloadInterrupted = true;
    }
}
