using System;
using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public Inventory inventory;
    public GameObject waterBulletPrefab;
    public Rigidbody2D rb;

    public float bulletForce = 30f;
    float nextFireTime = 0f;

    int reloadAmount;
    public bool isReloading = false;
    int previousWeapon;
    public bool reloadInterrupted = false;
    public float reloadProgress;

    Vector2 lastPosition;
    bool isMoving = false;

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

            if(Time.time >= nextFireTime && inventory.GetWeapon(inventory.currentWeaponIndex).currentAmmo > 0){
                if(Input.GetButton("Fire1") && !isReloading){
                    Shoot();
                    nextFireTime = Time.time + 1f / inventory.GetWeapon(inventory.currentWeaponIndex).fireRate;
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

    void LateUpdate(){
        lastPosition = rb.position;
    }
        
    void Shoot(){
        float bulletSpread = UnityEngine.Random.Range(0f, inventory.GetWeapon(inventory.currentWeaponIndex).bulletSpread);
        float movementSpreadModifier = inventory.GetWeapon(inventory.currentWeaponIndex).bulletSpreadMovingModifier;

        if(isMoving){
            bulletSpread *= movementSpreadModifier;
        }

        Vector2 baseDirection = firePoint.up;

        float spreadAngle = bulletSpread * Mathf.Deg2Rad;
        Vector2 spreadOffset = UnityEngine.Random.insideUnitCircle * spreadAngle;

        Vector2 targetDirection = baseDirection + spreadOffset;
        targetDirection.Normalize();

        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion spreadRotation = Quaternion.Euler(0, 0, angle-90f);

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
