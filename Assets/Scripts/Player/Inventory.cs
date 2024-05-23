using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    public List<Weapon> weapons;
    public Weapon startingWeapon;
    public Weapon melee;
    public Weapon dirtBomb;
    public GameObject dirtBombPrefab;
    public int maxInventorySlots = 4;
    public int currentWeaponIndex = 1;

    public int primaryCurrentAmmo;
    public int primaryMagAmmo;
    public int primaryMagTotalSize;

    public int secondaryCurrentAmmo;
    public int secondaryMagAmmo;
    public int secondaryMagTotalSize;

    private void Start(){
        weapons =  new List<Weapon>();
        LoadWeapons();
        
        try{
            if(weapons[0] != null){
                InitializeWeapon(weapons[0]);
            }

            if(weapons[1] != null){
                InitializeWeapon(weapons[1]);
            }
        }catch(NullReferenceException){}
    }

    private void Update(){
        /*
        try{
            if(GetWeapon() != null){
                currentAmmo = weapons[currentWeaponIndex].currentAmmo;
            }
        }catch(IndexOutOfRangeException){}catch(ArgumentOutOfRangeException){}
        */
    }

    public void LoadWeapons(){
        weapons.Clear();

        for(int i = 0; i < maxInventorySlots; i++){
            weapons.Add(null);
        }

        string weaponDataString = GetComponent<PlayerData>().LoadWeaponData();

        if(weaponDataString != null){
            CurrentWeaponData currentWeaponData = JsonUtility.FromJson<CurrentWeaponData>(weaponDataString);   
            
            if(currentWeaponData.hasDiedLastRound){
                //Debug.Log("Died Last Round");
            }else{
                GetComponent<Inventory>().weapons[0] = currentWeaponData.primaryWeapon;
                GetComponent<Inventory>().weapons[1] = currentWeaponData.secondaryWeapon;
                //Debug.Log("Didn't Die Last Round");
            }
        }

        EquipStartingWeapon();
        weapons[2] = melee;
        weapons[3] = null;
        GetComponent<PlayerStats>().isDead = false;
    }

    private void EquipStartingWeapon(){
        if(weapons[1] == null){
            weapons[1] = startingWeapon;
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

    public void AddPrimaryWeapon(Weapon newWeapon){
        if(newWeapon.weaponClass == WeaponClass.Primary){
            weapons[0] = newWeapon;
        }
    }

    public void AddSecondaryWeapon(Weapon newWeapon){
        if(newWeapon.weaponClass == WeaponClass.Secondary){
            weapons[1] = newWeapon;
        }
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
        if(currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count){
            return weapons[currentWeaponIndex];
        }
        return null;
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

    public void InitializeWeapon(Weapon weapon){
        if(weapon.weaponClass == WeaponClass.Primary){
            primaryMagAmmo = weapons[0].magazineSize;
            primaryCurrentAmmo = primaryMagAmmo;
            primaryMagTotalSize = primaryMagAmmo * 3;
        }
        
        if(weapon.weaponClass == WeaponClass.Secondary){
            secondaryMagAmmo = weapons[1].magazineSize;
            secondaryCurrentAmmo = weapons[1].magazineSize;
            secondaryMagTotalSize = secondaryMagAmmo * 3;
        }
    }

    public bool IsGun(){
        try{
            if(GetWeapon().weaponClass == WeaponClass.Primary && GetWeapon() != null){
                return true;
            }else if(GetWeapon().weaponClass == WeaponClass.Secondary && GetWeapon() != null){
                return true;
            }else{
                return false;
            }
        }catch(ArgumentOutOfRangeException){return false;}catch(NullReferenceException){return false;}
    }

    public void TakeBomb(){
        if(gameObject.CompareTag("TerroDirt")){
            weapons[3] = dirtBomb;
        }
    }
}
