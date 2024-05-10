using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Inventory inv;
    public Shooting shooting;
    public TextMeshProUGUI weapon;
    public TextMeshProUGUI currentAmmo;
    public Image ammoIcon;
    public Slider reloadCircle;
    public Slider plantCircle;
    public Slider defusingCirle;
    

    void Update(){
        try{
            if(inv.currentWeaponIndex > 1 && !inv.IsGun()){
                currentAmmo.text = "";
                weapon.text = inv.GetWeapon(inv.currentWeaponIndex).name.ToString();
                ammoIcon.enabled = false;
            }else{
                weapon.text = inv.GetWeapon(inv.currentWeaponIndex).name.ToString();
                currentAmmo.text = inv.GetWeapon(inv.currentWeaponIndex).currentAmmo.ToString() + "/" + inv.GetWeapon(inv.currentWeaponIndex).magazineTotalSize.ToString();
                ammoIcon.enabled = true;
            }

            if(shooting.isReloading && inv.IsGun()){
                ammoIcon.enabled = false;
                reloadCircle.gameObject.SetActive(true);
                reloadCircle.value = shooting.reloadProgress;
            }
            if(!shooting.isReloading && inv.IsGun()){
                ammoIcon.enabled = true;
                reloadCircle.gameObject.SetActive(false);
            }  
        }catch(NullReferenceException){}
    }

    void FixedUpdate(){
        try{
            //TerroDirt Planting
            if(FindObjectOfType<TerroDirt>().isPlanting){
                plantCircle.gameObject.SetActive(true);
                plantCircle.value = FindObjectOfType<TerroDirt>().plantProgress;
            }else{
                plantCircle.gameObject.SetActive(false);
            }

            //Purifier Defusing
            if(FindObjectOfType<DirtBomb>().isDefusing && FindObjectOfType<DirtBomb>() != null){
                defusingCirle.gameObject.SetActive(true);
                defusingCirle.value = FindObjectOfType<DirtBomb>().defuseProgress;
            }else{
                defusingCirle.gameObject.SetActive(false);
            }
        }catch(NullReferenceException){}
    }
}
