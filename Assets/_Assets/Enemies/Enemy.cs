using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject Target 
    {
        get { return Target; }
        set { Target = value; }
    }
    [SerializeField] float mEyeHeight = 1.5f;
    [SerializeField] float mSightDistance = 5f;
    [SerializeField] float mViewAngle = 30f;
    void Update()
    {
        UpdatePlayerPrerception();
    }
    void UpdatePlayerPrerception() 
    {
        Player player = GameMode.MainGameMode.mPlayer;
        if (!player)
        {
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) > mSightDistance) 
        {
            Target = null;
            return;
        }
        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        if (Vector3.Angle(playerDir, transform.forward) > mViewAngle) 
        {
            Target = null;
            return;
        }
        Vector3 eyeViewPoint = transform.position + Vector3.up * mEyeHeight;
        if (Physics.Raycast(eyeViewPoint, playerDir, out RaycastHit hitInfo, mSightDistance)) 
        {
            if (hitInfo.collider.gameObject != player) 
            {
                Target = null;
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 eyeviewPoint = transform.position + Vector3.up * mEyeHeight;
        Gizmos.DrawWireSphere(eyeviewPoint, mSightDistance);

        Vector3 leftLineDir = Quaternion.AngleAxis(mViewAngle, Vector3.up) * transform.forward;
        Vector3 rightLineDir = Quaternion.AngleAxis(-mViewAngle, Vector3.up) * transform.forward;
        Gizmos.DrawLine(eyeviewPoint, eyeviewPoint + leftLineDir * mSightDistance);
        Gizmos.DrawLine(eyeviewPoint, eyeviewPoint + rightLineDir * mSightDistance);
    }
}
