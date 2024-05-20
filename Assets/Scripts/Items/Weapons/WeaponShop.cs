using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponShop : MonoBehaviour
{
    public string shopOwner;
    public GameObject shopOwnerObject;

    public Weapon secondaryWeapon1;
    public Weapon secondaryWeapon2;
    public Weapon secondaryWeapon3;

    public Weapon primaryWeapon1;
    public Weapon primaryWeapon2;
    public Weapon primaryWeapon3;

    public TextMeshProUGUI secondaryGunPrice_1;
    public TextMeshProUGUI secondaryGunPrice_2;
    public TextMeshProUGUI secondaryGunPrice_3;
    public TextMeshProUGUI primaryGunPrice_1;
    public TextMeshProUGUI primaryGunPrice_2;
    public TextMeshProUGUI primaryGunPrice_3;

    public TextMeshProUGUI playerCredits;

    void Start(){
        secondaryGunPrice_1.text = "Free";
        secondaryGunPrice_2.text = secondaryWeapon2.cost.ToString();
        secondaryGunPrice_3.text = secondaryWeapon3.cost.ToString();
        primaryGunPrice_1.text = primaryWeapon1.cost.ToString();
        primaryGunPrice_2.text = primaryWeapon2.cost.ToString();
        primaryGunPrice_3.text = primaryWeapon3.cost.ToString();
    }
    void Update(){
        if(shopOwnerObject != null){
            playerCredits.text = "Credits: " + shopOwnerObject.GetComponent<Credits>().GetCredits().ToString();
        }
    }

    public void buyWeapon(Weapon weapon){
        int weaponClass = 0;
        if(shopOwnerObject.GetComponent<Credits>().GetCredits() >= weapon.cost){
            if(weapon.weaponClass == WeaponClass.Primary){
                weaponClass = 0;
            }else if(weapon.weaponClass == WeaponClass.Secondary){
                weaponClass = 1;
            }
            shopOwnerObject.GetComponent<Inventory>().weapons[weaponClass] = weapon;
            shopOwnerObject.GetComponent<Credits>().RemoveCredits(weapon.cost);

            //Set Proper Ammo
            shopOwnerObject.GetComponent<Inventory>().InitializeWeapon(weapon);
        }else{
            Debug.Log("Insufficient Credits!");
        }
    }

    public string GetShopOwner(){
        return shopOwner;
    }
    public void SetShopOwner(string username){
        shopOwner = username;
    }

    public GameObject GetShopOwnerObject(){
        return shopOwnerObject;
    }
    public void SetShopOwnerObject(GameObject shopOwnerObject){
        this.shopOwnerObject = shopOwnerObject;
    }
}
