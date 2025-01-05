using System.Collections;
using System.Collections.Generic;
using Animations;
using DG.Tweening;
using UnityEngine;
using System.Linq;


/// <summary>
/// Represents an animation of a dot
/// </summary>
public interface IAnimation{
    
    IEnumerator Animate(IAnimatable animatable);
}


public class MoveAnimation : IAnimation{
    public List<Vector3> Target {get; set;}
    public AnimationSettings Settings {get; set;}
    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IMovable>()?.Animate(this);
    }
}

public class DropAnimation : IAnimation
{
    public float Target {get; set;}
    public AnimationSettings Settings => new(){
        Ease = Ease.OutBounce,
        Duration = 0.3f,
        Amplitude = 2,
        Period = 0.8f
    };
    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IDroppable>()?.Animate(this);
    }
}
public class SwapAnimation : IAnimation{
    public Vector3 Target {get; set;}

    public AnimationSettings Settings => new(){
        Ease = Ease.OutQuad,
        Period = 0.8f,
        Duration = 0.5f
    };

    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<Animations.ISwappable>().Animate(this);
    }

}
public class RotateAnimation : IAnimation{
    public AnimationSettings Settings {get; set;}
    public Vector3 Target {get; set;}

    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IRotatable>()?.Animate(this);
    }
}
public class ShakeAnimation : IAnimation{
    public ShakeSettings Settings {get; set;}

    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IShakable>()?.Animate(this);
    }
}

public class PreviewClearAnimation : IAnimation{
    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IClearPreviewable>()?.Animate(this);
    }
}
public class ClearAnimation : IAnimation{

    public AnimationSettings Settings {get; set;}
    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IClearable>()?.Animate(this);
    }
}

public class HitAnimation : IAnimation{
    public AnimationSettings Settings {get; set;}
    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<Animations.IHittable>()?.Animate(this);
    }
}

public class PreviewHitAnimation : IAnimation{
    
    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IHitPreviewable>()?.Animate(this);
    }
}

public class IdleAnimation : IAnimation{
    
    public IEnumerator Animate(IAnimatable animatable)
    {
        yield return animatable.OfType<IIdlable>()?.Animate();
    }
}