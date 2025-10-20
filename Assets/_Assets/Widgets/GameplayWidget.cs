using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWidget : MonoBehaviour
{
    [SerializeField] Image mTransitionImage;
    [SerializeField] ChildSwitcher mMainSwitcher;
    [SerializeField] BattleWidget mBattleWidget;
    [SerializeField] GameObject mRoamingWidget;

    private void Awake()
    {
        mTransitionImage.gameObject.SetActive(false);
    }

    public void DipToBlack(float dipInAndOutDuration, float dipStayDuration, Action dippedToBlackCallback) 
    {
        StartCoroutine(StartDipToBlack(dipInAndOutDuration, dipStayDuration, dippedToBlackCallback));
    }

    public void SetFocusedCharacterInBattle(BattleCharacter battleCharacter) 
    {
        mBattleWidget.SetCharacterControlTarget(battleCharacter);
    }

    IEnumerator StartDipToBlack(float dipInAndOutDuration, float dipStayDuration, Action dippedToBlackCallback) 
    {
        float timeCounter = 0;
        mTransitionImage.gameObject.SetActive(true);
        Color transitionImageColor = Color.black;
        transitionImageColor.a = 0;
        while (timeCounter < dipInAndOutDuration) 
        {
            transitionImageColor.a = timeCounter/dipInAndOutDuration;
            mTransitionImage.color = transitionImageColor;

            timeCounter += Time.deltaTime;
            Debug.Log("DIP IN");
            yield return new WaitForEndOfFrame(); ;
        }
        transitionImageColor.a = 1;
        mTransitionImage.color = transitionImageColor;
        dippedToBlackCallback();

        yield return new WaitForSeconds(dipStayDuration);

        while (transitionImageColor.a > 0) 
        {
            transitionImageColor.a -= Time.deltaTime;
            mTransitionImage.color = transitionImageColor;
            yield return new WaitForEndOfFrame();
        }

        mTransitionImage.gameObject.SetActive(false);
    }

    internal void SwitchToBattle()
    {
        mMainSwitcher.SetActiveChild(mBattleWidget.gameObject);
    }

    internal void SwitchToRoaming() 
    {
        mMainSwitcher.SetActiveChild(mRoamingWidget);
    }
}
