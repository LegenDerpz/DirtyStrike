using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Inventory inv;
    public TextMeshProUGUI weapon;
    public TextMeshProUGUI currentAmmo;
    public Image ammoIcon;

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
        }catch(NullReferenceException){}
    }
}
