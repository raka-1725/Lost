using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    List<BattleSite> mBattleSites;
    List<BattleCharacter> mBattleCharacters = new List<BattleCharacter>();
    public void StartBattle(BattlePartyComponent playerParty, BattlePartyComponent enemyParty) 
    {
        if (mBattleSites == null) 
        {
            mBattleSites = new List<BattleSite>();
            mBattleSites.AddRange(GameObject.FindObjectsByType<BattleSite>(FindObjectsSortMode.None)); ;
        }
        Debug.Log($"Starting Battle between: {playerParty.gameObject.name} and {enemyParty.gameObject.name}");
        PrepParty(playerParty);
        PrepParty(enemyParty);
        StartCoroutine(StartTurns());
    }

    IEnumerator StartTurns() 
    {
        //TODO refactor to not hard code the delay
        yield return new WaitForSeconds(0);
        NextTurn();
    }
    void NextTurn() 
    {
        mBattleCharacters = mBattleCharacters.OrderBy((battleCharacter) => { return battleCharacter.CoolDownTimeRemaining; }).ToList();
        float advanceTime = mBattleCharacters[0].CoolDownTimeRemaining;
        foreach (BattleCharacter battleCharacter in mBattleCharacters)
        {
            battleCharacter.CoolDownSubtract(advanceTime);
        }
        BattleCharacter nextInturn = mBattleCharacters[0];
        nextInturn.TakeTurn();
        mBattleCharacters.Remove(nextInturn);
        mBattleCharacters.Add(nextInturn);
        
    }

    private void PrepParty(BattlePartyComponent party) 
    {
        BattleSite partyBattleSite = mBattleSites.Find((battleSite) => { return !battleSite.IsPlayerSite; });
        if (party.gameObject.CompareTag("Player")) 
        {
            partyBattleSite = mBattleSites.Find((battleSite) => { return battleSite.IsPlayerSite; });
        }
        int i = 0;
        foreach (BattleCharacter partyBattleCharacter in party.GetBattleCharacters()) 
        {
            partyBattleCharacter.transform.position = partyBattleSite.GetPositionForUnit(i);
            partyBattleCharacter.transform.rotation = partyBattleSite.transform.rotation;
            partyBattleCharacter.OnTurnFinished += NextTurn;
            mBattleCharacters.Add(partyBattleCharacter);
            i++;
        }

        party.FinishPrep();
    }
}
