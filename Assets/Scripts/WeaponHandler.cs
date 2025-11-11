using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Diagnostics;
using StarterAssets;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

public class WeaponHandler : MonoBehaviour
{
    public PlayerInventory inventory;
    public CrosshairHandler crosshairHandler;
    public Animator animator;

    private Transform handTransform;

    private int _animFire;
    public Transform barrelEnd;  // Assign on instantiate
    public WeaponInstance currentInstance;
    public LineRenderer tracerPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject muzzlePointLight;
    //public AudioClip GunAudioClip;
    public AudioClip ReloadAudioClip;
    [Range(0, 1)] public float GunAudioVolume = 0.7f;
    [Header("Tracer Settings")]
    [SerializeField] private float tracerDuration = 0.5f;
    [SerializeField] private float range = 1000f;
    public float spreadAngle = 5.0f;
    public StarterAssets.StarterAssetsInputs _input;
    //public float triggerDown;
    public float coolDown;
    public float fireRate;
    public float nextTimeToFire;
    public GameObject bulletImpactPrefab;
    public float impactLifetime = 5f;
    public LayerMask shootMask;

    // Assign in Inspector
    [Header("Ejection Settings")]
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private Transform ejectionPort; // <-- New Transform: Where the shell spawns
    [SerializeField] private CinemachineImpulseSource impulseSource;
    private GameObject _mainCamera;
    public ThirdPersonController tpc;

    [Header("Casing Physics")]
    [SerializeField] private float ejectionUpwardForce = 0.5f;
    [SerializeField] private float ejectionSideForce = 0.5f;
    [SerializeField] private float ejectionTorque = 1.5f; // For the backwards spin
    [SerializeField] private float casingLifetime = 5f; // How long until cleanup

    private void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    void Start()
    {
        //_animFire = Animator.StringToHash("Fire");
        //_input = GetComponent<StarterAssetsInputs>();
        //_input = GetComponentInParent<StarterAssetsInputs>();
    }

    void Update()
    {
        WeaponInstance current = inventory.currentWeapon;
        if (current == null) return;

        //coolDown time
        CoolDownTimer();

        // If it changed, update references
        if (current != currentInstance)
        {
            currentInstance = current;
            casingPrefab = current.casingPrefab;
            muzzleFlashPrefab = current.muzzleFlash;
            handTransform = animator.GetBoneTransform(HumanBodyBones.RightHand);
            // Assuming weaponPrefab is instantiated already and parented under players hand
            GameObject weaponGO = inventory.currentWeaponModel; // <--- see note below
            barrelEnd = weaponGO.transform.Find("barrelEnd");
            ejectionPort = weaponGO.transform.Find("casingExit");
            fireRate = currentInstance.fireRate;
            nextTimeToFire = (1 / fireRate * 60);

            if (barrelEnd == null)
            {
                UnityEngine.Debug.Log("No BarrelEnd found on " + weaponGO.name);
            }
        }

        // Shooting
        if (_input.shoot > 0.01f && _input.aim > 0.01f && coolDown >= nextTimeToFire)
        {
            
            //UnityEngine.Debug.Log("_input shoot down");
            if (current.Shoot())
            {
                animator.SetBool("Fire", true);
                animator.SetBool("Reload", false);
                //UnityEngine.Debug.Log("current.shoot()returned true");
                //animator.SetBool(_animFire, true);
                if (currentInstance.weaponType == "Melee")
                {
                    MeleeHandler(current);
                }
                else
                {
                    ShootHandler(current);
                }

                // Muzzle flash, sound, etc.
                coolDown = 0;
            }
        }
        else if (_input.shoot > 0.01f && currentInstance.weaponType == "Melee"  && coolDown >= nextTimeToFire)
        {
            animator.SetBool("Fire", true);
            MeleeHandler(current);
            coolDown = 0;
        }
        else if (coolDown < nextTimeToFire)
        {
            //UnityEngine.Debug.Log("COOLING DOWN HOSS"); 
        }
        else
        {
            animator.SetBool("Fire", false);
        }

        // Reloading
        if (_input.reload)
        {
            if (current.Reload())
            {
                // Reload sound
                StartCoroutine(ResetReload());
                //animator.SetBool("Reload", false);
            }
            _input.reload = false;
        }

    }
    void MeleeHandler(WeaponInstance weapon)
    {
        AudioSource.PlayClipAtPoint(weapon.fireClip, handTransform.position, GunAudioVolume);
        StartCoroutine(ResetMelee());
        //animator.SetBool("Fire", false);
    }

