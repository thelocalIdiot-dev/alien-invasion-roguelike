using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public Camera Camera;

    [Header("fields of view")]
    public float baseFOV;
    public float runingFOV;
    public float slidingFOV;
    public float dashingFOV;
    public float WallrunningFOV;

    [Header("tilt values")]
    public float strafTilt;
    public float slidingTilt;
    public float WallrunningTilt;
    
    public static CameraEffects instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    
    public void changeFOV(float endValue, float duration)
    {
        Camera.DOFieldOfView(endValue, duration);
    }
    
    public void tilt(float endValue, float duration)
    {
        Vector3 rotateValue = new Vector3(0, 0, endValue);
        transform.DOLocalRotate(rotateValue, duration);
    }

    public void punch(Vector3 punchPower, float punchDuration, string punchType)
    {
        if(punchType == "position")
        {
            transform.DOPunchPosition(punchPower, punchDuration, 1, 0.5f);
        }

        if (punchType == "rotation")
        {
            transform.DOPunchRotation(punchPower, punchDuration);
        }

        if (punchType == "scale")
        {
            transform.DOPunchScale(punchPower, punchDuration);
        }
    }

}
