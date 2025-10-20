using TMPro;
using UnityEditor.Media;
using UnityEngine;

public class CharacterWidget : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mCharacterNameText;
    internal void SetBattleCharacter(BattleCharacter battleCharacter) 
    {
        mCharacterNameText.SetText(battleCharacter.gameObject.name);
    }
}
