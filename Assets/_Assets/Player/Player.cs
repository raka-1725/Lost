using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour
{
    [SerializeField] CameraRig mCameraRigPrefab;

    private PlayerInputActions mPlayerInputAction;
    private MovementController mMovementController;

    CameraRig mCameraRig;

    private void Awake()
    {
        mMovementController = GetComponent<MovementController>();

        mPlayerInputAction = new PlayerInputActions();
        mPlayerInputAction.Gameplay.Jump.performed += mMovementController.PerformJump;

        mPlayerInputAction.Gameplay.Move.performed += mMovementController.HandleMoveInput;
        mPlayerInputAction.Gameplay.Move.canceled += mMovementController.HandleMoveInput;


        mCameraRig = Instantiate(mCameraRigPrefab);
        mCameraRig.SetFollowTransform(transform);

        mPlayerInputAction.Gameplay.Look.performed += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());
        mPlayerInputAction.Gameplay.Look.canceled += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());

    }

    private void OnEnable()
    {
        mPlayerInputAction.Enable();
    }

    private void OnDisable()
    {
        mPlayerInputAction.Disable();
    }
}
