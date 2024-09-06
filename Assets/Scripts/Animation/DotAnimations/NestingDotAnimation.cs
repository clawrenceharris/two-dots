using System.Collections;
using DG.Tweening;
using UnityEngine;

public class NestingDotAnimation : DotsAnimationComponent{
    
    private NestingDotVisuals Visuals => DotsGameObject.VisualController.GetVisuals<NestingDotVisuals>();
    
    public override IEnumerator PreviewClear()
    {
        Vector2 strength = Vector2.one * 0.1f; 
        float duration = 0.8f;
        var settings = new ShakeSettings{
            Duration = duration,
            Vibrato = 10,
            Randomness = 20,
            FadeOut = true
        };
        Color initialColor = Visuals.spriteRenderer.color;
        StartCoroutine(Shake(strength, settings));

        Visuals.spriteRenderer.DOColor(Color.black, duration);
        yield return new WaitForSeconds(duration);
        Visuals.spriteRenderer.DOColor(initialColor, duration);
        yield return new WaitForSeconds(Random.Range(2, 5));
    }

    
}