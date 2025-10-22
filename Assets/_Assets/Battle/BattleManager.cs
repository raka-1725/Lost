using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    List<BattleSite> mBattleSites;
    List<BattleCharacter> mBattleCharacters = new List<BattleCharacter>();

    Queue<BattleCharacter> mFirstRoundBattleCharacters = new Queue<BattleCharacter>();


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
        yield return new WaitForSeconds(2);
        UpdateTurnOrder();
        mFirstRoundBattleCharacters = new Queue<BattleCharacter>(mBattleCharacters);
        ProcessFirstRound();

    }
    private void ProcessFirstRound() 
    {
        if (mFirstRoundBattleCharacters.TryDequeue(out BattleCharacter nextBattleCharacter)) 
        {
            if (mBattleCharacters.Contains(nextBattleCharacter))
            {
                nextBattleCharacter.TakeTurn();
            }
            else
            {
                ProcessFirstRound();
            }
            return;
        }
        foreach (BattleCharacter battleCharacter in mBattleCharacters)
        {
            battleCharacter.OnTurnFinished -= ProcessFirstRound;
            battleCharacter.OnTurnFinished += NextTurn;
        }

        NextTurn();
    }
    void NextTurn()
    {
        Debug.Log("Next turn");
        UpdateTurnOrder();

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

    private void UpdateTurnOrder()
    {
        mBattleCharacters = mBattleCharacters.OrderBy((battleCharacter) => 
        { return battleCharacter.CoolDownTimeRemaining; }).ThenBy((battleCharacter) => 
        { return 1/battleCharacter.Speed; }).ToList();

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
            partyBattleCharacter.OnTurnFinished += ProcessFirstRound;
            mBattleCharacters.Add(partyBattleCharacter);
            i++;
        }

        party.FinishPrep();
    }
}
