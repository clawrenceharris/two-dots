using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClockDotAnimation : DotsAnimationComponent
{
    private ClockDot Dot => (ClockDot)DotsGameObject;
    public override IEnumerator Move(Vector3 targetPosition, AnimationSettings settings)
    {
       
        

        Tween tween = transform.DOMove(targetPosition, settings.Duration)
        .SetEase(settings.Curve);
        Tweens.Add(tween);
        yield return tween.WaitForCompletion();
    }

    public override IEnumerator Idle()
    {
       float elapsedTime = 0f;
        Vector3 originalRotation = Dot.transform.eulerAngles;
        float duration = 1f;
        float shakeIntensity = 15f;
        float shakeSpeed = 10f;
        while (elapsedTime < duration)
        {
            // Calculate the amount to rotate by interpolating between -shakeIntensity and shakeIntensity
            float shakeAmount = Mathf.Sin(elapsedTime * shakeSpeed) * shakeIntensity;

            // Apply the rotation
            Dot.transform.eulerAngles = originalRotation + new Vector3(0, 0, shakeAmount);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset rotation to original position after the shaking animation is finished
        Dot.transform.DORotate(Vector3.zero, duration /2);
        yield return new WaitForSeconds(duration /2);
        yield return new WaitForSeconds(UnityEngine.Random.Range(6, 10));
    }
}
