using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    //CHANGE THIS TO GENERIC STUFF
    public Sprite icon;
    public GameObject prefab;
    public GameObject casing;
    public string weaponName; 
    public string weaponType; //Slot the weapon occupies
    public string ammoPool; //What it shares ammo with (In this case the M2 Carbine)
    public int baseDamage; //Not Including Multipliers
    public float fireRate; //Rounds per minute
    public int magSize; //Doesn't count +1 in barrel, will keep track in player inventory
    public int extraAmmo; //total ammo you have on reserve
}
