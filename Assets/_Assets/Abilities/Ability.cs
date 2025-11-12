using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }

    public AbilityComponent OwningAbilityComponent { get; private set; }

    public event Action onAbilityActivated;
    public event Action onabilityEnded;
    internal void Init(AbilityComponent newability)
    {
        OwningAbilityComponent = newability;
    }

    public virtual void ActivateAbility() 
    {
        Debug.Log("Acrtivating ability");
        onAbilityActivated?.Invoke();
    }

    public virtual void EndAbility() 
    {
        Debug.Log($"End Ability");
        onabilityEnded?.Invoke();
    }
}
