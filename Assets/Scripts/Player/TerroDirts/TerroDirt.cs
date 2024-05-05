using UnityEngine;

public class TerroDirt : MonoBehaviour
{
    public float health = 1000f;
    public GameObject deathEffect;

    public void TakeDamage(float damage){
        health -= damage;

        if(health <= 0){
            Die();
        }
    }

    public void Die(){
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
