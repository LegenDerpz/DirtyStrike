using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Inventory inv;
    public Combat combat;
    public TextMeshProUGUI hpValue;
    public Slider hpSlider;
    TextMeshProUGUI weapon;
    TextMeshProUGUI currentAmmo;
    Image ammoIcon;
    Slider reloadCircle;
    Slider plantCircle;
    Slider defusingCirle;

    TextMeshProUGUI purifierScore;
    TextMeshProUGUI terroDirtScore;


    int p_score;
    int t_score;

    //Temp
    TextMeshProUGUI kills;
    
    void Start(){
        purifierScore = GameObject.Find("Canvas/Purifier Score").GetComponent<TextMeshProUGUI>();
        terroDirtScore = GameObject.Find("Canvas/TerroDirt Score").GetComponent<TextMeshProUGUI>();
        p_score = FindObjectOfType<GameLoop>().GetPurifierScore();
        t_score = FindObjectOfType<GameLoop>().GetTerroDirtScore();

        kills = GameObject.Find("Canvas/Kills").GetComponent<TextMeshProUGUI>();
        weapon = GameObject.Find("Canvas/Weapon").GetComponent<TextMeshProUGUI>();
        currentAmmo = GameObject.Find("Canvas/Ammo").GetComponent<TextMeshProUGUI>();
        ammoIcon = GameObject.Find("Canvas/Ammo/AmmoIcon").GetComponent<Image>();
        reloadCircle = GameObject.Find("Canvas/Reloading").GetComponent<Slider>();
        plantCircle = GameObject.Find("Canvas/PlantCircle").GetComponent<Slider>();
        defusingCirle = GameObject.Find("Canvas/DefuseCircle").GetComponent<Slider>();
    }

    void Update(){
        try{
            if(inv.currentWeaponIndex > 1 && !inv.IsGun()){
                currentAmmo.text = "";
                weapon.text = inv.GetWeapon(inv.currentWeaponIndex).name.ToString();
                ammoIcon.enabled = false;
            }else{
                weapon.text = inv.GetWeapon().name.ToString();
                if(inv.GetWeapon().weaponClass == WeaponClass.Primary){
                    currentAmmo.text = inv.primaryCurrentAmmo.ToString() + "/" + inv.primaryMagTotalSize.ToString();
                }else if(inv.GetWeapon().weaponClass == WeaponClass.Secondary){
                    currentAmmo.text = inv.secondaryCurrentAmmo.ToString() + "/" + inv.secondaryMagTotalSize.ToString();
                }
                ammoIcon.enabled = true;
            }

            if(combat.isReloading && inv.IsGun()){
                ammoIcon.enabled = false;
                reloadCircle.gameObject.SetActive(true);
                reloadCircle.value = combat.reloadProgress;
            }
            if(!combat.isReloading){
                if(inv.IsGun()){
                    ammoIcon.enabled = true;
                }
                reloadCircle.gameObject.SetActive(false);
            }
        }catch(NullReferenceException){}

        purifierScore.text = p_score.ToString();
        terroDirtScore.text = t_score.ToString();
        kills.text = GetComponent<PlayerData>().GetPlayerKills().ToString();

        hpValue.text = GetComponent<PlayerData>().GetCurrentHealth().ToString();
    }

    void FixedUpdate(){
        try{
            //TerroDirt Planting
            if(GetComponent<TerroDirt>().isPlanting){
                plantCircle.gameObject.SetActive(true);
                plantCircle.value = GetComponent<TerroDirt>().plantProgress;
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

    public void SetMaxHealth(float health){
        hpSlider.maxValue = health;
        hpSlider.value = health;
    }

    public void SetHealth(float health){
        hpSlider.value = health;
    }
}
