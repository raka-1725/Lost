using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BattleWidget : MonoBehaviour
{
    [SerializeField] CharacterWidget mCharacterControlWidget;
    [SerializeField] LayoutGroup mAbilityListLayoutGroup;
    [SerializeField] AbilityWidget mAbilityWidgetPrefab;

    List<AbilityWidget> mAbilityWidgets = new List<AbilityWidget>();

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

        EventSystem.current.SetSelectedGameObject(mAbilityWidgets[0].gameObject);
    }

    private void AddAbilitytoAbilityList(Ability ability) 
    {
        AbilityWidget newAbilityWidget = Instantiate(mAbilityWidgetPrefab, mAbilityListLayoutGroup.transform);
        mAbilityWidgets.Add(newAbilityWidget);
        newAbilityWidget.SetAbility(ability);

    }
}
