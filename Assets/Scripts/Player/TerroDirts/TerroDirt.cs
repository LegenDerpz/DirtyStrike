using UnityEngine;
using System;
using System.Collections;

public class TerroDirt : MonoBehaviour
{
    public Inventory inv;
    public DirtBomb dirtBomb;
    public Rigidbody2D rb;

    public float plantProgress;
    public bool isPlanting = false;

    void Update()
    {
        //Plant Bomb
        try{
            if(inv.GetWeapon(inv.currentWeaponIndex).weaponType == WeaponType.Bomb && Input.GetKeyDown(KeyCode.F) && FindObjectOfType<DirtBomb>() == null){
                StartCoroutine(PlantBomb());
            }   
        }catch(NullReferenceException){}
    }

    IEnumerator PlantBomb(){
        isPlanting = true;
        float elapsedTime = 0f;

        float plantTime = dirtBomb.plantTime;
        
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
                //Change Timer to Bomb Planted
                yield break;
            }

            if(gameObject.GetComponent<Shooting>().isMoving){
                CancelPlant();
            }else if(gameObject.GetComponent<Shooting>().isShooting){
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