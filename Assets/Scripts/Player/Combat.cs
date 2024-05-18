using System;
using System.Collections;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Transform firePoint;
    public Inventory inventory;
    public Rigidbody2D rb;
    public AnimationHandler anim;

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

    //Transform
    Vector2 lastPosition;
    public bool isMoving = false;

    void Update()
    {        
        try{
            if(inventory.GetWeapon(inventory.currentWeaponIndex).magazineTotalSize > 0){
                if(inventory.GetWeapon(inventory.currentWeaponIndex).magazineTotalSize >= inventory.GetWeapon(inventory.currentWeaponIndex).magazineSize){
                    reloadAmount = inventory.GetWeapon(inventory.currentWeaponIndex).magazineSize - inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo;
                }else if(inventory.GetWeapon(inventory.currentWeaponIndex).magazineTotalSize < inventory.GetWeapon(inventory.currentWeaponIndex).magazineSize){
                    reloadAmount = Mathf.Min(inventory.GetWeapon(inventory.currentWeaponIndex).magazineSize - inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo, inventory.GetWeapon(inventory.currentWeaponIndex).magazineTotalSize);
                }
            }

            if(Time.time >= nextAttackTime && inventory.GetWeapon().weaponType != WeaponType.Bomb){
                if(Input.GetButton("Fire1") && !isReloading){
                    if(inventory.IsGun() && inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo > 0){
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
        
            if((Input.GetKeyDown(KeyCode.R) || inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo <= 0) && inventory.IsGun() && !isReloading){
                if(inventory.GetWeapon(inventory.currentWeaponIndex).magazineTotalSize > 0 && !isReloading && 
                    inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo < inventory.GetWeapon(inventory.currentWeaponIndex).magazineSize){
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
        }catch(NullReferenceException){}
    }

    void FixedUpdate(){
        lastPosition = rb.position;
    }

    void Attack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint.position, attackRange, layerMask);

        foreach(Collider2D enemy in hitEnemies){
            Debug.Log("Hit " + enemy.tag);
            enemy.GetComponent<PlayerStats>().TakeDamage(inventory.GetWeapon().damage);
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
        rb.AddForce(targetDirection * bulletForce, ForceMode2D.Impulse);
        inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo--;
    }

    IEnumerator Reload(){
        isReloading = true;
        Debug.Log("Reloading...");

        float reloadTime = inventory.GetWeapon(inventory.currentWeaponIndex).reloadTime;
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
        inventory.GetWeapon(inventory.currentWeaponIndex).magazineTotalSize -= reloadAmount;
        inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo += reloadAmount;
        Debug.Log("Reloaded!");
    }

    public void InterruptReload(){
        reloadInterrupted = true;
    }
}
