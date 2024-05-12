using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    public Credits credits;
    public List<Weapon> primaryShop;
    public List<Weapon> secondaryShop;
    public Weapon classicPistol;
    public Weapon rifle;
    public Weapon knife;

    void Start(){
        primaryShop = new List<Weapon>
        {
            rifle
        };

        secondaryShop = new List<Weapon>
        {
            classicPistol
        };
    }

    public void buyWeaponPrimary(){
        
    }
    public void buyRifle(){
        GetComponent<Inventory>().weapons[0] = rifle;
    }
}
