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
    public WeaponData autoMagWeapon; // Assign in Inspector for testing only
    public WeaponData m2CarbineWeapon;
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
        AddWeapon(m2CarbineWeapon);
        AddWeapon(autoMagWeapon);
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
                localPositionOffset = new Vector3(0.249f, 0.003f, 0.017f);
                localRotationOffset = new Vector3(8.973f, -13.525f, 179.375f);
                localScaleOffset = new Vector3(0.1f, 0.1f, 0.1f);
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
