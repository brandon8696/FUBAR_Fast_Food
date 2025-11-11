using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool roll;
        public float aim;
        public float shoot;
        public bool reload;
        public bool switchleft;
		public bool switchright;
		public bool interact;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnRoll(InputValue value)
		{
		   RollInput(value.isPressed);
		}
		public void OnReload(InputValue value)
		{
			ReloadInput(value.isPressed);
		}
		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
        }
        public void OnAim(InputValue value)
        {
            AimInput(value.Get<float>());
        }
        public void OnShoot(InputValue value)
        {
          ShootInput(value.Get<float>());
        }
        public void OnSwitchRight(InputValue value)
        {
            SwitchRightInput(value.isPressed);
        }
        public void OnSwitchLeft(InputValue value)
        {
            SwitchLeftInput(value.isPressed);
        }
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void RollInput(bool newRollState)
		{
			roll = newRollState;
		}

		public void ReloadInput(bool newReloadState)
		{
			reload = newReloadState;
		}
		public void InteractInput(bool newInteractState)
        {
			interact = newInteractState;
        }

        public void AimInput(float newAimState)
        {
            aim = newAimState;
        }

        public void ShootInput(float newShootState)
        {
            shoot = newShootState;
        }

        public void SwitchRightInput(bool newSwitchRightState)
        {
            switchright = newSwitchRightState;
        }

        public void SwitchLeftInput(bool newSwitchLeftState)
        {
            switchleft = newSwitchLeftState;
        }

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}