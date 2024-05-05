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

        TerroDirt terroDirt = collider.gameObject.GetComponent<TerroDirt>();
        if(terroDirt != null){
            if(Random.value <= critChance){
                Debug.Log("Critical Hit!");
                terroDirt.TakeDamage(damage * critMultiplier);
            }else{
                terroDirt.TakeDamage(damage);
            }
        }
        
        //Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
