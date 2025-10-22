using UnityEngine;
using UnityEngine.UI;

public class BattleWidget : MonoBehaviour
{
    [SerializeField] CharacterWidget mCharacterControlWidget;
    [SerializeField] LayoutGroup mAbilityListLayoutGroup;
    [SerializeField] AbilityWidget mAbilityWidgetPrefab;

    public void SetCharacterControlTarget(BattleCharacter battleCharacter) 
    {
        foreach (Transform existingEntries in mAbilityListLayoutGroup.transform) 
        {
            Destroy(existingEntries.gameObject);
        }


        mCharacterControlWidget.gameObject.SetActive(true);
        mCharacterControlWidget.SetBattleCharacter(battleCharacter);
        AbilityComponent abilitycomponent = battleCharacter.GetAbilityComponent();
        if (abilitycomponent) 
        {
            foreach (Ability ability in abilitycomponent.GetAbilities()) 
            {
                AddAbilitytoAbilityList(ability);
            }
        }
    }

    private void AddAbilitytoAbilityList(Ability ability) 
    {
        AbilityWidget newAbilityWidget = Instantiate(mAbilityWidgetPrefab, mAbilityListLayoutGroup.transform);
        newAbilityWidget.SetAbility(ability);
        newAbilityWidget.SetText("NEW ABILITY");
    }
}
