using UnityEngine;

public class TorsoAimIK : MonoBehaviour
{
    [Header("References")]
    public Animator animator;        // Player animator
    public Transform spineBone;      // The spine or chest bone you want to rotate

    [Header("Settings")]
    public float rotationSpeed = 10f; // How fast the spine rotates toward target
    public float maxPitchUp = 45f;    // Limit upward rotation
    public float maxPitchDown = -30f; // Limit downward rotation

    private Transform cameraTransform;

    [Header("Input Reference")]
    public StarterAssets.StarterAssetsInputs playerInput; // reference to your input script

    void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    void Awake()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!animator || !spineBone) return;

        // Only update torso when aiming
        if (playerInput != null && playerInput.aim > 0f)
        {
            // Calculate direction from spine to camera forward
            Vector3 aimDirection = cameraTransform.forward;

            // Keep only horizontal and vertical separately
            Vector3 spinePosition = spineBone.position;
            Vector3 lookDirection = (spinePosition + aimDirection) - spinePosition;

            // Convert to local rotation
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            // Clamp pitch
            Vector3 euler = targetRotation.eulerAngles;
            euler.x = ClampAngle(euler.x, maxPitchDown, maxPitchUp);
            euler.z = 0; // Keep roll zero
            targetRotation = Quaternion.Euler(euler);

            // Smoothly rotate spine
            spineBone.rotation = Quaternion.Slerp(spineBone.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        angle = angle > 180 ? angle - 360 : angle;
        return Mathf.Clamp(angle, min, max);
    }
}