    void ShootHandler(WeaponInstance weapon)
    {
        // direction from barrel to aim point
        Vector3 aimPoint = tpc.aimPoint;
        Vector3 dir = (aimPoint - barrelEnd.position).normalized;

        AudioSource.PlayClipAtPoint(weapon.fireClip, barrelEnd.position, GunAudioVolume);

        Vector3 recoilDir = _mainCamera.transform.forward + Vector3.up * 0.15f;
        impulseSource.GenerateImpulse(recoilDir.normalized);
        crosshairHandler.Pulse();
        // Raycast from barrelEnd
        //Ray ray = new Ray(barrelEnd.position);

        RaycastHit hit;

        StartCoroutine(FlashSequence());

        //Instantiate(testAss, barrelEnd.position, barrelEnd.rotation);
        if (weapon.weaponType == "Shotgun")
        {
            // Define the number of pellets (8 is a good choice)
            int pelletCount = 8;
            // Assume you have a float variable 'spreadAngle' for the maximum deviation in degrees

            for (int i = 0; i < pelletCount; i++)
            {
                // 1. Calculate a random direction for the pellet
                // Get a random point in a unit circle (disc)
                Vector3 randomCircle = UnityEngine.Random.insideUnitCircle;

                // Convert the 2D random point into a 3D direction vector
                // Use the weapon's forward direction as the center of the cone.
                // The spreadAngle determines the maximum deviation.
                Quaternion rot = Quaternion.LookRotation(dir);
                // Create a random offset rotation
                Quaternion spreadRotation = Quaternion.Euler(randomCircle.x * spreadAngle, randomCircle.y * spreadAngle, 0);
                // Combine the base direction rotation with the random spread rotation
                Vector3 pelletDir = (rot * spreadRotation) * Vector3.forward;


                // 2. Perform the Raycast for the individual pellet
                RaycastHit pelletHit;
                if (Physics.Raycast(barrelEnd.position, pelletDir, out pelletHit, range, shootMask))
                {
                    // --- Pellet Hit Logic ---
                    UnityEngine.Debug.Log($"Shotgun Pellet Hit: {pelletHit.collider.name}");
                    // Draw a line for visualization (can be removed later)
                    UnityEngine.Debug.DrawLine(barrelEnd.position, pelletHit.point, Color.yellow, 5f, false);

                    // Spawn bullet impact at the pellet's hit point
                    Quaternion impactRotation = Quaternion.LookRotation(pelletHit.normal);
                    GameObject impact = Instantiate(bulletImpactPrefab, pelletHit.point + pelletHit.normal * 0.001f, impactRotation);
                    impact.transform.SetParent(pelletHit.collider.transform);
                    Destroy(impact, impactLifetime);

                    // Spawn tracer (from barrel to hit point)
                    SpawnTracer(barrelEnd.position, pelletHit.point);

                    // Handle damage/crosshair logic (note: you may want to apply less damage per pellet)
                    crosshairHandler.SetTargetState(pelletHit.collider.CompareTag("Enemy"));
                }
                else
                {
                    // --- Pellet Miss Logic (Optional) ---
                    // Spawn tracer (from barrel to max range in the spread direction)
                    SpawnTracer(barrelEnd.position, barrelEnd.position + pelletDir * range);
                    // Draw a line for visualization (can be removed later)
                    UnityEngine.Debug.DrawLine(barrelEnd.position, barrelEnd.position + pelletDir * range, Color.blue, 5f, false);
                }
            }
        }
        else
        {

            if (Physics.Raycast(barrelEnd.position, dir, out hit, range, shootMask))
            {
                UnityEngine.Debug.Log("Hit " + hit.collider.name);
                UnityEngine.Debug.DrawLine(barrelEnd.position, hit.point, Color.red, 5f, false); // duration 10s
                                                                                                 // Spawn bullet impact
                Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                GameObject impact = Instantiate(bulletImpactPrefab, hit.point + hit.normal * 0.001f, impactRotation);

                // Optional: parent to surface so it moves with it (doors, enemies, etc.)
                impact.transform.SetParent(hit.collider.transform);

                // Destroy after a delay (keep scene clean)
                Destroy(impact, impactLifetime);
                SpawnTracer(barrelEnd.position, hit.point);
                crosshairHandler.SetTargetState(hit.collider.CompareTag("Enemy"));
            }
            else
            {
                //UnityEngine.Debug.Log("Nothing getting hit...");
                //UnityEngine.Debug.DrawRay(ray.origin, ray.direction * range, Color.blue, 1f); // extend to see it
                SpawnTracer(barrelEnd.position, barrelEnd.position + barrelEnd.forward * range);
                UnityEngine.Debug.DrawLine(barrelEnd.position, hit.point, Color.blue, 5f, false); // duration 10s
            }
        }

        StartCoroutine(EjectCasing());
    }

