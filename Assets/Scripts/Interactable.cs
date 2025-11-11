using UnityEngine;

public enum InteractType
{
    Ammo,
    Health,
    StoryItem,
    Weapon,
    Door,
    Generic
}

public class Interactable : MonoBehaviour
{
    public InteractType type;

    [Header("Generic Data")]
    public string interactName;

    [Header("Ammo / Health")]
    public int amount;

    [Header("Story Item")]
    //public ItemData itemData; // Optional — can be null

    [Header("Weapon Pickup")]
    public WeaponData weaponData; // Optional — can be null

    [Header("Audio Settings")]
    public AudioClip interactSound;  // Sound to play when picked up
    [Range(0f, 1f)] public float soundVolume = 1f;

    // Called when this item is picked up (destroy or disable)
    public virtual void OnPickup()
    {
        if(type == InteractType.Weapon || type == InteractType.Ammo)
        {
            AudioSource.PlayClipAtPoint(interactSound, transform.position, soundVolume);
        }
        Destroy(gameObject);
    }

    // Called for generic interactable behavior (like switches, triggers)
    public virtual void Activate()
    {
        Debug.Log($"{name} activated!");
    }

    // Optional door handling
    public virtual void TriggerDoor()
    {
        Debug.Log($"{name} door triggered!");
    }
}
