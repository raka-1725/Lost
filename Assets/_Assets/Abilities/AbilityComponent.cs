using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] initialAbilites;
    List<Ability> mAbilities = new List<Ability>();

    public int GetPartyID() 
    {
        return GetComponent<BattleCharacter>().PartyID;
    }
    private void Start()
    {
        foreach (Ability initialABility in initialAbilites) 
        {
            GiveAbility(initialABility);
        }
    }

    private void GiveAbility(Ability abilityDefaultObject) 
    {
        Ability newability = Instantiate(abilityDefaultObject);
        newability.Init(this);
        mAbilities.Add(newability);
    }

    internal IEnumerable<Ability> GetAbilities()
    {
        return mAbilities;
    }
}
