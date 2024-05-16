using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public List<Weapon> weapons;
    public WeaponShop shop;
    public Weapon dirtBomb;
    public GameObject dirtBombPrefab;
    public int maxInventorySlots = 4;
    public int currentWeaponIndex = 0;
    public int currentAmmo;

    private void Start(){
        weapons =  new List<Weapon>();
        LoadWeapons();
        //weapons[0] = shop.rifle;
        
        try{
            int i = 0;
            for(i = 0; i < weapons.Count; i++){
                if(weapons[i] != null){
                    currentAmmo = weapons[i].magazineSize;
                    weapons[i].currentAmmo = currentAmmo;
                    weapons[i].magazineTotalSize = weapons[i].magazineSize * 3;
                }
            }
        }catch(NullReferenceException){}
    }

    private void Update(){
        try{
            if(GetWeapon(currentWeaponIndex) != null){
                currentAmmo = weapons[currentWeaponIndex].currentAmmo;
            }
        }catch(IndexOutOfRangeException){}catch(ArgumentOutOfRangeException){}
    }

    public void LoadWeapons(){
        weapons.Clear();

        for(int i = 0; i < maxInventorySlots; i++){
                weapons.Add(null);
        }

        string weaponDataString = PlayerData.LoadWeaponData();

        if(weaponDataString != null){
            CurrentWeaponData currentWeaponData = JsonUtility.FromJson<CurrentWeaponData>(weaponDataString);   
            
            if(currentWeaponData.hasDiedLastRound){
                Debug.Log("Died Last Round");
            }else{
                GetComponent<Inventory>().weapons[0] = currentWeaponData.primaryWeapon;
                GetComponent<Inventory>().weapons[1] = currentWeaponData.secondaryWeapon;
                Debug.Log("Didn't Die Last Round");
                Debug.Log(currentWeaponData.primaryWeapon);
                Debug.Log(currentWeaponData.secondaryWeapon);
            }
        }

        EquipStartingWeapon();
        weapons[2] = shop.knife;
        weapons[3] = null;
        GetComponent<PlayerStats>().isDead = false;
    }

    private void EquipStartingWeapon(){
        if(weapons[1] == null){
            weapons[1] = shop.classicPistol;
        }
    }

    public void AddWeapon(Weapon newWeapon){
        if(weapons.Contains(newWeapon)){
            return;
        }

        if(newWeapon.weaponClass == WeaponClass.Primary && GetWeaponCount(WeaponClass.Primary) >= 1){
            Debug.Log("Cannot equip more than one Primary Weapon!");
            return;
        }else if(newWeapon.weaponClass == WeaponClass.Secondary && GetWeaponCount(WeaponClass.Secondary) >= 1){
            Debug.Log("Cannot equip more than one Secondary Weapon!");
            return;
        }
        weapons.Add(newWeapon);
    }
    
    public void RemoveWeapon(Weapon weapon){
        weapons.Remove(weapon);
    }

    public Weapon GetWeapon(int index){
        if(index >= 0 && index < weapons.Count){
            return weapons[currentWeaponIndex];
        }
        return null;
    }
    public Weapon GetWeapon(){
        return weapons[currentWeaponIndex];
    }

    private int GetWeaponCount(WeaponClass weaponClass){
        int count = 0;
        foreach(Weapon weapon in weapons){
            if(weapon.weaponClass == weaponClass){
                count++;
            }
        }
        return count;
    }

    public int GetMagazineSize(){
        return weapons[currentWeaponIndex].magazineSize;
    }
    public int GetMagazineCount(){
        return weapons[currentWeaponIndex].magazineTotalSize;
    }

    public bool IsGun(){
        try{
            if(weapons[currentWeaponIndex].weaponClass == WeaponClass.Primary && GetWeapon(currentWeaponIndex) != null){
                return true;
            }else if(weapons[currentWeaponIndex].weaponClass == WeaponClass.Secondary && GetWeapon(currentWeaponIndex) != null){
                return true;
            }else{
                return false;
            }
        }catch(ArgumentOutOfRangeException){return false;}catch(NullReferenceException){return false;}
    }

    public void TakeBomb(){
        weapons[3] = dirtBomb;
    }
}
