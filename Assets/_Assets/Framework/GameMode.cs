using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] Player mPlayerGameObjectPrefab;
    Player mPlayerGameObject;

    public Player mPlayer => mPlayerGameObject;
    public static GameMode MainGameMode;

    public BattleManager BattleManager{ get; private set; }
    private void OnDestroy()
    {
        if (MainGameMode == this) 
        {
            MainGameMode = null;
        }
    }
    private void Awake()
    {
        if (MainGameMode != null) 
        {
            Destroy(gameObject);
        }
        MainGameMode = this;
        BattleManager = new BattleManager();

        PlayerStart playerStart = FindFirstObjectByType<PlayerStart>();
        if (!playerStart) { throw new System.Exception("PlayerStart need"); }
        mPlayerGameObject = Instantiate(mPlayerGameObjectPrefab, playerStart.transform.position, playerStart.transform.rotation);

    }
}
