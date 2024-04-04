using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject waterBulletPrefab;

    public float bulletForce = 20f;

    
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            Shoot();
        }
    }

    void Shoot(){
        GameObject waterBullet = Instantiate(waterBulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = waterBullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
}
