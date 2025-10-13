using Cinemachine;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 4.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 8.0f;
        public static float SprintCache = 8.0f;

        public float RollMultiplier = 1.5f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        [SerializeField] private CinemachineVirtualCamera normalCam;
        [SerializeField] private CinemachineVirtualCamera aimCam;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        //private float rollBoostMultiplier = 1.5f;
        //private float rollBoostDuration = 0.37f; // Match your roll animation duration
        private bool _rollBoost = false;  // default = no boost

        [SerializeField] private int torsoAimLayerIndex = 2; // whatever index it is in Animator
        //[SerializeField] private string chestBoneName = "Spine2"; // or Chest
        [SerializeField] private float torsoAimWeightSpeed = 5f;

        private Transform chestBone;
        private float currentTorsoLayerWeight;
        public LayerMask shootMask;
        public float maxDistance = 1500f;
        public Vector3 aimPoint;
        public Transform playerTransform;
        [SerializeField] private float torsoAimSmooth = 1f;


        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDRoll;
        private int _animIDWalkBack;
        private int _aimingLayerIndex;
        public float _aimLayerWeight = 1.0f;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private PlayerInventory playerInventory;
        public string equippedWeapon;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            playerInventory = GetComponent<PlayerInventory>();
             // Cache chest bone
            chestBone =  _animator.GetBoneTransform(HumanBodyBones.Chest);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            JumpAndGravity();
            GroundedCheck();
            Move();
            Dive();
            Aim();
            Equipped();
            SwitchWeapon();
            UpdateAimPoint();
            
        }

        private void LateUpdate()
        {
            CameraRotation();
            //UpdateTorsoAim();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDRoll = Animator.StringToHash("Roll");
            _animIDWalkBack = Animator.StringToHash("WalkBack");
            //_aimingLayerIndex = Animator.StringToHash("Aiming");
            _aimingLayerIndex = _animator.GetLayerIndex("Aiming");
            
            //_animIDDiveRight = _hasAnimator.StringToHash("RollRight");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

     
        private void Move()
        {
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // ✅ Rotation logic
            if (_input.aim > 0.5f) // aiming
            {
                // Lock player to camera forward
                Vector3 camForward = _mainCamera.transform.forward;
                camForward.y = 0;
                // ✅ Smoothly rotate toward camera forward
                Quaternion targetRotation = Quaternion.LookRotation(camForward);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime * 15f // ← adjust this multiplier for rotation speed
                );

                // Movement relative to camera forward/right
                Vector3 camRight = _mainCamera.transform.right;
                camRight.y = 0;

                Vector3 moveDir = (camForward.normalized * inputDirection.z +
                                   camRight.normalized * inputDirection.x).normalized;

                _controller.Move(moveDir * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }
            else // normal movement
            {
                if (_input.move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }

            // ✅ Animator
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void Dive()
        {
            if(Grounded)
            {
              //!_animator.GetBool(_animIDRoll)
              if (_input.roll && !_animator.GetBool(_animIDRoll))
               {
                 _input.roll = false;
                 UnityEngine.Debug.Log("Diving!");
                 // update animator if using character
                 // a reference to the players current horizontal velocity
                 float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
                if (_hasAnimator && (currentHorizontalSpeed > 1))
                {
                 _animator.SetBool(_animIDRoll, true);
                 _animator.SetLayerWeight(_aimingLayerIndex, 0);
                }
                else
                {
                 UnityEngine.Debug.Log("No velocity!");
                 return;
                 
                }

                  //Invoke(nameof(ResetRoll), 0.37f); // Assuming your roll animation is ~0.8 seconds
                  StartCoroutine(ApplyRollBoost());
                  _input.roll = false;
                }      
                    //dive right or left depending on input vector
            }
        }

        private IEnumerator ApplyRollBoost()
        {
            _rollBoost = true;
            yield return new WaitForSeconds(0.80f); // adjust for your animation length
            _rollBoost = false;
            ResetRoll();
        }

        private void ResetRoll()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDRoll, false);
                UnityEngine.Debug.Log("Roll reset.");
            }
        }

        private void Equipped()
        {
                // We'll update this string every frame to see the current weapon
                if (playerInventory != null && playerInventory.equippedWeaponKey != null)
                {
                    equippedWeapon = playerInventory.equippedWeaponKey; // Assuming your Weapon class has a 'weaponName' string.
                    // Set the layer weight to 1 to fully activate the aiming layer
                    if(_animator.GetBool(_animIDRoll) != true)
                     {
                      _animator.SetLayerWeight(_aimingLayerIndex, _aimLayerWeight);
                     }
                   
                    //set idle animation
                    if(equippedWeapon == "AutoMag")
                    {
                        _animator.SetInteger("WeaponID", 1);
                    }
                    if(equippedWeapon == "M2 Carbine")
                    {
                        _animator.SetInteger("WeaponID", 2);
                    }
                    
                }
                else
                {
                    equippedWeapon = "No Weapon Equipped";
                    _animator.SetLayerWeight(_aimingLayerIndex, 0);
                }
        }

        private void Aim()
        {
             float isAiming = _input.aim; // This checks if the left trigger is being pressed

             //UnityEngine.Debug.Log("This is input.aim:");
             //UnityEngine.Debug.Log(_input.aim);
            //isShooting = _input.shooting;
            bool isShooting = false; 

            //animator.SetBool("IsAiming", isAiming);

            aimCam.Priority = isAiming > 0.0f ? 20 : 5;
            normalCam.Priority = isAiming > 0.0f ? 5 : 10;

            SprintSpeed = isAiming > 0.0f ? SprintCache*0.75f: SprintCache;

            if (isAiming > 0.0f)
            {
                _animator.SetLayerWeight(_aimingLayerIndex, 1);
                // GET CURRENT WEAPON LOGIC HERE 
                //AimTowardMouse();
                _animator.SetBool("isAiming", true);

                if (isShooting)
                {
                    // TODO: Add accurate aiming logic here
                }
            }
            else if (isShooting)
            {
                // TODO: Add hip-fire logic here
                //_animator.SetBool(_animIDWalkBack, false);
                //animator.SetBool("IsAiming", false);
            }
            else
            {
                //_animator.SetBool(_animIDWalkBack, false);
                _animator.SetBool("isAiming", false);
            }
        }
        
        public void UpdateTorsoAim()
        {
            // Smooth layer weight on/off depending on aim
            float targetWeight = _input.aim > 0.5f ? 1f : 0f;
            currentTorsoLayerWeight = Mathf.Lerp(
                currentTorsoLayerWeight, 
                targetWeight, 
                Time.deltaTime * torsoAimWeightSpeed
            );
            _animator.SetLayerWeight(2, currentTorsoLayerWeight);

            // Shoot a ray towards the center of the screen
            //Vector3 aimPoint;
            Ray camRay = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

            if (Physics.Raycast(camRay, out RaycastHit camHit, maxDistance, shootMask))
            {
                aimPoint = camHit.point;
            }
            else
            {
                aimPoint = _mainCamera.transform.position + _mainCamera.transform.forward * maxDistance;
            }

            // Only adjust chest when aiming
            if (_input.aim > 0.5f && _animator)
            {
                float pitch = _cinemachineTargetPitch; // your camera pitch input

                Transform chest = _animator.GetBoneTransform(HumanBodyBones.Chest);
                Transform chestParent = chest.parent;

                // direction to aim target
                Vector3 dir = (aimPoint - chest.position).normalized;
                Vector3 flatDir = new Vector3(dir.x, 0f, dir.z).normalized;

                // yaw from aim point
                Quaternion worldYaw = Quaternion.LookRotation(flatDir, Vector3.up);

                // pitch from input
                Quaternion pitchRot = Quaternion.Euler(-pitch, 0f, 0f);

                // combine yaw + pitch
                Quaternion worldRot = worldYaw * pitchRot;

                // convert to local space
                Quaternion localRot = Quaternion.Inverse(chestParent.rotation) * worldRot;

                // apply to chest bone via Animator
                _animator.SetBoneLocalRotation(HumanBodyBones.Chest, localRot);

                // Debug
                UnityEngine.Debug.Log("Torso Aim LocalRot: " + localRot.eulerAngles);
            }
        }
      


        private void SwitchWeapon()
        {
             // Scroll wheel (mouse) weapon switching
            /*float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f) 
            {
                playerInventory.NextWeapon();
            }
            else if (scroll < 0f) 
            {
                playerInventory.PreviousWeapon();
            }

             // Number keys (1, 2, 3, …)
            if (Input.GetKeyDown(KeyCode.Alpha1))
                playerInventory.EquipWeapon("AutoMag"); // match name in your inventory
            if (Input.GetKeyDown(KeyCode.Alpha2))
                playerInventory.EquipWeapon("M2Carbine");
            if (Input.GetKeyDown(KeyCode.Alpha3))
                playerInventory.EquipWeapon("CombatPump");
            if (Input.GetKeyDown(KeyCode.Alpha4))
                playerInventory.EquipWeapon("MosinNagant");
            if (Input.GetKeyDown(KeyCode.Alpha5))
                playerInventory.EquipWeapon("Uzi");
            if (Input.GetKeyDown(KeyCode.Alpha6))
                playerInventory.EquipWeapon("SteyrAug");
            if (Input.GetKeyDown(KeyCode.Alpha7))
                playerInventory.EquipWeapon("Dynamite");
            if (Input.GetKeyDown(KeyCode.Alpha8))
                playerInventory.EquipWeapon("Thumper");
            if (Input.GetKeyDown(KeyCode.Alpha9))
                playerInventory.EquipWeapon("Chainsaw");
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                playerInventory.EquipWeapon("Machete");
            }*/

            if(_input.switchleft)
            {
                UnityEngine.Debug.Log("switch left hit!");
                playerInventory.PreviousWeapon();
                _input.switchleft = false;
                
            }
            if (_input.switchright)
            {
                 UnityEngine.Debug.Log("switch right hit!");
                playerInventory.NextWeapon();
                _input.switchright = false;
            }
            // etc…

        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        // Call this to update the aim target before IK
        private void UpdateAimPoint()
        {
            // ... (Aim raycast logic you already have) ...
            Ray camRay = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);

            if (Physics.Raycast(camRay, out RaycastHit camHit, maxDistance, shootMask))
            {
                aimPoint = camHit.point;
            }
            else
            {
                aimPoint = _mainCamera.transform.position + _mainCamera.transform.forward * maxDistance;
            }
            UnityEngine.Debug.DrawLine(camRay.origin, aimPoint, Color.red, Time.deltaTime, false); 
        }

        void OnAnimatorIK(int torsoAimLayerIndex)
        {
            if (!_animator) return;

            if (_input.aim > 0.5f && _animator)
            {
                // Smooth layer weight on/off depending on aim
                float targetWeight = _input.aim > 0.5f ? 1f : 0f;
                currentTorsoLayerWeight = Mathf.Lerp(currentTorsoLayerWeight, targetWeight, Time.deltaTime * torsoAimWeightSpeed);
                _animator.SetLayerWeight(2, currentTorsoLayerWeight);

                // Only adjust chest when aiming
                /*if ((_input.aim > 0.5f) && _animator)
                {
                    // Convert camera pitch into a small rotation offset
                    float pitch = _cinemachineTargetPitch; // from your CameraRotation method
                    // Current local chest rotation as set by animator
                    Quaternion currentRot = _animator.GetBoneTransform(HumanBodyBones.Chest).localRotation;

                    // Build a *relative* pitch-only rotation
                    Quaternion pitchOffset = Quaternion.Euler(0f, 0f, -(pitch));

                    // Combine rotations: animator’s pose * our pitch offset
                    Quaternion newRot = currentRot * pitchOffset;

                    // Apply back
                    _animator.SetBoneLocalRotation(HumanBodyBones.Chest, newRot);
                    //Quaternion localRot = Quaternion.Euler(0f, 0f, -(pitch));

                    // Apply to chest (relative to its original pose)
                    //chestBone.localRotation = localRot; (NOT WORKING)
                    //chestBone.transform.localRotation = localRot; (NOT WORKING)
                    //_animator.SetBoneLocalRotation(HumanBodyBones.Chest, localRot);
                    //UnityEngine.Debug.Log(pitch);
                    //UnityEngine.Debug.Log("SETTING LOCAL ROT");
                }*/
                // NEW 
                Transform chest = _animator.GetBoneTransform(HumanBodyBones.UpperChest);
                Transform chestParent = chest.parent;
                Transform playerRoot = playerTransform; // assign this (your character forward)

                // Safety
                Vector3 toTarget = aimPoint - chest.position;
                //Vector3 toTargetFlat = Vector3.ProjectOnPlane(toTarget, chestParent.up);
                Vector3 toTargetFlat = Vector3.ProjectOnPlane(toTarget, playerRoot.up);
                //if (toTargetFlat.sqrMagnitude < 1e-6f) return;

                Vector3 parentForwardFlat = Vector3.ProjectOnPlane(playerRoot.forward, playerRoot.up).normalized;

                float yawLocalDegrees = Vector3.SignedAngle(parentForwardFlat, toTargetFlat.normalized, playerRoot.up);

                yawLocalDegrees = Mathf.Clamp(yawLocalDegrees, -1f, 20f);

                //UnityEngine.Debug.Log("Yaw local degrees:" + yawLocalDegrees);

                // camera pitch -> local pitch (adjust sign to match your camera)
                float pitchLocalDegrees = _cinemachineTargetPitch;

                // Build a *relative* pitch-only rotation
                Quaternion pitchOffset = Quaternion.Euler(0f, 0f, -(pitchLocalDegrees));

                //Quaternion yawOffset = Quaternion.Euler(-8f,0f,0f);
                Quaternion yawOffset = Quaternion.Euler(-(yawLocalDegrees),0f,0f);
                // smooth
                //targetLocal = Quaternion.Slerp(chest.localRotation, targetLocal, Time.deltaTime * torsoAimSmooth);
                Quaternion currentRot = _animator.GetBoneTransform(HumanBodyBones.UpperChest).localRotation;

                Quaternion newRot = currentRot * pitchOffset;

                newRot = newRot * yawOffset;

                _animator.SetBoneLocalRotation(HumanBodyBones.UpperChest, newRot);
            }
            // END NEW 

        }
    }
}