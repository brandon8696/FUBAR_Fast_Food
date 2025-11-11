using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Image crosshairImage;  
    [SerializeField] private CanvasGroup crosshairCanvas; // optional for fade control       // Your main crosshair sprite
    [SerializeField] public Camera mainCam;               // Camera reference
    [SerializeField] public WeaponHandler weaponHandler;  // Optional: to pull crosshair sprites per weapon

    [Header("Crosshair Settings")]
    [SerializeField] public float baseSize = 50f;         // Default resting size
    [SerializeField] public float pulseAmount = 20f;      // Size increase when shooting
    [SerializeField] public float pulseSpeed = 8f;        // How fast it returns to normal
    [SerializeField] public float distanceScale = 0.05f;  // Crosshair grows when close to surfaces
    [SerializeField] public float maxScale = 1.5f;        // Clamp for scaling
    [SerializeField] public Color normalColor = Color.white;
    [SerializeField] public Color targetColor = Color.red;

    public Color shootColor = Color.orange;

    public float currentSize;
    public float targetSize;
    public bool onPulse;
    public bool onTarget;
    public float currentDistance = 100f; // updated per frame from raycast

    
    void Awake()
    {
        if (!crosshairImage)
            crosshairImage = GetComponent<Image>();

        if (!crosshairCanvas)
            crosshairCanvas = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        currentSize = baseSize;
        targetSize = baseSize;
        crosshairImage.rectTransform.sizeDelta = new Vector2(baseSize, baseSize);
        crosshairImage.color = normalColor;
    }

    private void Update()
    {
        // Smoothly lerp back to base size
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * pulseSpeed);

        // Apply distance scaling (crosshair grows when close to walls)
        float distanceFactor = 1f + Mathf.Clamp01((1f / Mathf.Max(currentDistance, 0.01f))) * distanceScale;
        float finalSize = Mathf.Clamp(currentSize * distanceFactor, baseSize, baseSize * maxScale);

        // Update crosshair size
        crosshairImage.rectTransform.sizeDelta = new Vector2(finalSize, finalSize);

        // Update color based on target state
        crosshairImage.color = Color.Lerp(crosshairImage.color, onTarget ? targetColor : normalColor, Time.deltaTime * 10f);
        crosshairImage.color = Color.Lerp(crosshairImage.color, onPulse ? shootColor : normalColor, Time.deltaTime * 10f);
    }

    // Called by WeaponHandler on shoot
    public void Pulse()
    {
        onPulse = true;
        //crosshairImage.color = Color.Lerp(crosshairImage.color, shootColor, Time.deltaTime * 10f);
        targetSize = baseSize + pulseAmount;
        StartCoroutine(resetPulse(0.20f));
    }

    IEnumerator resetPulse(float delayTime)
    {

        // Pause execution for the specified delayTime
        yield return new WaitForSeconds(delayTime);
        onPulse = false;
        targetSize = baseSize;
    }

    // Called by WeaponHandler based on raycast hit tag
    public void SetTargetState(bool isOnTarget)
    {
        onTarget = isOnTarget;
    }

    // Called by ThirdPersonController or WeaponHandler to update aim distance
    public void UpdateAimData(RaycastHit hit, bool isAiming)
    {
        if (!isAiming)
        {
            crosshairCanvas.alpha = 0f;
            currentDistance = 100f;
            return;
        }

        // This is where we grab distance directly from RaycastHit
        currentDistance = hit.distance;
        crosshairCanvas.alpha = 0.70f;
    }

    // Optional: switch crosshair sprite per weapon
    public void SetCrosshairSprite(Sprite newSprite)
    {
        if (crosshairImage != null && newSprite != null)
            crosshairImage.sprite = newSprite;
    }
}
