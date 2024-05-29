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

    InitializePlayer initializePlayer;

    string currentUsername;

    void Awake(){
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.Play("Gunshot");

        initializePlayer = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<InitializePlayer>();
        currentUsername = PlayerPrefs.GetString("username");
    }

    void OnCollisionEnter2D(Collision2D collider){        
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        PlayerStats player = collider.gameObject.GetComponent<PlayerStats>();
        if(player != null){
            if(UnityEngine.Random.value <= critChance){
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

    void Update(){

        Vector2 bulletPosition = gameObject.transform.position;

        string bulletTransform = @"
        {{
            ""username"": ""{0}"",
            ""bullet_position"": {{""x"": {1}, ""y"": {2}}}
        }}";

        initializePlayer.Socket.Emit("bullet_transfrom", string.Format(bulletTransform, currentUsername, bulletPosition.x, bulletPosition.y));
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
