using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    BattleCharacter mTarget;
    [SerializeField] float mDamageAmount = 20f;
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        OwningAbilityComponent.StartTargetting(true);

        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;
        OwningAbilityComponent.onTargetPicked += TargetPicked;
        OwningAbilityComponent.onTargetCancelled += TargetCancelled;
    }
    private void TargetCancelled()
    {
        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;
        EndAbility();
    }

    private void TargetPicked(BattleCharacter character)
    {
        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;
        Debug.Log($"attacking : {character.gameObject.name}");

        OwningAbilityComponent.MoveToTarget(character.transform.position);
        OwningAbilityComponent.onMoveToTargetFinished -= MovedToTarget;
        OwningAbilityComponent.onMoveToTargetFinished += MovedToTarget;
    }

    private void MovedToTarget()
    {
        OwningAbilityComponent.onMoveToTargetFinished -= MovedToTarget;
        OwningAbilityComponent.GetComponent<Animator>().SetTrigger("Attack");
        OwningAbilityComponent.onGameplayEventRecieved += HandleGameplayEvent;
    }

    public void HandleGameplayEvent(string eventTag) 
    {
        if (eventTag == "ApplyDamage") 
        {
            mTarget.TakeDamage(mDamageAmount);
            return;
        }

        if (eventTag == "AttackFinished") 
        {
            OwningAbilityComponent.MoveBackToPartySpot();
        }
        
    }
}
