using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerData : MonoBehaviour
{
    public static readonly string Player_Data_Folder = Application.dataPath + "/PlayerData/";
    public string username = "username";
    string purifierTag = "Purifier";
    string terrodirtTag = "TerroDirt";
    string customTag;
    public Inventory inv;

    public Vector2 position;
    public Rigidbody2D rb;


    //Sprite Setup
    public GameObject head, rightArm, rightHand, leftArm, leftHand;
    SpriteResolver sr_head, sr_rightArm, sr_rightHand, sr_leftArm, sr_leftHand;
    private int layer = 10; //10 is the Purifier Layer

    //====Stats====
    //kills;
    //deaths;
    
    //====To Consider====
    //assists
    //damageDealt
    //damageTaken
    //defuses
    //plants


    void Start()
    {
        Init();

        sr_head = head.GetComponent<SpriteResolver>();
        sr_rightArm = rightArm.GetComponent<SpriteResolver>();
        sr_rightHand = rightHand.GetComponent<SpriteResolver>();
        sr_leftArm = leftArm.GetComponent<SpriteResolver>();
        sr_leftHand = leftHand.GetComponent<SpriteResolver>();

        if(CompareTag("Purifier")){
            sr_head.SetCategoryAndLabel("Head", "Purifier Head");
            sr_rightArm.SetCategoryAndLabel("R_Arm", "Purifier Right Arm");
            sr_rightHand.SetCategoryAndLabel("R_Hand", "Purifier Right Hand");
            sr_leftArm.SetCategoryAndLabel("L_Arm", "Purifier Left Arm");
            sr_leftHand.SetCategoryAndLabel("L_Hand", "Purifier Left Hand");
            GetComponent<Purifier>().enabled = true;
            GetComponent<Combat>().layerMask |= 2 << layer; //Can Only Damage TerroDirt Players
            gameObject.layer = 10;
        }else if(CompareTag("TerroDirt")){
            sr_head.SetCategoryAndLabel("Head", "TerroDirt Head");
            sr_rightArm.SetCategoryAndLabel("R_Arm", "TerroDirt Right Arm");
            sr_rightHand.SetCategoryAndLabel("R_Hand", "TerroDirt Right Hand");
            sr_leftArm.SetCategoryAndLabel("L_Arm", "TerroDirt Left Arm");
            sr_leftHand.SetCategoryAndLabel("L_Hand", "TerroDirt Left Hand");
            GetComponent<TerroDirt>().enabled = true;
            GetComponent<Combat>().layerMask |= 1 << layer; //Can Only Damage Purifier Players
            gameObject.layer = 11;
        }
    }

    public void Init()
    {
        if (!Directory.Exists(Player_Data_Folder))
        {
            Directory.CreateDirectory(Player_Data_Folder);
        }
        foreach (PlayerData player in FindObjectsOfType<PlayerData>())
        {
            if (!Directory.Exists(Player_Data_Folder + "/" + player.username))
            {
                Directory.CreateDirectory(Player_Data_Folder + "/" + player.username);
            }
        }
    }

    //Transform getters/setters
    public Vector2 GetPlayerPosition()
    {
        return rb.position;
    }
    public Quaternion GetPlayerRotation()
    {
        return transform.rotation;
    }
    public void SetPlayerTransform(Vector2 position, Quaternion rotation)
    {
        rb.position = position;
        transform.rotation = rotation;
    }
    //

    //Weapon getters/setters
    public Weapon GetPlayerWeapon()
    {
        return inv.GetWeapon();
    }
    public Sprite GetPlayerWeaponSprite()
    {
        return inv.GetWeapon().sprite;
    }
    //Set Current Weapon by Current Index
    public void SetPlayerWeapon(Weapon weapon)
    {
        inv.weapons[inv.currentWeaponIndex] = weapon;
    }
    //Set Weapon by Selected Index
    public void SetPlayerWeapon(int index, Weapon weapon)
    {
        inv.weapons[index] = weapon;
    }
    public int GetPrimaryAmmo(){
        return inv.primaryCurrentAmmo;
    }
    public int GetPrimaryMagAmmo(){
        return inv.primaryMagAmmo;
    }
    public int GetPrimaryMagTotalAmmo(){
        return inv.primaryMagTotalSize;
    }
    public int GetSecondaryAmmo(){
        return inv.secondaryCurrentAmmo;
    }
    public int GetSecondaryMagAmmo(){
        return inv.secondaryMagAmmo;
    }
    public int GetSecondaryMagTotalAmmo(){
        return inv.secondaryMagTotalSize;
    }
    public List<Weapon> GetPlayerInventory()
    {
        return inv.weapons;
    }
    //

    //Player Stats
    public string GetUsername(){
        return username;
    }
    public string GetPuriferTag(){
        return purifierTag;
    }
    public string GetTerrodirtTag(){
        return terrodirtTag;
    }
    public void SetCustomTag(string tagName){
        customTag = GetUsername() + "_" + tagName;
    }
    public float GetPlayerHealth()
    {
        return GetComponent<PlayerStats>().health;
    }
    public void SetPlayerHealth(float health)
    {
        GetComponent<PlayerStats>().health = health;
    }
    public float GetCurrentHealth(){
        return GetComponent<PlayerStats>().GetCurrentHealth();
    }
    public bool GetPlayerDeathState()
    {
        return GetComponent<PlayerStats>().isDead;
    }
    public void SetPlayerDeathState(bool deathState)
    {
        GetComponent<PlayerStats>().isDead = deathState;
    }
    public void AddPlayerKills(int killAmount){
        PlayerPrefs.SetInt(username + "_" + "Kills", GetPlayerKills() + killAmount);
    }
    public int GetPlayerKills(){
        return PlayerPrefs.GetInt(username + "_" + "Kills");
    }
    //

    // Bullets
    public Transform GetAllBulletTransforms()
    {
        if (FindObjectsOfType<Bullet>() != null)
        {
            foreach (Bullet bullet in FindObjectsOfType<Bullet>())
            {
                return bullet.GetBulletTransform();
            }
        }
        return null;
    }

    //Saves player weapon data to use for next round
    public void SaveWeaponData()
    {
        if (GetComponent<PlayerStats>().isDead)
        {
            Weapon primaryWeapon = null;
            Weapon secondaryWeapon = null;
            bool hasDiedLastRound = true;

            CurrentWeaponData currentWeaponData = new CurrentWeaponData
            {
                primaryWeapon = primaryWeapon,
                secondaryWeapon = secondaryWeapon,
                hasDiedLastRound = hasDiedLastRound
            };
            string json = JsonUtility.ToJson(currentWeaponData);
            File.WriteAllText(Player_Data_Folder + "/" + username + "/CurrentWeaponData.txt", json);
            GetComponent<Inventory>().LoadWeapons();
        }
        else
        {
            Weapon primaryWeapon = null;
            Weapon secondaryWeapon = null;
            bool hasDiedLastRound = false;

            if (GetComponent<Inventory>().weapons[0] != null)
            {
                primaryWeapon = GetComponent<Inventory>().weapons[0];
            }
            if (GetComponent<Inventory>().weapons[1] != null)
            {
                secondaryWeapon = GetComponent<Inventory>().weapons[1];
            }

            CurrentWeaponData currentWeaponData = new CurrentWeaponData
            {
                primaryWeapon = primaryWeapon,
                secondaryWeapon = secondaryWeapon,
                hasDiedLastRound = hasDiedLastRound
            };
            string json = JsonUtility.ToJson(currentWeaponData);
            File.WriteAllText(Player_Data_Folder + "/" + username + "/CurrentWeaponData.txt", json);
        }
    }

    public string LoadWeaponData()
    {
        if (File.Exists(Player_Data_Folder + "/" + username + "/CurrentWeaponData.txt"))
        {
            string weaponDataString = File.ReadAllText(Player_Data_Folder + "/" + username + "/CurrentWeaponData.txt");
            return weaponDataString;
        }
        else
        {
            return null;
        }
    }

}
public class CurrentWeaponData
{
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public bool hasDiedLastRound;
}
