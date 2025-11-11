using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    /*This class controls the players stuff, including weapon instances and items instances...
    * This includes switching weapons, picking up weapons, picking up items, and using items
    * 
    */
    // PlayerInventory → WeaponHandler → WeaponInstance → animations & effects
    //Dictionaries can hold instances of objects
    //When I add a new weapon, I am making a new class instance, like creating a web dev page or class. 
    //These instances are taken from the scriptable object. 
    private Dictionary<string, WeaponInstance> weaponInventory = new();
    private List<string> weaponKeys = new List<string>();
    private int currentWeaponIndex = 0;
    //private Dictionary<string, ItemInstance> itemsInventory = new();

    public string equippedWeaponKey;
    public WeaponInstance currentWeapon;
   // public WeaponData autoMagWeapon; // Assign in Inspector for testing only
    //public WeaponData m2CarbineWeapon; for testing only
    public Transform handTransform; // Assign in Inspector — the player's hand bone
    public GameObject currentWeaponModel; // Tracks the model in the hand
    public Vector3 localPositionOffset;
    public Vector3 localRotationOffset;
    public Vector3 localScaleOffset;
    //public StarterAssetsInputs _input;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_input = GetComponent<StarterAssetsInputs>();
        StartCoroutine(DelayedAction()); // Start the coroutine for debugging
        //AddWeapon(m2CarbineWeapon); testing only
        //AddWeapon(autoMagWeapon); testing only
    }

    // Update is called once per frame
    void Update()
    {
             // (Optional) Adjust offsets if weapon doesn't align right
             //currentWeaponModel.transform.localPosition = localPositionOffset;
             //currentWeaponModel.transform.localEulerAngles = localRotationOffset;
    }

    public void EquipWeapon(string weaponName)
    {
        if (weaponInventory.ContainsKey(weaponName))
        {
          equippedWeaponKey = weaponName;
          currentWeapon = weaponInventory[weaponName]; // grab the instance you made earlier;
           UnityEngine.Debug.Log("Equipped Weapon is: " + weaponName);
        }
        else
        {
            return;
        }

        // Destroy old weapon model if it exists
        if (currentWeaponModel != null)
        {
            Destroy(currentWeaponModel);
        }

        // Instantiate the new model in the player's hand
        if (currentWeapon.weaponPrefab != null)
        {
            currentWeaponModel = Instantiate(currentWeapon.weaponPrefab, handTransform);

            // Reset local position/rotation so it fits the hand
            currentWeaponModel.transform.SetParent(handTransform);
            //currentWeaponModel.transform.localPosition = Vector3.zero;
            //currentWeaponModel.transform.localRotation = Quaternion.identity;

            // (Optional) Adjust offsets if weapon doesn't align right
            if(currentWeapon.weaponName == "AutoMag")
            {
                localPositionOffset = new Vector3(0.0668f, 0.0403f, -0.0729f);
                localRotationOffset = new Vector3(107.03f, -107.08f, -30.73f);
                //localRotationOffset = new Vector3(105.3f, -85.6f, -5.56f);
                localScaleOffset = new Vector3(107.0f, 107.0f, 107.0f);
            }
            if (currentWeapon.weaponName == "M2 Carbine")
            {
                localPositionOffset = new Vector3(0.251f, 0.142f, -0.043f);
                localRotationOffset = new Vector3(22.539f, 14.425f, 210.893f);
                localScaleOffset = new Vector3(0.1f, 0.1f, 0.1f);
            }
            if(currentWeapon.weaponName == "Model 1897")
            {
                localPositionOffset = new Vector3(0.144f,0.084f,-0.030f);
                localRotationOffset = new Vector3(24.065f, 17.878f, 208.310f);
                localScaleOffset = new Vector3(0.796f, 0.796f, 0.796f);
            }
            if (currentWeapon.weaponName == "Machete")
            {
                localPositionOffset = new Vector3(0.0459f, 0.0862f, -0.0445f);
                localRotationOffset = new Vector3(107.03f, -107.06f, -30.72f);
                localScaleOffset = new Vector3(0.15f, 8.75f, 8.75f);
            }
             currentWeaponModel.transform.localPosition = localPositionOffset;
             currentWeaponModel.transform.localEulerAngles = localRotationOffset;
             currentWeaponModel.transform.localScale = localScaleOffset;
        }
    }

    public void AddWeapon(WeaponData weaponData,  bool autoEquip = true)
    {
        if (!weaponInventory.ContainsKey(weaponData.weaponName))
        {
            WeaponInstance newInstance = new WeaponInstance(weaponData);
            weaponInventory.Add(weaponData.weaponName, newInstance);
            weaponKeys.Add(weaponData.weaponName);
            UnityEngine.Debug.Log("Added weapon: " + weaponData.weaponName);
        }
        else
        {
            weaponInventory.TryGetValue(weaponData.weaponName, out WeaponInstance existingWeapon);
            existingWeapon.currentReserve = existingWeapon.currentReserve + weaponData.extraAmmo;
            //No need to auto equip and annoy player, just return!
            return;
        }

        if (autoEquip)
        {
            EquipWeapon(weaponData.weaponName);
        }
    }

    public void NextWeapon()
    {
        UnityEngine.Debug.Log("Next Weapon!");
        if (weaponKeys.Count == 0) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % weaponKeys.Count;
        EquipWeapon(weaponKeys[currentWeaponIndex]);
        return;
    }

    public void PreviousWeapon()
    {
        UnityEngine.Debug.Log("Previous Weapon!");
        if (weaponKeys.Count == 0) return;

        currentWeaponIndex = (currentWeaponIndex - 1 + weaponKeys.Count) % weaponKeys.Count;
        EquipWeapon(weaponKeys[currentWeaponIndex]);
        return;
    }

    public void AddItem()
    {

    }

    public void AddHealth(int Addhealth)
    {

    }
    
    public void AddAmmo(int Addammo)
    {
        
    }

    // This is the coroutine that handles the delay
    IEnumerator DelayedAction()
    {
            // Wait for 1 second
            yield return new WaitForSeconds(1f); 

            // Code to execute after the 1-second delay
             UnityEngine.Debug.Log("1 second has passed!");
            // You can put any other code here, e.g., enabling a UI element, playing a sound, etc.
     }
}
