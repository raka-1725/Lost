using UnityEngine.UI;
using UnityEngine;

public class AbilityWidget : MonoBehaviour
{
    Button mButton;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(ActivateAbility);
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
}
