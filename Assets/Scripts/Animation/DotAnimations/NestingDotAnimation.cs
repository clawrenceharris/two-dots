using System.Collections;
using DG.Tweening;
using UnityEngine;
using Animations;
public class NestingDotAnimation : DotAnimation,
Animations.IHittable,
IClearPreviewable,
IShakable
{
    
    [SerializeField]private AnimationSettings hitSettings;

    [SerializeField]private ShakeSettings shakeSettings;
    ShakeSettings IShakable.Settings => shakeSettings;
    AnimationSettings Animations.IHittable.Settings => hitSettings;
    public IEnumerator Move(Vector3 targetPosition, IAnimationSettings settings)
    {

        Tween tween = transform.DOMove(targetPosition, settings.Duration)
        .SetEase(settings.Curve);
        yield return tween.WaitForCompletion();
    }
    
    IEnumerator Animations.IHittable.Animate(HitAnimation animation)
    {
        return null;
    }
    
    IEnumerator IClearPreviewable.Animate(PreviewClearAnimation animation)
    {
        float intervalDuration = 0;
        IShakable shakable = this;
        Color initialColor = GetVisuals<NestingDotVisuals>().spriteRenderer.color;
        // var settings = new ShakeSettings{
        //     Duration = intervalDuration,
        //     Vibrato = 10,
        //     Randomness = 20,
        //     FadeOut = true
        // };

        StartCoroutine(shakable.Animate(new ShakeAnimation{
            Settings = shakeSettings
        }));

        GetVisuals<NestingDotVisuals>().spriteRenderer.DOColor(Color.black, intervalDuration);
        yield return new WaitForSeconds(intervalDuration);
        GetVisuals<NestingDotVisuals>().spriteRenderer.DOColor(initialColor, intervalDuration);
        yield return new WaitForSeconds(Random.Range(2, 5));
    }

    IEnumerator IShakable.Animate(ShakeAnimation animation)
    {
        animation.Settings = shakeSettings;
        yield return GetAnimatable<ShakableBase>().Animate(animation);

    }

}