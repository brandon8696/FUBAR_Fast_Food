using System.Diagnostics;
using UnityEngine;

public class WeaponInstance
{
    public WeaponData WeaponData { get; private set; }

    public Animator weaponAnimator;
    public int currentMag;
    public int currentReserve;
    public float cooldownTimer;
    public GameObject weaponPrefab;
    public GameObject casingPrefab;
    public string weaponName;
    public float fireRate;

    public WeaponInstance(WeaponData data)
    {
        WeaponData = data;
        currentMag = data.magSize;
        currentReserve = data.extraAmmo;
        //cooldownTimer = 0.01f;
        weaponPrefab = data.prefab;
        weaponName = data.weaponName;
        fireRate = data.fireRate;
        casingPrefab = data.casing;
    }

    public bool Shoot()
    {
        UnityEngine.Debug.Log("Shoot() Called");
        if (currentMag > 0 && cooldownTimer <= 0f)
        {
            UnityEngine.Debug.Log("Adjusting mag size and respecting fire rate");
            currentMag--;
            //cooldownTimer = 1f / WeaponData.fireRate; // assume fireRate is shots/sec
            return true; // Shot fired successfully
        }
        else
        {
            // NO AMMO
            //Trigger voice response, trigger click response, and ammo UI flash
        }
            return false; 
    }

    public bool Reload()
    {
        //reload based on if you have ammo reserve and you shot anything out of the magazine
        if (currentMag < WeaponData.magSize && currentReserve > 0)
        { 
            int ammoNeeded = WeaponData.magSize - currentMag;
            int ammoToLoad = Mathf.Min(ammoNeeded, currentReserve);
            currentReserve -= ammoToLoad;
            currentMag += ammoToLoad;
            UnityEngine.Debug.Log("Reloaded.");
            return true; // reload happened
            //Update ammoReserve, but have to also account for ammo still in clip
           
        }
        else
        {
            //No Ammo 
            //Trigger voice response, trigger click response, and ammo UI flash
        }
            return false;
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= deltaTime;
        }
    }
}
