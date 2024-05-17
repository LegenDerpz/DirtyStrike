using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 1000f;
    float currentHealth;
    public GameObject deathEffect;

    public bool isDead = false;

    void Start(){
        currentHealth = health;
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;

        if(currentHealth <= 0){
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
