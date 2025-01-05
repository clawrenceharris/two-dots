using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Animations
{
    /// <summary>
    /// Represents an object that can be animated
    /// </summary>
    public interface IAnimatable{
       
    }
   
    /// <summary>
    /// Represents an object that can be animated with a clearing action.
    /// </summary>
    public interface IClearable : IAnimatable
    {
        /// <summary>
        /// The animation settings for the clear tween
        /// </summary>
        AnimationSettings Settings {get;}
        /// <summary>
        /// Performs a clearing animation on the object.
        /// </summary>
        /// <param name="settings">The animation settings for the clearing tween.</param>
        IEnumerator Animate(ClearAnimation animation); 
    }

    /// <summary>
    /// Represents an object that can be animated with a hit action.
    /// </summary>
    public interface IHittable : IAnimatable
    {
        /// <summary>
        /// The animation settings for the hit tween
        /// </summary>
        AnimationSettings Settings {get;}
        /// <summary>
        /// Performs a hit animation
        /// <param name="settings">The animation settings for the hit tween.</param>
        /// </summary>
        IEnumerator Animate(HitAnimation animation);
    }

    /// <summary>
    /// Represents an object that can be animated with a shake action.
    /// </summary>
    public interface IShakable : IAnimatable
    {
        /// <summary>
        /// The animation settings for the shake tween
        /// </summary>
        ShakeSettings Settings {get;}
        /// <summary>
        /// Performs a shake animation.
        /// </summary>
        /// <param name="strength">The strength of the shake.</param>
        /// <param name="settings">The shake animation settings for the shake tween.</param>
        IEnumerator Animate(ShakeAnimation animation);
    }

    /// <summary>
    /// Represents an object that can be animated with a movement action.
    /// </summary>
    public interface IMovable : IAnimatable
    {
        /// <summary>
        /// The animation settings for the move tween
        /// </summary>
        AnimationSettings Settings {get;}
        /// <summary>
        /// Performs a movement animation to each the target position.
        /// </summary>
        /// <param name="targetPosition">The target positions to move to.</param>
        /// <param name="settings"The animation settings for the move tween.></param>
        IEnumerator Animate(MoveAnimation animation);
        
    }

    /// <summary>
    /// Represents an object that can be animated with a rotate action.
    /// </summary>
    public interface IRotatable : IAnimatable
    {
        /// <summary>
        /// The animation settings for the rotate tween
        /// </summary>
        AnimationSettings Settings {get;}
        /// <summary>
        /// Performs a rotation animation.
        /// </summary>
        /// <param name="targetRotation">The target rotation.</param>
        /// <param name="settings"The animation settings for the rotate tween.></param>
        IEnumerator Animate(RotateAnimation animation);

    }
    
    /// <summary>
    /// Represents an object that can be animated with a swap action.
    /// </summary>
    public interface ISwappable : IAnimatable
    {
        /// <summary>
        /// Performs a swap animation towards the target position.
        /// </summary>
        /// <param name="targetPosition">The target position of the swap.</param>
        IEnumerator Animate(SwapAnimation animation);
    }
   
   
    /// <summary>
    /// Represents an object that can be animated with an idle sequence.
    /// </summary>
    public interface IIdlable : IAnimatable
    {
        /// <summary>
        /// Performs an idle animation.
        /// </summary>
        IEnumerator Animate();
    }

    /// <summary>
    /// Represents an object that has a hit preview animation.
    /// </summary>
    public interface IHitPreviewable : IAnimatable
    {
        /// <summary>
        /// Performs a hit preview animation.
        /// </summary>
        IEnumerator Animate(PreviewHitAnimation animation);
    }

    /// <summary>
    /// Represents an object that has a clear preview animation.
    /// </summary>
    public interface IClearPreviewable : IAnimatable
    {
        /// <summary>
        /// Performs a clear preview animation.
        /// </summary>
        IEnumerator Animate(PreviewClearAnimation animation);
    }
    

    /// <summary>
    /// Represents an object that can be animated with a drop action.
    /// </summary>
    public interface IDroppable : IAnimatable
    {
        /// <summary>
        /// Performs a drop animation to the target y value.
        /// </summary>
        IEnumerator Animate(DropAnimation animation);
    }


}