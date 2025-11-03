using System.Collections.Generic;
using UnityEngine;

public interface ITargetService
{
    public List<BattleCharacter> GetTargetForTeam(int teamID, bool hostileTargets);
    public TargetingComponent GetTargetingComponent();
}
