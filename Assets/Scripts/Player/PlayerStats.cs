using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 1000f;
    float currentHealth;
    public GameObject deathEffect;
    Bullet bullet;
    Collider2D[] enemyMelee;

    public bool isDead = false;

    void Start(){
        currentHealth = health;
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;

        if(currentHealth <= 0){
            //Get bullet owner
            StartCoroutine(DieDelay());
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collider){
        bullet = collider.gameObject.GetComponent<Bullet>();

        if(isDead && bullet != null){
            Debug.Log(bullet.GetBulletOwner() + " killed " + gameObject.tag + "!");
            bullet.GetBulletOwnerBody().AddPlayerKills(1);
            bullet.GetBulletOwnerBody().GetComponent<Credits>().AddCredits(Credits.killReward);
            Debug.Log(bullet.GetBulletOwnerBody().GetComponent<Credits>().GetCredits());
        }

        if(isDead && enemyMelee != null){
            Debug.Log(enemyMelee[0].GetComponent<PlayerData>().GetUsername() + " killed " + gameObject.tag + "!");
            enemyMelee[0].GetComponent<PlayerData>().AddPlayerKills(1);
            enemyMelee[0].GetComponent<Credits>().AddCredits(Credits.killReward);
            Debug.Log(enemyMelee[0].GetComponent<Credits>().GetCredits());
        }
        Debug.Log(enemyMelee);
    }

    public void Die(){
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        isDead = true;

        if(bullet != null){
            
        }
        
        if(gameObject.CompareTag("Purifier")){
            if(GetComponent<Purifier>() != null){
                GetComponent<Purifier>().enabled = false;
            }
        }else if(gameObject.CompareTag("TerroDirt")){
            if(GetComponent<TerroDirt>() != null){
                GetComponent<TerroDirt>().enabled = false;
            }
        }
        
        if(GetComponent<PlayerControls>() != null){
            GetComponent<PlayerControls>().enabled = false;
        }
        if(GetComponent<Collider2D>() != null){
            GetComponent<Collider2D>().enabled = false;
        }
        StartCoroutine(DieDelay());
        FindObjectOfType<GameLoop>().FindWinCondition();
        //Destroy(gameObject);
    }

    IEnumerator DieDelay(){
        yield return new WaitForSeconds(1f);
    }

    public float GetCurrentHealth(){
        return currentHealth;
    }
    public void SetCurrentHealth(float currentHealth){
        this.currentHealth = currentHealth;
    }

    public void GetAttacker(GameObject attacker){
        enemyMelee = attacker.GetComponent<Combat>().GetMeleeCollider();
    }
}
