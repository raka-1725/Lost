using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        OwningAbilityComponent.StartTargetting(true);

        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;
        OwningAbilityComponent.onTargetPicked += TargetPicked;
        OwningAbilityComponent.onTargetCancelled += TargetCancelled;
    }

    private void TargetPicked(BattleCharacter character)
    {
        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;
        Debug.Log($"attacking : {character.gameObject.name}");
    }

    private void TargetCancelled()
    {
        OwningAbilityComponent.onTargetPicked -= TargetPicked;
        OwningAbilityComponent.onTargetCancelled -= TargetCancelled;
        EndAbility();
    }
}
