using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public new string name;
    public float damage;
    public float fireRate;
    public float bulletSpread;
    public float bulletSpreadMovingModifier;
    public int currentAmmo;
    public float reloadTime;
    public int magazineSize;
    public int magazineTotalSize;
    public float scopeRange;
    public float scopeFov;
    public float cameraScoped;
    public float cameraUnscoped;
    public float moveSpeed;
    public int cost;
    public WeaponType weaponType;
    public WeaponClass weaponClass;
}

public enum WeaponType{Melee, Pistol, Rifle, SMG, Shotgun, Sniper, Bomb}
public enum WeaponClass{Primary, Secondary, Melee, Bomb}
