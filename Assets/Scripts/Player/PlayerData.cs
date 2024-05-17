using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D.Animation;

public class PlayerData : MonoBehaviour
{
    public static readonly string Player_Data_Folder = Application.dataPath + "/PlayerData/";
    public Inventory inv;

    public Vector2 position;
    public Rigidbody2D rb;

    public float rotation;
    public Weapon currentWeapon;

    public GameObject head, rightArm, rightHand, leftArm, leftHand;
    SpriteResolver sr_head, sr_rightArm, sr_rightHand, sr_leftArm, sr_leftHand;

    private int layer = 10;

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

    public static void Init()
    {
        if (!Directory.Exists(Player_Data_Folder))
        {
            Directory.CreateDirectory(Player_Data_Folder);
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
    public int GetPlayerAmmo()
    {
        return inv.currentAmmo;
    }
    public List<Weapon> GetPlayerInventory()
    {
        return inv.weapons;
    }
    //

    //Player Stats
    public float GetPlayerHealth()
    {
        return GetComponent<PlayerStats>().health;
    }
    public void SetPlayerHealth(float health)
    {
        GetComponent<PlayerStats>().health = health;
    }
    public bool GetPlayerDeathState()
    {
        return GetComponent<PlayerStats>().isDead;
    }
    public void SetPlayerDeathState(bool deathState)
    {
        GetComponent<PlayerStats>().isDead = deathState;
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
            File.WriteAllText(Player_Data_Folder + "/CurrentWeaponData.txt", json);
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
            File.WriteAllText(Player_Data_Folder + "CurrentWeaponData.txt", json);
        }
    }

    public static string LoadWeaponData()
    {
        if (File.Exists(Player_Data_Folder + "CurrentWeaponData.txt"))
        {
            string weaponDataString = File.ReadAllText(Player_Data_Folder + "CurrentWeaponData.txt");
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
