using System;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [SerializeField] GameObject mTurnIndicator;
    public float CoolDownDuration => 1f / Speed;
    public float CoolDownTimeRemaining { get; private set; }
    public event Action OnTurnFinished;
    private void Awake()
    {
        CoolDownTimeRemaining = CoolDownDuration;
        mTurnIndicator.SetActive(false);
    }
    public void CoolDownSubtract(float duration) 
    {
        CoolDownTimeRemaining -= duration;
    }
    public void TakeTurn() 
    {
        Invoke("FinishTurn", 1);
        mTurnIndicator.SetActive(true);
        CoolDownTimeRemaining = CoolDownDuration;
    }

    public void FinishTurn() 
    {
        mTurnIndicator.SetActive(false);
        OnTurnFinished?.Invoke();
    }
}
