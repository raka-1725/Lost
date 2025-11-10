using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] initialAbilites;
    [SerializeField] Transform mTargettingFollowTransform;
    List<Ability> mAbilities = new List<Ability>();

    IViewClient mOwnerViewClient;

    public int GetPartyID() 
    {
        return GetComponent<BattleCharacter>().PartyID;
    }
    private void Start()
    {
        foreach (Ability initialABility in initialAbilites) 
        {
            GiveAbility(initialABility);
        }
    }

    public void StartTargetting(bool hostile) 
    {
        if (mOwnerViewClient is not null) 
        {
            mOwnerViewClient.PushViewTarget(mTargettingFollowTransform);
        }
        TargetingComponent targetingComponent = GameMode.MainGameMode.BattleManager.GetTargetingComponent();
        targetingComponent.onTargetCancelled -= CancelTargeting;
        targetingComponent.onTargetCancelled += CancelTargeting;
        targetingComponent.StartTargetting(GetPartyID(), hostile);
    }

    private void CancelTargeting()
    {
        if (mOwnerViewClient is not null) 
        {
            mOwnerViewClient.PopViewTarget(mTargettingFollowTransform);
        }
    }

    private void GiveAbility(Ability abilityDefaultObject) 
    {
        Ability newability = Instantiate(abilityDefaultObject);
        newability.Init(this);
        mAbilities.Add(newability);
    }

    internal IEnumerable<Ability> GetAbilities()
    {
        return mAbilities;
    }

    internal void SetViewClient(IViewClient viewClient)
    {
        mOwnerViewClient = viewClient;
    }
}
