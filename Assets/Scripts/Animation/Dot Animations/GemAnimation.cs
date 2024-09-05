using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GemAnimation : DotsAnimationComponent{
    public override IEnumerator PreviewClear()
    {
        Vector2 strength = new(0.02f, 0.02f);
        ShakeSettings settings = new(){
                Vibrato = 10,
                Duration = 2f,
                Ease = Ease.OutQuad,
                FadeOut = false
                     
        };
        
        yield return Shake(strength, settings);
    }
}