using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AbilityWidget : MonoBehaviour
{
    

    Button mButton;
    TextMeshProUGUI mText;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(ActivateAbility);

        mText = GetComponentInChildren<TextMeshProUGUI>();
    }
    Ability mAbility;
    public void SetAbility(Ability ability) 
    {
        mAbility = ability;
    }

    void ActivateAbility() 
    {
        mAbility.ActivateAbility();
    }

    public void SetText(string text) 
    {
        mText.text = text;
    }
}
