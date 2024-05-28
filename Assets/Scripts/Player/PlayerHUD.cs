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
    public TextMeshProUGUI weapon;
    public TextMeshProUGUI currentAmmo;
    public Image ammoIcon;
    public Slider reloadCircle;
    public Slider plantCircle;
    public Slider defusingCirle;

    public TextMeshProUGUI purifierScore;
    public TextMeshProUGUI terroDirtScore;
    int p_score;
    int t_score;

    //Temp
    public TextMeshProUGUI kills;
    
    void Start(){
        p_score = FindObjectOfType<GameLoop>().GetPurifierScore();
        t_score = FindObjectOfType<GameLoop>().GetTerroDirtScore();
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
