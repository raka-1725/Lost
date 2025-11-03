using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }

    public AbilityComponent OwningAbilityComponent { get; private set; }
    internal void Init(AbilityComponent newability)
    {
        OwningAbilityComponent = newability;
    }

    public virtual void ActivateAbility() 
    {
        Debug.Log("Acrtivating ability");
    }
    
}
