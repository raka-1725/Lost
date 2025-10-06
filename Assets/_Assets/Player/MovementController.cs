using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))] 

public class MovementController : MonoBehaviour
{
    [SerializeField] float mJumpSpeed = 15f;
    [SerializeField] float mMaxMoveSpeed = 2f;
    [SerializeField] float mGroundMoveSpeedAccelaration = 50f;
    [SerializeField] float mAirMoveSpeedAccelaration = 5f;
    [SerializeField] float mTurnLerpRate = 40f;
    [SerializeField] float mMaxFallSpeed = 50f;
    [SerializeField] float mAirCheckRadius = .2f;
    [SerializeField] LayerMask mAirCheckLayerMask = 1;


    private CharacterController mCharacterController;
    private Vector3 mVerticalVelocity;
    private Vector3 mHorizontalVelocity;
    private Vector2 mMoveInput;

    private bool mShouldTryJump;
    private bool mIsInAir;

    private Animator mAnimator;


    private void Awake()
    {
        mCharacterController = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
    }
    public void HandleMoveInput(InputAction.CallbackContext context) 
    {
        mMoveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move input is : {mMoveInput}");
    }
    public void PerformJump(InputAction.CallbackContext context) 
    {
        //Debug.Log("JUMP");

        if (!mIsInAir) 
        {
            mShouldTryJump = true;
        }
    }

    bool IsInAir() 
    {
        if (mCharacterController.isGrounded) 
        {
            return false;
        }

        Collider[] airCheckColliders = Physics.OverlapSphere(transform.position, mAirCheckRadius, mAirCheckLayerMask);
        foreach (Collider collider in airCheckColliders)
        {
            if (collider.gameObject != gameObject) 
            {
                return false;
            }
        }

        return true;
    }


    private void Update()
    {
        mIsInAir = IsInAir();

        UpdateVerticalVelocity();
        UpdateHorizontalVelocity();

        UpdateTransform();
        UpdateAnimation();
        Debug.Log($"is grounded: {mCharacterController.isGrounded}");
    }

    private void UpdateAnimation()
    {
        mAnimator.SetFloat("Speed", mHorizontalVelocity.magnitude);
        mAnimator.SetBool("Landed", !mIsInAir);
    }

    private void UpdateTransform()
    {
        mCharacterController.Move((mHorizontalVelocity + mVerticalVelocity) * Time.deltaTime);
        if (mHorizontalVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mHorizontalVelocity.normalized, Vector3.up), Time.deltaTime * mTurnLerpRate);
        }
    }

    private void UpdateVerticalVelocity()
    {
        if (mShouldTryJump && !mIsInAir) 
        {
            mVerticalVelocity.y = mJumpSpeed;
            mAnimator.SetTrigger("Jump");
            mShouldTryJump = false;
            return;
        }
        //on the ground, set the velocity to a small velocity going down
        if (mCharacterController.isGrounded) 
        {
            mAnimator.ResetTrigger("Jump");
            mVerticalVelocity.y = -1f;
            return;
        }

        //free fall
        if (mVerticalVelocity.y > -mMaxFallSpeed)
        {
            mVerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = mIsInAir ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, mAirCheckRadius);
    }
}
