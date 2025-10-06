using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] float mHeightOffset = 0.5f;
    [SerializeField] float mFollowLearpRate = 20f;
    [SerializeField] Transform mYawTransform;
    [SerializeField] Transform mPitchTransform;
    [SerializeField] float mRotationRate;
    Transform mFollowTransform;

    [SerializeField] float mPitchMin = -89f;
    [SerializeField] float mPitchMax = 89f;

    Vector2 mLookInput;

    float mPitch;
    public void SetLookInput(Vector2 lookInput) 
    {
        mLookInput = lookInput;
    }
    public void SetFollowTransform(Transform followTransform)
    {
        mFollowTransform = followTransform;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, mFollowTransform.position + mHeightOffset * Vector3.up, mFollowLearpRate * Time.deltaTime);
        mYawTransform.rotation *= Quaternion.AngleAxis(mLookInput.x * mRotationRate * Time.deltaTime, Vector3.up);

        mPitch = mPitch + mRotationRate * Time.deltaTime * mLookInput.y;
        mPitch = Mathf.Clamp(mPitch, mPitchMin, mPitchMax);
        mPitchTransform.localEulerAngles = new Vector3(mPitch, 0f, 0f);
    }
}
