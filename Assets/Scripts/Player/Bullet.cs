using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string bulletOwner;
    public PlayerData bulletOwnerBody;
    public AudioManager audioManager;
    public Rigidbody2D rb;
    public float damage = 20f;
    public float critChance = 0.1f;
    public float critMultiplier = 1.5f;
    public GameObject hitEffect;

    void Awake(){
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Gunshot");
    }

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
            Debug.Log(GetBulletOwner() + " hit " + player.tag);
        }
        
        //Destroy(effect, 0.5f);
        Destroy(gameObject);
    }

    public Vector2 GetBulletPosition(){
        return rb.position;
    }
    public Quaternion GetBulletRotation(){
        return transform.rotation;
    }
    public Transform GetBulletTransform(){
        return rb.transform;
    }
    public float GetBulletDamage(){
        return damage;
    }
    public string GetBulletOwner(){
        return bulletOwner;
    }
    public PlayerData GetBulletOwnerBody(){
        return bulletOwnerBody;
    }
}
