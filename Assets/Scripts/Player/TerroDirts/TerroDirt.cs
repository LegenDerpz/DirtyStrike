using UnityEngine;
using System;
using System.Collections;

public class TerroDirt : MonoBehaviour
{
    public Inventory inv;
    public DirtBomb dirtBomb;
    public Rigidbody2D rb;

    public bool canPlant = false;
    public float plantProgress;
    public bool isPlanting = false;

    void Update()
    {
        //Plant Bomb
        try{
            if(inv.GetWeapon().weaponType == WeaponType.Bomb && Input.GetKeyDown(KeyCode.F) && FindObjectOfType<DirtBomb>() == null
            && canPlant && FindObjectOfType<RoundTimer>().buyPhaseEnded){
                StartCoroutine(PlantBomb());
            }   
        }catch(NullReferenceException){}
    }

    IEnumerator PlantBomb(){
        isPlanting = true;
        float elapsedTime = 0f;

        float plantTime = dirtBomb.plantTime;
        FindObjectOfType<AudioManager>().Play("BombPlanting");

        while(elapsedTime < plantTime){

            if(!isPlanting){
                yield break;
            }

            plantProgress = elapsedTime / plantTime;
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= plantTime){
                CancelPlant();
                Instantiate(inv.GetWeapon(3).prefab, rb.position, Quaternion.identity);
                FindObjectOfType<DirtBomb>().isPlanted = true;
                inv.weapons[3] = null;
                FindObjectOfType<DirtBomb>().ChangeSprite();

                if(inv.weapons[0] != null){
                    GetComponent<AnimationHandler>().animator.SetBool("IsGun", true);
                    GetComponent<AnimationHandler>().animator.SetBool("IsMelee", false);
                    GetComponent<PlayerControls>().SelectWeapon(0);
                    GetComponent<PlayerControls>().ChangeWeaponSprite(inv.GetWeapon());
                }else if(inv.weapons[0] == null && inv.weapons[1] != null){
                    GetComponent<AnimationHandler>().animator.SetBool("IsGun", true);
                    GetComponent<AnimationHandler>().animator.SetBool("IsMelee", false);
                    GetComponent<PlayerControls>().SelectWeapon(1);
                    GetComponent<PlayerControls>().ChangeWeaponSprite(inv.GetWeapon());
                }else{
                    GetComponent<PlayerControls>().SelectWeapon(2);
                    GetComponent<AnimationHandler>().animator.SetBool("IsGun", false);
                    GetComponent<AnimationHandler>().animator.SetBool("IsMelee", true);
                    GetComponent<PlayerControls>().ChangeWeaponSprite(inv.GetWeapon());
                }
                //Change Timer to Bomb Planted
                yield break;
            }

            if(gameObject.GetComponent<Combat>().isMoving){
                CancelPlant();
            }else if(gameObject.GetComponent<Combat>().isAttacking){
                CancelPlant();
            }else if(inv.currentWeaponIndex != 3 && inv.GetWeapon(inv.currentWeaponIndex) != null){
                CancelPlant();
            }

            yield return null;
        }  
    }

    public void CancelPlant(){
        isPlanting = false;
    }
}