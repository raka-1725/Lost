using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour, IViewClient
{
    [SerializeField] CameraRig mCameraRigPrefab;

    private PlayerInputActions mPlayerInputAction;
    private MovementController mMovementController;
    private BattlePartyComponent mBattlePartyComponent;

    private BattleState mBattleState;
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

        mBattlePartyComponent = GetComponent<BattlePartyComponent>();
    }

    private void OnEnable()
    {
        mPlayerInputAction.Enable();
    }

    private void OnDisable()
    {
        mPlayerInputAction.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject) 
        {
            return;
        }
        BattlePartyComponent otherBattlePartyComponent = other.GetComponent<BattlePartyComponent>();
        if (otherBattlePartyComponent && !IsInBattle()) 
        {
            GameMode.MainGameMode.BattleManager.StartBattle(mBattlePartyComponent, otherBattlePartyComponent);
            SwitchToBattle(BattleState.InBattle);
        }
    }

    private void SwitchToBattle(BattleState battleState)
    {
        if (battleState == BattleState.InBattle) 
        {
            mPlayerInputAction.Gameplay.Disable();
        }
        if (battleState == BattleState.Roaming) 
        {
            mPlayerInputAction.Gameplay.Enable();
        }
        

    }

    private bool IsInBattle() 
    {
        return mBattleState == BattleState.InBattle;
    }

    public void SetViewTarget(Transform viewTarget)
    {
        mCameraRig.SetFollowTransform(viewTarget);
        mCameraRig.transform.rotation = viewTarget.transform.rotation;
    }

    public void ResetViewAngle()
    {
        mCameraRig.ResetViewAngle();
    }
}
