
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public interface IAnimatable
{
    List<Tween> Tweens{ get; }
    IEnumerator Move(Vector3 targetPosition, AnimationSettings settings);
    IEnumerator Shake(Vector3 strength, ShakeSettings settings);
    IEnumerator PreviewClear(); 
    IEnumerator PreviewHit(); 
    IEnumerator Clear(AnimationSettings settings); 

    IEnumerator Idle(); 

    IEnumerator Hit(AnimationSettings settings); 
}

public class DotsAnimationComponent : MonoBehaviour, IAnimatable
{
    public List<Tween> Tweens => new();
    private DotsGameObject dotsGameObject;
    public DotsGameObject DotsGameObject{
        get{
            if(dotsGameObject == null){
                if(TryGetComponent(out DotsGameObject dotsGameObject)){
                    this.dotsGameObject = dotsGameObject;
                }
                else{
                    this.dotsGameObject = GetComponentInParent<DotsGameObject>();
                }
            }
            return dotsGameObject;
        }
    }

    private void OnDestroy(){
        StopAllCoroutines();
    }
    public virtual IEnumerator Hit(AnimationSettings settings)
    {
         return null;
    }
    
    public virtual IEnumerator Clear(AnimationSettings settings)
    {
        Tween tween = transform.DOScale(Vector2.zero, settings.Duration);
        Tweens.Add(tween);
        yield return tween.WaitForCompletion();
    }

    public virtual IEnumerator Move(Vector3 targetPosition, AnimationSettings settings)
    {
        
        Tween tween = transform.DOMove(targetPosition, settings.Duration)
        .SetEase(settings.Ease);
        Tweens.Add(tween);
        yield return tween.WaitForCompletion();
        
    }
    public virtual IEnumerator Shake(Vector3 strength, ShakeSettings settings)
    {
        float duration = settings.Duration;
        int vibrato = settings.Vibrato; 
        float randomness = settings.Randomness; 
        bool snapping = settings.Snapping; 
        bool fadeOut = settings.FadeOut; 
        Ease ease = settings.Ease; 
        ShakeRandomnessMode shakeRandomnessMode = settings.ShakeRandomnessMode;
        Tween tween = transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut, shakeRandomnessMode)
        .SetEase(ease, amplitude: settings.Amplitude, period: settings.Period);
        Tweens.Add(tween);
        yield return tween.WaitForCompletion();
        
    }
    
    public virtual IEnumerator PreviewClear()
    {
        return null;
    }

    public virtual IEnumerator PreviewHit()
    {
        return null;
    }

    public virtual IEnumerator Idle()
    {
        return null;
    }

    public IEnumerator Swap(Vector3 targetPosition, AnimationSettings settings)
    {
        Tween tween = transform.DOMove(targetPosition, settings.Duration)
        .SetEase(settings.Ease);
        yield return tween.WaitForCompletion();
    }

    public IEnumerator Drop(float yValue, AnimationSettings settings)
    {
        Tween tween = transform.DOMoveY(yValue, settings.Duration)
        .SetEase(settings.Ease, settings.Amplitude, settings.Period);
        
        yield return tween.WaitForCompletion();
    }
}


