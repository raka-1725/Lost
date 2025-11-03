using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        int partyID = OwningAbilityComponent.GetPartyID();
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().StartTargetting(partyID, true);
    }
}
