using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 1000f;
    public GameObject deathEffect;

    public bool isDead = false;

    public void TakeDamage(float damage){
        health -= damage;

        if(health <= 0){
            Die();
        }
    }

    public void Die(){
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        isDead = true;

        FindObjectOfType<GameLoop>().FindWinCondition();
        
        if(gameObject.tag == "Purifier"){
            GetComponent<Purifier>().enabled = false;
        }else if(gameObject.tag == "TerroDirt"){
            GetComponent<TerroDirt>().enabled = false;
        }
        //Destroy(gameObject);
    }
}
