using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static readonly string Player_Data_Folder = Application.dataPath + "/PlayerData/";
    public Inventory inv;

    public Vector2 position;
    public Rigidbody2D rb;

    public float rotation;
    public Weapon currentWeapon;

    void Start()
    {
        Init();
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
