using UnityEngine;

public class BattleManager
{
    public void StartBattle(BattlePartyComponent partyOne, BattlePartyComponent partyTwo) 
    {
        Debug.Log($"Starting Battle betweem: {partyOne.gameObject.name} and {partyTwo.gameObject.name}");
    }
}
