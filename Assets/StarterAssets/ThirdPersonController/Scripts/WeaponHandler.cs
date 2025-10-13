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
    public Animator animator;
    public Transform barrelEnd;  // Assign on instantiate
    public WeaponInstance currentInstance;
    public LineRenderer tracerPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject muzzlePointLight;
    public AudioClip PistolAudioClip;
    public AudioClip PistolReloadAudioClip;
    [Range(0, 1)] public float GunAudioVolume = 0.7f;
    [Header("Tracer Settings")]
    [SerializeField] private float tracerDuration = 0.5f;
    [SerializeField] private float range = 1000f;
    public StarterAssets.StarterAssetsInputs _input;
    //public float triggerDown;
    public float coolDown;
    public float fireRate;
    public float nextTimeToFire;

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

            // Assuming weaponPrefab is instantiated already and parented under player’s hand
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
                UnityEngine.Debug.Log("current.shoot()returned true");
                animator.SetTrigger("Fire");
                ShootHandler(current);
                // Muzzle flash, sound, etc.
                animator.ResetTrigger("Fire");
                coolDown = 0;
            }
        }
        else if(coolDown < nextTimeToFire) { 
            //UnityEngine.Debug.Log("COOLING DOWN HOSS"); 
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

    void ShootHandler(WeaponInstance weapon)
    {
        // direction from barrel to aim point
        Vector3 aimPoint = tpc.aimPoint;
        Vector3 dir = (aimPoint - barrelEnd.position).normalized;

        AudioSource.PlayClipAtPoint(PistolAudioClip, barrelEnd.position, GunAudioVolume);

        Vector3 recoilDir = _mainCamera.transform.forward + Vector3.up * 0.15f;
        impulseSource.GenerateImpulse(recoilDir.normalized);

        // Raycast from barrelEnd
        Ray ray = new Ray(barrelEnd.position, aimPoint);

        RaycastHit hit;

        StartCoroutine(FlashSequence()); 

        //Instantiate(testAss, barrelEnd.position, barrelEnd.rotation);

        if (Physics.Raycast(ray, out hit, range))
        {
            UnityEngine.Debug.Log("Hit " + hit.collider.name);
            UnityEngine.Debug.DrawLine(ray.origin, hit.point, Color.red, 5f, false); // duration 10s
            SpawnTracer(ray.origin, hit.point);
        }
        else
        {
            //UnityEngine.Debug.Log("Nothing getting hit...");
            //UnityEngine.Debug.DrawRay(ray.origin, ray.direction * range, Color.blue, 1f); // extend to see it
            SpawnTracer(ray.origin, barrelEnd.position + barrelEnd.forward * range);
            UnityEngine.Debug.DrawLine(ray.origin, hit.point, Color.blue, 5f, false); // duration 10s
        }

        EjectCasing();
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
        // Reset local scale so it doesn’t inherit weird world scale
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
    }

    public void CoolDownTimer()
    {
        //coolDown time
        coolDown += Time.deltaTime;
    }

    // Call this after a successful shot in HandleShoot()
    public void EjectCasing()
    {
        if (casingPrefab == null || ejectionPort == null) 
        {
            UnityEngine.Debug.Log("Casing Prefab or Ejection Port not assigned in WeaponHandler.");
            return;
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
            
            // Combine forces: mostly Up, slightly to the Side (Right)
            Vector3 ejectionForce = 
                (localRight * ejectionSideForce) + 
                (localUp * ejectionUpwardForce);
            
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
    AudioSource.PlayClipAtPoint(PistolReloadAudioClip, barrelEnd.position, GunAudioVolume);
    yield return new WaitForSeconds(0.5f); 
    //float reloadTime = 2.5f;
    //float reloadTime = animator.GetCurrentAnimatorStateInfo(0).length;
    //UnityEngine.Debug.Log("reload animation length is: " + reloadTime );
    //yield return new WaitForSeconds(reloadTime);
    animator.SetBool("Reload", false);
    _input.reload = false;
}

}

