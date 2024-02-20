using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingAnimation
{
    private AnimationCurve _animatinCurve;
    private Transform _transform;

    public JumpingAnimation(Transform transform,AnimationCurve animatinCurve)
    {
        _transform = transform;
        _animatinCurve = animatinCurve;
    }


    public IEnumerator AnimateMoveY(Vector3 origin, Vector3 target, float duration,float height)
    {
        float journey = 0;

        while (journey <= duration)
        {
            journey = journey + Time.deltaTime;

            float percent = Mathf.Clamp01(journey / duration);
            float curvePercent = _animatinCurve.Evaluate(percent);

            Vector3 newTargetPos = target;
            newTargetPos.y += curvePercent * height;

            _transform.localPosition = Vector3.Lerp(origin, newTargetPos, percent);
            yield return null;
        }
    }
}
