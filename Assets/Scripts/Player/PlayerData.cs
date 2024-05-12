using System.Collections;
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

    public PlayerData(Vector2 position, float rotation){
        this.position = position;
        this.rotation = rotation;
    }

    void Start(){
        Init();
    }

    public static void Init(){
        if(!Directory.Exists(Player_Data_Folder)){
            Directory.CreateDirectory(Player_Data_Folder);
        }
    }

    void SendMovementUpdate(){
        PlayerData data = new PlayerData(rb.position, rb.rotation);
    }

    public void SaveWeaponData(){
        if(GetComponent<PlayerStats>().isDead){
            Weapon primaryWeapon = null;
            Weapon secondaryWeapon = null;
            bool hasDiedLastRound = true;

            CurrentWeaponData currentWeaponData = new CurrentWeaponData{
                primaryWeapon = primaryWeapon,
                secondaryWeapon = secondaryWeapon,
                hasDiedLastRound =hasDiedLastRound
            };
            string json = JsonUtility.ToJson(currentWeaponData);
            File.WriteAllText(Player_Data_Folder + "/CurrentWeaponData.txt", json);
            GetComponent<Inventory>().LoadWeapons();
        }else{
            Weapon primaryWeapon = null;
            Weapon secondaryWeapon = null;
            bool hasDiedLastRound = false;

            if(GetComponent<Inventory>().weapons[0] != null){
                primaryWeapon = GetComponent<Inventory>().weapons[0];
            }
            if(GetComponent<Inventory>().weapons[1] != null){
                secondaryWeapon = GetComponent<Inventory>().weapons[1];
            }

            CurrentWeaponData currentWeaponData = new CurrentWeaponData{
                primaryWeapon = primaryWeapon,
                secondaryWeapon = secondaryWeapon,
                hasDiedLastRound = hasDiedLastRound
            };
            string json = JsonUtility.ToJson(currentWeaponData);
            File.WriteAllText(Player_Data_Folder + "CurrentWeaponData.txt", json);
        }
    }

    public static string LoadWeaponData(){
        if(File.Exists(Player_Data_Folder + "CurrentWeaponData.txt")){
            string weaponDataString = File.ReadAllText(Player_Data_Folder + "CurrentWeaponData.txt");
            return weaponDataString;
        }else{
            return null;
        }
    }

}
public class CurrentWeaponData{
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public bool hasDiedLastRound;
}
