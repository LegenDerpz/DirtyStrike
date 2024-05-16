using System.Collections;
using UnityEngine;

public class DirtBomb : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    //public Sprite plantedSprite;

    //TerroDirts
    public bool isPlanted = false;
    public float plantTime = 4f;
    float explodeTime = 10f;
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

            if(elapsedTime >= explodeTime){
                Debug.Log("Bomb has exploded.");
                StartCoroutine(Explode());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "TerroDirt" && !isPlanted){
            FindFirstObjectByType<Inventory>().TakeBomb();
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider){
        if(collider.tag == "Purifier" && isPlanted && FindFirstObjectByType<Purifier>().GetDefuseInput()){
            StartCoroutine(Defuse());
        }

        if(FindObjectOfType<Shooting>().isShooting){
            CancelDefuse();
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "Purifier"){
            CancelDefuse();
        }
    }

    public void ChangeSprite(){
        //spriteRenderer.sprite = plantedSprite;
        spriteRenderer.color = new Color(0.3867925f, 0.1511306f, 0.08940016f, 1f);
    }

    IEnumerator Explode(){
        hasExploded = true;
        FindObjectOfType<GameLoop>().AddTerroDirtScore();

        yield return new WaitForSeconds(5f);
        
        FindObjectOfType<GameLoop>().RoundEnd();
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
                FindObjectOfType<GameLoop>().FindWinCondition();
                yield break;
            }
            
            /*
            if(!FindFirstObjectByType<Purifier>().GetDefuseInput() && !defused){
                CancelDefuse();
            }
            */

            if(FindObjectOfType<Shooting>().isMoving){
                CancelDefuse();
            }
            
            yield return null;
        }
    }

    public void CancelDefuse(){
        isDefusing = false;
    }
}
