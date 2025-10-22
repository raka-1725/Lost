using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AbilityWidget : MonoBehaviour
{
    

    Button mButton;
    TextMeshProUGUI mAbilityNameText;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(ActivateAbility);

        mAbilityNameText = GetComponentInChildren<TextMeshProUGUI>();
    }
    Ability mAbility;
    public void SetAbility(Ability ability) 
    {
        mAbility = ability;
        mAbilityNameText.SetText(ability.name);
    }

    void ActivateAbility() 
    {
        mAbility.ActivateAbility();
    }

    public void SetText(string text) 
    {
        mAbilityNameText.text = text;
    }
}
