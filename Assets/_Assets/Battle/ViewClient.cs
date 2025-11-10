using UnityEngine;

public interface IViewClient
{
    public void PushViewTarget(Transform viewTarget);

    public void PopViewTarget(Transform viewtarget);
    public void ResetViewAngle();
}
