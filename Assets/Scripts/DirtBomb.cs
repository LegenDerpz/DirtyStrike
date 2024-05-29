using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirtBomb : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite plantedSprite;
    public Sprite explodeSprite;
    public Sprite defuseSprite;


    //TerroDirts
    public bool isPlanted = false;
    public float plantTime = 4f;
    public float explodeTime = 20f;
    float elapsedTime = 0f;
    public bool hasExploded = false;
    
    //Purifiers
    public float defuseTime = 7.5f;
    public bool isDefusing = false;
    public float defuseProgress;
    public bool defused = false;

    void Update(){
        if(isPlanted && !hasExploded){
            elapsedTime += Time.deltaTime;

            if(elapsedTime < 10f){
                ChangeSpriteExplode();
            }

            if(elapsedTime >= explodeTime){
                Debug.Log("Bomb has exploded.");
                Explode();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.CompareTag("TerroDirt") && !isPlanted){
            FindFirstObjectByType<Inventory>().TakeBomb();
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider){
        if(collider.CompareTag("Purifier") && isPlanted && FindFirstObjectByType<Purifier>().GetDefuseInput()){
            StartCoroutine(Defuse());
        }

        if(FindObjectOfType<Combat>().isAttacking || FindObjectOfType<Combat>().isMoving){
            CancelDefuse();
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.CompareTag("Purifier")){
            CancelDefuse();
        }
    }

    public void ChangeSpritePlanted(){
        spriteRenderer.sprite = plantedSprite;
        //spriteRenderer.color = new Color(0.3867925f, 0.1511306f, 0.08940016f, 1f);
    }
    
    public void ChangeSpriteExplode(){
        spriteRenderer.sprite = explodeSprite;
    }

    public void ChangeSpriteDefuse(){
        spriteRenderer.sprite = defuseSprite;
    }

    void Explode(){
        hasExploded = true;
        GetComponent<Animator>().SetBool("HasExploded", true);
        GetComponent<Animator>().SetTrigger("Explode");
        FindObjectOfType<AudioManager>().Play("BombExplode");
        StartCoroutine(MudSplatter());
        
        FindObjectOfType<GameLoop>().FindWinCondition();
    }

    IEnumerator Defuse(){
        isDefusing = true;
        float elapsedTime = 0f;

        while(elapsedTime < defuseTime){

            if (!isDefusing)
            {
                yield break;
            }

            defuseProgress = elapsedTime / defuseTime;
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= defuseTime){
                CancelDefuse();
                defused = true;
                FindObjectOfType<AudioManager>().Play("BombDefused");
                ChangeSpriteDefuse();
                FindObjectOfType<GameLoop>().FindWinCondition();
                yield break;
            }      
            
            yield return null;
        }
    }

    public void CancelDefuse(){
        isDefusing = false;
    }

    IEnumerator MudSplatter(){
        SceneManager.LoadSceneAsync("DirtSplatter", LoadSceneMode.Additive);

        yield return new WaitForSeconds(5f);

        SceneManager.UnloadSceneAsync("DirtSplatter");
    }
}
