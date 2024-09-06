using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface IAnimation{
    
    IEnumerator Animate(DotsAnimationComponent animatable);
}

public class MoveAnimation : IAnimation{
    public Vector3 Target {get; set;}
    public AnimationSettings Settings {get; set;}

    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        yield return animatable.Move(Target, Settings);
    }
}

public class DropAnimation : IAnimation
{
    public float Target {get; set;}
    private AnimationSettings settings;
    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        settings = new(){
            Duration = Board.DotDropSpeed,
            Ease = Ease.OutBounce,
            Amplitude = 1,
            Period = 0.8f
        };
        yield return animatable.Drop(Target, settings);
    }
}
public class SwapAnimation : IAnimation{
    public Vector3 Target {get; set;}
    private AnimationSettings settings;
    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        settings = new(){
            Duration = 0.7f,
            Ease = Ease.OutQuad
        };
        yield return animatable.Swap(Target, settings);
    }

}
public class ShakeAnimation : IAnimation{
    public Vector3 Target {get; set;}
    public ShakeSettings Settings {get; set;}

    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        yield return animatable.Shake(Target, Settings);
    }
}

public class PreviewClearAnimation : IAnimation{
    
    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        yield return animatable.PreviewClear();
    }
}
public class ClearAnimation : IAnimation{
    public AnimationSettings Settings {get; set;}
    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        yield return animatable.Clear(Settings);
    }
}

public class HitAnimation : IAnimation{
    public AnimationSettings Settings {get; set;}
    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        yield return animatable.Hit(Settings);
    }
}

public class PreviewHitAnimation : IAnimation{
    
    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        yield return animatable.PreviewHit();
    }
}

public class IdleAnimation : IAnimation{
    
    public IEnumerator Animate(DotsAnimationComponent animatable)
    {
        yield return animatable.Idle();
    }
}