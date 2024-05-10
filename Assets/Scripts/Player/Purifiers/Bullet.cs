using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 20f;
    public float critChance = 0.1f;
    public float critMultiplier = 1.5f;
    public GameObject hitEffect;

    void OnCollisionEnter2D(Collision2D collider){        
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        PlayerStats player = collider.gameObject.GetComponent<PlayerStats>();
        if(player != null){
            if(Random.value <= critChance){
                Debug.Log("Critical Hit!");
                player.TakeDamage(damage * critMultiplier);
            }else{
                player.TakeDamage(damage);
            }
        }
        
        //Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
