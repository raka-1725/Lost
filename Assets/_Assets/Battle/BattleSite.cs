using UnityEngine;

public class BattleSite : MonoBehaviour
{
    [SerializeField] float mSiteRadius;
    [SerializeField,Range(0,5)] int mSiteCapacity;//range works as a selector in inspector
    [SerializeField] bool mIsPlayerSite = false;


    public bool IsPlayerSite => mIsPlayerSite;
    //zero index, first unity should have a index of 0
    public Vector3 GetPositionForUnit(int index) 
    {
        if (mSiteCapacity <= 1) 
        {
            return transform.position;
        }

        float gap = (mSiteRadius * 2) / (mSiteCapacity - 1);
        Vector3 startigPoint = transform.position - transform.right * mSiteRadius;

        return startigPoint + index * gap * transform.right;
 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = mIsPlayerSite ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, mSiteRadius);
        for (int i = 0; i < mSiteCapacity; ++i)
        {
            Gizmos.DrawSphere(GetPositionForUnit(i), 0.5f);
        }
    }
}
