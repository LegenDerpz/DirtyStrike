using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Weapon> weapons;
    public int maxInventorySlots = 3;
    public WeaponShop shop;
    public int currentWeaponIndex = 0;
    int currentAmmo;

    private void Start(){
        weapons =  new List<Weapon>();
        LoadWeapons();
        weapons[0] = shop.rifle;
        weapons[2] = shop.knife;

        int i = 0;
        foreach(Weapon weapon in weapons){
            if(weapon != null && (weapon.weaponType != WeaponType.Melee || weapon.weaponType != WeaponType.Bomb)){
                currentAmmo = weapons[i].magazineSize;
                weapons[i].currentAmmo = currentAmmo;
                weapons[i].magazineTotalSize = weapons[i].magazineSize * 3;
                i++;
            }
        }
    }

    private void Update(){
        try{
            currentAmmo = weapons[currentWeaponIndex].currentAmmo;
        }catch(IndexOutOfRangeException){}catch(ArgumentOutOfRangeException){}
    }

    private void LoadWeapons(){
        weapons.Clear();
        for(int i = 0; i < maxInventorySlots; i++){
            weapons.Add(null);
        }
        EquipStartingWeapon();
    }

    private void EquipStartingWeapon(){
        weapons[1] = shop.classicPistol;
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
            if(weapons[currentWeaponIndex].weaponClass == WeaponClass.Primary){
                return true;
            }else if(weapons[currentWeaponIndex].weaponClass == WeaponClass.Secondary){
                return true;
            }else{
                return false;
            }
        }catch(ArgumentOutOfRangeException){return false;}
    }
}
