using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] initialAbilites;
    [SerializeField] Transform mTargettingFollowTransform;
    List<Ability> mAbilities = new List<Ability>();

    IViewClient mOwnerViewClient;

    public event Action onTargetCancelled;
    public event Action<BattleCharacter> onTargetPicked;

    public event Action onMoveToTargetFinished;
    public event Action onMoveBackToPartySpotFinished;
    public event Action<string> onGameplayEventRecieved;


    NavMeshAgent mNavMeshAgent;

    bool mHasReachedDestination = true;

    private void Awake()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();
    }
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

    public void StartTargetting(bool hostile) 
    {
        if (mOwnerViewClient is not null) 
        {
            mOwnerViewClient.PushViewTarget(mTargettingFollowTransform);
        }
        TargetingComponent targetingComponent = GameMode.MainGameMode.BattleManager.GetTargetingComponent();
        targetingComponent.onTargetCancelled -= CancelTargeting;
        targetingComponent.onTargetCancelled += CancelTargeting;
        SubscribeToTargetingDelegates();
        targetingComponent.StartTargetting(GetPartyID(), hostile);
    }

    void SubscribeToTargetingDelegates() 
    {
        UnSubscribeToTargettingDelegates();
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetCancelled += CancelTargeting;
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetPicked += TargetPicked;
    }
    void UnSubscribeToTargettingDelegates()
    {
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetCancelled -= CancelTargeting;
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().onTargetPicked -= TargetPicked;
    }

    private void TargetPicked(BattleCharacter character)
    {
        UnSubscribeToTargettingDelegates();

        if (mOwnerViewClient is not null)
        {
            mOwnerViewClient.PopViewTarget(mTargettingFollowTransform);
        }

        onTargetPicked?.Invoke(character);

    }

    private void CancelTargeting()
    {
        UnSubscribeToTargettingDelegates();

        if (mOwnerViewClient is not null) 
        {
            mOwnerViewClient.PopViewTarget(mTargettingFollowTransform);

        }

        onTargetCancelled?.Invoke();
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

    internal void SetViewClient(IViewClient viewClient)
    {
        mOwnerViewClient = viewClient;
    }

    public void MoveToTarget(Vector3 targetPosition) 
    {
        mHasReachedDestination = false;
        mNavMeshAgent.SetDestination(targetPosition);
    }

    private void Update()
    {
        UpdateNavigation();
    }

    private void UpdateNavigation()
    {
        if(mHasReachedDestination)
            return;
        if (mNavMeshAgent.pathPending) 
            return;

        if(mNavMeshAgent.remainingDistance > mNavMeshAgent.stoppingDistance)
            return;
        if (!mNavMeshAgent.hasPath || mNavMeshAgent.velocity.sqrMagnitude == 0f) 
        {
            mHasReachedDestination = true;
            onMoveToTargetFinished?.Invoke();
        }
    }

    public void HandleGameplayEvent(string eventTag)
    {
        onGameplayEventRecieved?.Invoke(eventTag);
    }

    internal void MoveBackToPartySpot()
    {
        
    }
}
