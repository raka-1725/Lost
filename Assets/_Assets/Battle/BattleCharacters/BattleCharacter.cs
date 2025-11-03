using System;
using UnityEngine;

[RequireComponent(typeof(AbilityComponent))]
public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [field: SerializeField] public string Name { get; private set; } =  "BattleCharacter";

    [SerializeField] GameObject mTurnIndicator;
    public float CoolDownDuration => 1f / Speed;
    public float CoolDownTimeRemaining { get; private set; }

    public Action<BattleCharacter> onTurnStarted;

    AbilityComponent mAbilityComponent;
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
    public void TakeTurn() 
    {
        //Invoke("FinishTurn", 1);
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
