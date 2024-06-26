using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 1000f;
    float currentHealth;
    public GameObject deathEffect;
    Bullet bullet;
    GameObject enemyMeleeAttacker;
    public bool isDead = false;

    void Start(){
        currentHealth = health;
    }

    public void TakeDamage(float damage){
        currentHealth -= damage;

        if(currentHealth <= 0){
            StartCoroutine(DieDelay());
            Die();
            if(enemyMeleeAttacker != null){
                Debug.Log(enemyMeleeAttacker.GetComponent<PlayerData>().GetUsername() + " killed " +
                gameObject.tag);
                enemyMeleeAttacker.GetComponent<PlayerData>().AddPlayerKills(1);
                enemyMeleeAttacker.GetComponent<Credits>().AddCredits(Credits.killReward);
            }
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

    public void SetAttackerInfo(GameObject attacker){
        enemyMeleeAttacker = attacker.gameObject;
    }
    public GameObject GetAttackerInfo(){
        return enemyMeleeAttacker;
    }
}
