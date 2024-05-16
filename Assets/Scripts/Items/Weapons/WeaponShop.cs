using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponShop : MonoBehaviour
{
    public Credits credits;
    public List<Weapon> primaryShop;
    public List<Weapon> secondaryShop;
    public Weapon classicPistol;
    public Weapon rifle;
    public Weapon knife;

    bool shopOpened = false;

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

    void Update(){
        if(Input.GetKeyDown(KeyCode.B) && !shopOpened && !FindObjectOfType<RoundTimer>().buyPhaseEnded){
            SceneManager.LoadSceneAsync("WeaponShop", LoadSceneMode.Additive);
            shopOpened = true;
        }else if(Input.GetKeyDown(KeyCode.B) && shopOpened){
            SceneManager.UnloadSceneAsync("WeaponShop");
            shopOpened = false;
        }
    }

    public void buyWeaponPrimary(){
        
    }
    public void buyRifle(){
        gameObject.GetComponent<Inventory>().weapons[0] = rifle;
    }
}
