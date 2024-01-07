using EasyCharacterMovement;
using Nova;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Matroska
{
	public class CMThirdPersonCharacter : Matroska.Controller.Character
{
#region SerializedFields
	[SerializeField] private Transform cameraTarget;
#endregion

#region Properties
	public EasyCharacterMovement.CharacterLook CharacterLook
	{
		get
		{
			if (_characterLook == null)
				_characterLook = GetComponent<EasyCharacterMovement.CharacterLook>();

			return _characterLook;
		}
	}
#endregion

#region Private Fields

	private float _pitch;
	private float _yaw;

	private EasyCharacterMovement.CharacterLook _characterLook;

#endregion

#region Input Actions

	protected InputAction MouseLookInputAction { get; set; }
	protected InputAction CursorLockInputAction { get; set; }
	protected InputAction CursorUnlockInputAction { get; set; }

	/// <summary>
	/// Gets the mouse look value.
	/// Return its current value or zero if no valid InputAction found.
	/// </summary>
	protected virtual Vector2 GetMouseLookInput ()
	{
		return MouseLookInputAction?.ReadValue<Vector2>() ?? Vector2.zero;
	}
	
	protected virtual void OnCursorLock(InputAction.CallbackContext context)
	{
		// Do not allow to lock cursor if using UI
		if (EventSystem.current && EventSystem.current.IsPointerOverGameObject())
			return;

		if (context.started)
			CharacterLook.LockCursor();
	}
	
	protected virtual void OnCursorUnlock(InputAction.CallbackContext context)
	{
		if (context.started)
			CharacterLook.UnlockCursor();
	}

#endregion

#region Public Methods

	/// <summary>
	///  Rotate the camera along its yaw.
	/// </summary>
	[Button]
	public void AddCameraYawInput(float value)
	{
		_yaw = MathLib.Clamp0360(_yaw + value);
	}

	/// <summary>
	/// Rotate the camera along its pitch.
	/// </summary>
	[Button]
	public void AddCameraPitchInput(float value)
	{
		value = CharacterLook.invertLook ? value : -value;

		_pitch = Mathf.Clamp(_pitch + value, CharacterLook.minPitchAngle, CharacterLook.maxPitchAngle);
	}
#endregion

#region Override Methods

	// Extends HandleInput method to add camera input.
	protected override void HandleInput()
	{
		base.HandleInput();

		// Camera input (mouse look),
		// Rotates the camera target independently of the Character's rotation,
		// basically we are manually rotating the Cinemachine camera here
		if (IsDisabled())
			return;

		var mouseLookInput = GetMouseLookInput();

		if (mouseLookInput.x != 0.0f)
			AddCameraYawInput(mouseLookInput.x*CharacterLook.mouseHorizontalSensitivity);

		if (mouseLookInput.y != 0.0f)
			AddCameraPitchInput(-mouseLookInput.y*CharacterLook.mouseVerticalSensitivity);
	}

	protected override void OnLateUpdate()
	{
		base.OnLateUpdate();

		// Set final camera rotation
		cameraTarget.rotation = Quaternion.Euler(_pitch, _yaw, 0.0f);
	}

	protected override void InitPlayerInput()
	{
		base.InitPlayerInput();

		// Setup input action handlers
		MouseLookInputAction = inputActions.FindAction("Mouse Look");
		MouseLookInputAction?.Enable();

		CursorLockInputAction = inputActions.FindAction("Cursor Lock");
		if (CursorLockInputAction != null)
		{
			CursorLockInputAction.started += OnCursorLock;
			CursorLockInputAction.Enable();
		}

		CursorUnlockInputAction = inputActions.FindAction("Cursor Unlock");
		if (CursorUnlockInputAction != null)
		{
			CursorUnlockInputAction.started += OnCursorUnlock;
			CursorUnlockInputAction.Enable();
		}
	}

	[Button]
	protected override void DeinitPlayerInput()
	{
		base.DeinitPlayerInput();

		if (MouseLookInputAction != null)
		{
			MouseLookInputAction.Disable();
			MouseLookInputAction = null;
		}

		if (CursorLockInputAction != null)
		{
			CursorLockInputAction.started -= OnCursorLock;

			CursorLockInputAction.Disable();
			CursorLockInputAction = null;
		}

		if (CursorUnlockInputAction != null)
		{
			CursorUnlockInputAction.started -= OnCursorUnlock;

			CursorUnlockInputAction.Disable();
			CursorUnlockInputAction = null;
		}
	}

	protected override void OnStart()
	{
		base.OnStart();

		// Cache camera's initial orientation (yaw / pitch)
		var cameraTargetEulerAngles = cameraTarget.eulerAngles;

		_pitch = cameraTargetEulerAngles.x;
		_yaw = cameraTargetEulerAngles.y;
	}

#endregion
}
}

