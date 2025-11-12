using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AbilityComponent))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class BattleCharacter : MonoBehaviour
{
    
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [field: SerializeField] public string Name { get; private set; } =  "BattleCharacter";

    [SerializeField] GameObject mTurnIndicator;
    public float CoolDownDuration => 1f / Speed;
    public float CoolDownTimeRemaining { get; private set; }

    public Action<BattleCharacter> onTurnStarted;

    AbilityComponent mAbilityComponent;

    public int PartyID { get; private set; }

    public void Init(int partyID, IViewClient viewClient) 
    {
        PartyID = partyID;
        if (mAbilityComponent == null) 
        {
            mAbilityComponent = GetComponent<AbilityComponent>();
        }
        if (mAbilityComponent) 
        {
            mAbilityComponent.SetViewClient(viewClient);
        }
    }
    public AbilityComponent GetAbilityComponent() 
    {
        return mAbilityComponent;
    }

    public event Action OnTurnFinished;
    private void Awake()
    {
        CoolDownTimeRemaining = CoolDownDuration;
        mTurnIndicator.SetActive(false);

        mAbilityComponent = GetComponent<AbilityComponent>();
    }
    public void CoolDownSubtract(float duration) 
    {
        CoolDownTimeRemaining -= duration;
    }
    public void SetHighLighted(bool highlighted) 
    {
        mTurnIndicator.SetActive(highlighted);
    }

    public void TakeTurn() 
    {
        SetHighLighted(true);
        mTurnIndicator.SetActive(true);
        onTurnStarted?.Invoke(this);
        CoolDownTimeRemaining = CoolDownDuration;
    }

    public void FinishTurn() 
    {
        mTurnIndicator.SetActive(false);
        OnTurnFinished?.Invoke();
    }
}
