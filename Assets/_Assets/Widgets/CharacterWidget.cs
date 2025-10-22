using TMPro;
using System;
using UnityEngine;

public class CharacterWidget : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mCharacterNameText;
    internal void SetBattleCharacter(BattleCharacter battleCharacter) 
    {
        mCharacterNameText.SetText(battleCharacter.Name);
    }
}
