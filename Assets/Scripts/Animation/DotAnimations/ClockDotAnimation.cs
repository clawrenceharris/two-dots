using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animations;
using DG.Tweening;
using UnityEngine;

public class ClockDotAnimation : DotAnimation, 
IIdlable, 
IClearPreviewable, 
Animations.IHittable

{
    
    [SerializeField]private AnimationSettings moveSettings;
    [SerializeField]private AnimationSettings hitSettings;

    AnimationSettings Animations.IHittable.Settings => hitSettings;

    
    

    IEnumerator Animations.IHittable.Animate(HitAnimation animation)
    {
        float scaleDuration = hitSettings.Duration;

        GetVisuals<ClockDotVisuals>().Digit1.transform.DOScale(Vector2.one * 1.8f, scaleDuration /2);
        GetVisuals<ClockDotVisuals>().Digit2.transform.DOScale(Vector2.one * 1.8f, scaleDuration / 2);

        yield return new WaitForSeconds(scaleDuration / 2);
        GetVisuals<ClockDotVisuals>().Digit1.transform.DOScale(Vector2.one, scaleDuration / 2);
        GetVisuals<ClockDotVisuals>().Digit2.transform.DOScale(Vector2.one, scaleDuration / 2);
    }


    IEnumerator IIdlable.Animate()
    {
       float elapsedTime = 0f;
        Vector3 originalRotation = transform.eulerAngles;
        float duration = 1f;
        float shakeIntensity = 15f;
        float shakeSpeed = 10f;
        while (elapsedTime < duration)
        {
            // Calculate the amount to rotate by interpolating between -shakeIntensity and shakeIntensity
            float shakeAmount = Mathf.Sin(elapsedTime * shakeSpeed) * shakeIntensity;

            // Apply the rotation
            transform.eulerAngles = originalRotation + new Vector3(0, 0, shakeAmount);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset rotation to original position after the shaking animation is finished
        transform.DORotate(Vector3.zero, duration /2);
        yield return new WaitForSeconds(duration /2);
        yield return new WaitForSeconds(Random.Range(6, 10));
    }
   
    IEnumerator IClearPreviewable.Animate(PreviewClearAnimation animation)
    {
        float elapsedTime = 0f;
        Vector3 originalRotation = transform.eulerAngles;
        float shakeDuration = 0.6f;
        float shakeIntensity = 15f;
        float shakeSpeed = 35f;
        while (elapsedTime < shakeDuration)
        {
            // Calculate the amount to rotate by interpolating between -shakeIntensity and shakeIntensity
            float shakeAmount = Mathf.Sin(elapsedTime * shakeSpeed) * shakeIntensity;

            // Apply the rotation
            transform.eulerAngles = originalRotation + new Vector3(0, 0, shakeAmount);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset rotation to original position after the shaking animation is finished
        transform.eulerAngles = Vector2.zero;
    }
    

   
}