    void SpawnTracer(Vector3 start, Vector3 end)
    {
        UnityEngine.Debug.Log("Tracer function hit!");
        LineRenderer tracer = Instantiate(tracerPrefab, start, Quaternion.identity).GetComponent<LineRenderer>();
        tracer.positionCount = 2;
        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);
        Destroy(tracer.gameObject, 0.5f);
    }

    private IEnumerator FlashSequence()
    {
        // 1. Spawn: Instantiate the muzzle flash at the barrel tip
        GameObject flashInstance = Instantiate(
            muzzleFlashPrefab,
            barrelEnd.position,
            barrelEnd.rotation,
            barrelEnd // Parent it to the barrel to ensure it moves with the gun
        );
        // Reset local scale so it doesn�t inherit weird world scale
        //flashInstance.transform.localScale = Vector3.one;

        //flashInstance.transform.localScale = new Vector3(1,1,1);
        //flashInstance.transform.SetParent(barrelEnd, false); 

        // 1. Spawn: Instantiate the muzzle flash at the barrel tip
        GameObject lightInstance = Instantiate(
            muzzlePointLight,
            barrelEnd.position,
            barrelEnd.rotation,
            barrelEnd // Parent it to the barrel to ensure it moves with the gun
        );

        // 2. Manipulate/Run: Let the particle system run for its intended duration
        // For a short, snappy effect, 0.1s is usually plenty
        yield return new WaitForSeconds(0.1f);

        // 3. Destroy: Clean up the temporary object
        Destroy(flashInstance);
        Destroy(lightInstance);
        animator.SetBool("Fire", false);
    }

    public void CoolDownTimer()
    {
        //coolDown time
        coolDown += Time.deltaTime;
    }

    // Call this after a successful shot in HandleShoot()
    public IEnumerator EjectCasing()
    {

        animator.SetBool(_animFire, false);
        if (casingPrefab == null || ejectionPort == null)
        {
            UnityEngine.Debug.Log("Casing Prefab or Ejection Port not assigned in WeaponHandler.");
            yield return new WaitForSeconds(0f);
        }

        if (currentInstance.equipClip != null)
        {
            AudioSource.PlayClipAtPoint(currentInstance.equipClip, ejectionPort.position, GunAudioVolume);
            yield return new WaitForSeconds(0.50f);
        }
        // 1. Instantiate the casing
        GameObject casingInstance = Instantiate(
            casingPrefab,
            ejectionPort.position,
            ejectionPort.rotation
        );

        // 2. Get the Rigidbody (MUST be present on the casingPrefab)
        Rigidbody rb = casingInstance.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Calculate Force Vector
            // Right vector (ejection side) is usually the gun's local X-axis
            // Up vector is the gun's local Y-axis

            // Local Right and Up vectors relative to the ejectionPort
            Vector3 localRight = ejectionPort.right;
            Vector3 localUp = ejectionPort.up;

            float velocityMult = 1;
            // Combine forces: mostly Up, slightly to the Side (Right)
            if (currentInstance.weaponType == "Shotgun")
            {
                velocityMult = 0.50f;
            }
            Vector3 ejectionForce =
                (localRight * ejectionSideForce * velocityMult) +
                (localUp * ejectionUpwardForce * velocityMult);

            // Apply the initial impulse force
            rb.AddForce(ejectionForce, ForceMode.Impulse);

            // Apply Backwards Spin (Torque)
            // Local Forward (ejectionPort.forward) is used for the axis of rotation
            // Negative torque creates the backward spin
            rb.AddTorque(-ejectionPort.forward * ejectionTorque, ForceMode.Impulse);
        }

        // 3. Cleanup: Use Destroy for timed removal
        Destroy(casingInstance, casingLifetime);
    }

    IEnumerator ResetReload()
    {
        animator.SetBool("Reload", true);
        AudioSource.PlayClipAtPoint(currentInstance.reloadClip, barrelEnd.position, GunAudioVolume);
        yield return new WaitForSeconds(0.5f);
        //float reloadTime = 2.5f;
        //float reloadTime = animator.GetCurrentAnimatorStateInfo(0).length;
        //UnityEngine.Debug.Log("reload animation length is: " + reloadTime );
        //yield return new WaitForSeconds(reloadTime);
        animator.SetBool("Reload", false);
        _input.reload = false;
    }

    IEnumerator ResetMelee()
    {
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Fire", false);
    }

}

