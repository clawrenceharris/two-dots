 using System.Collections;
using System.ComponentModel;
using Animations;
using UnityEngine;
/// <summary>
/// Represents an Animatable object that can be animated with animation strategies 
/// that make up the basis of most dots
/// </summary>
public class DotAnimation : AnimatableComponent,
Animations.ISwappable,
IDroppable,
IClearable

{
    [SerializeField]private AnimationSettings clearSettings;
    AnimationSettings IClearable.Settings => clearSettings;
    
    IEnumerator Animations.ISwappable.Animate(SwapAnimation animation)
    {
        
        yield return GetAnimatable<Animations.SwappableBase>().Animate(animation);
    }
    protected virtual IEnumerator Clear(ClearAnimation animation){
        animation.Settings = clearSettings;
        yield return GetAnimatable<ClearableBase>().Animate(animation);
    }
    IEnumerator IDroppable.Animate(DropAnimation animation)
    {
        
        yield return GetAnimatable<DroppableBase>().Animate(animation);
    }

    IEnumerator IClearable.Animate(ClearAnimation animation)
    {
        yield return Clear(animation);
    }
}