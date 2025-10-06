using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] 

public class MovementController : MonoBehaviour
{
    [SerializeField] float mJumpSpeed = 15f;
    [SerializeField] float mMaxMoveSpeed = 2f;
    [SerializeField] float mGroundMoveSpeedAccelaration = 50f;
    [SerializeField] float mAirMoveSpeedAccelaration = 5f;
    [SerializeField] float mMaxFallSpeed = 50f;

    private PlayerInputActions mPlayerInputAction;
    private CharacterController mCharacterController;
    private Vector3 mVerticalVelocity;
    private Vector3 mHorizontalVelocity;
    private Vector2 mMoveInput;

    private void Awake()
    {
        mPlayerInputAction = new PlayerInputActions();
        mPlayerInputAction.Gameplay.Jump.performed += PerformJump;

        mPlayerInputAction.Gameplay.Move.performed += HandleMoveInput;
        mPlayerInputAction.Gameplay.Move.canceled += HandleMoveInput;

        mCharacterController = GetComponent<CharacterController>();
        
    }
    private void HandleMoveInput(InputAction.CallbackContext context) 
    {
        mMoveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move input is : {mMoveInput}");
    }
    private void PerformJump(InputAction.CallbackContext context) 
    {
        Debug.Log("JUMP");

        if (mCharacterController.isGrounded) 
        {
            mVerticalVelocity.y = mJumpSpeed;
        }
    }

    private void OnEnable()
    {
        mPlayerInputAction.Enable();
    }

    private void OnDisable()
    {
        mPlayerInputAction.Disable();
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (mVerticalVelocity.y > -mMaxFallSpeed) 
        {
            mVerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        UpdateHorizontalVelocity();

        mCharacterController.Move((mHorizontalVelocity + mVerticalVelocity) * Time.deltaTime);
    }

    void UpdateHorizontalVelocity() 
    {
        Vector3 moveDirection = PlayerInputToWorldDirection(mMoveInput);

        float acceleration = mCharacterController.isGrounded ? mGroundMoveSpeedAccelaration : mAirMoveSpeedAccelaration;
        if (moveDirection.sqrMagnitude > 0)
        {
            mHorizontalVelocity = Vector3.ClampMagnitude(mHorizontalVelocity, mMaxMoveSpeed);
            mHorizontalVelocity += moveDirection * acceleration * Time.deltaTime;

        }
        else 
        {
            if (mHorizontalVelocity.sqrMagnitude > 0) 
            {
                mHorizontalVelocity -= mHorizontalVelocity.normalized * acceleration * Time.deltaTime;
                if (mHorizontalVelocity.sqrMagnitude < 0.1) 
                {
                    mHorizontalVelocity = Vector3.zero;
                }
            }
        }
    }

    Vector3 PlayerInputToWorldDirection(Vector2 inputValue) 
    {
        Vector3 rightDirection = Camera.main.transform.right;
        Vector3 fwdDirection = Vector3.Cross(rightDirection, Vector3.up);

        return rightDirection * inputValue.x + fwdDirection * inputValue.y;
    }
}
