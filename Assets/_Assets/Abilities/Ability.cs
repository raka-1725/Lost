using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [field: SerializeField] public string AbilityName { get; private set; }

    AbilityComponent mOwningAbilityComponent;
    internal void Init(AbilityComponent newability)
    {
        mOwningAbilityComponent = newability;
    }

    public virtual void ActivateAbility() 
    {
        Debug.Log("Acrtivating ability");
    }
    
}
