using UnityEngine;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI weaponNameText;
    [SerializeField] private TextMeshProUGUI ammoText;

    private void Update()
    {
        if (playerInventory.currentWeapon != null && playerInventory.currentWeapon.weaponType == "Melee")
        {
            weaponNameText.text = playerInventory.equippedWeaponKey;

            ammoText.text = "\u221E/\u221E";
        }
        else if (playerInventory.currentWeapon != null)
        {
            weaponNameText.text = playerInventory.equippedWeaponKey;

            int currentMag = playerInventory.currentWeapon.currentMag;
            int totalAmmo = playerInventory.currentWeapon.currentReserve;

            ammoText.text = $"{currentMag} / {totalAmmo}";
        }
        else
        {
            weaponNameText.text = "";
            ammoText.text = "";
        }
    }
}
