using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingComponent : MonoBehaviour
{
    BattleInputActions mBattleInputActions;

    Vector2 mNavigationInput;

    ITargetService mTargetService;

    List<BattleCharacter> mTargets = new List<BattleCharacter>();

    bool bNavigationRest = true;

    private int mTargetIndex = 0;

    public void SetTargetService(ITargetService targetService) 
    {
        mTargetService = targetService;
    }

    public void StartTargetting(int partyID, bool hostile) 
    {
        mBattleInputActions.Enable();

        mTargets.Clear();
        mTargets = mTargetService.GetTargetForTeam(partyID, hostile);
        mTargets[0].SetHighLighted(true);
    }

    private void Awake()
    {
        mBattleInputActions = new BattleInputActions();
        mBattleInputActions.Battle.Navigation.performed += HandleTargetNavigation;
        mBattleInputActions.Battle.Navigation.canceled += HandleTargetNavigation;
        mBattleInputActions.Disable();

    }
    private void OnEnable()
    {
        mBattleInputActions.Enable();
    }
    private void OnDisable()
    {
        mBattleInputActions.Disable();
    }
    private void HandleTargetNavigation(InputAction.CallbackContext context)
    {
        mNavigationInput = context.ReadValue<Vector2>();
    }



    private void Update()
    {
        if (mNavigationInput.sqrMagnitude > 0.5 && bNavigationRest) 
        {
            bNavigationRest = false;
            Debug.Log($"Navigating with Input X : {mNavigationInput.x}");

            if (mNavigationInput.x == 1) 
            {
                mTargetIndex++;
                Debug.Log($"TaregetIndex : {mTargetIndex}");
                mTargets[mTargetIndex].SetHighLighted(true);
                if (mTargetIndex > mTargets.Count) 
                {
                    mTargetIndex = 0;
                }
            }
            if (mNavigationInput.x == -1) 
            {
                mTargetIndex--;
                Debug.Log($"TaregetIndex : {mTargetIndex}");
                mTargets[mTargetIndex].SetHighLighted(true);
                if (mTargetIndex < 0)
                {
                    mTargetIndex = mTargets.Count;
                }
            }
        }

        if (mNavigationInput.sqrMagnitude < 0.25) 
        {
            bNavigationRest = true;
        }
    }
}
