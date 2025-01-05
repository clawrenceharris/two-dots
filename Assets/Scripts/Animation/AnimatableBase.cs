using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animations;
using DG.Tweening;
using UnityEngine;
namespace Animations{


    public abstract class AnimatableBase : IAnimatable{
        protected AnimatableComponent animatable;

        public void Init(AnimatableComponent animatable){
            this.animatable = animatable;
        }
    }

    public class MovableBase : AnimatableBase{
        

        public IEnumerator Animate(MoveAnimation animation){
            List<Vector3> positions = animation.Target;
            foreach(Vector3 pos in positions){
                if(pos == positions.Last()){
                    Tween tween = animatable.transform.DOMove(pos, animation.Settings.Duration / positions.Count)
                    .SetEase(animation.Settings.Ease);
                    yield return tween.WaitForCompletion();

                }
                else{
                    Tween tween = animatable.transform.DOMove(pos, animation.Settings.Duration / positions.Count)
                    .SetEase(Ease.Linear);
                    yield return tween.WaitForCompletion();

                }
                
            }
        }

    
    }

    public class ClearableBase : AnimatableBase{

        public IEnumerator Animate(ClearAnimation animation){
            var settings = animation.Settings;
            Tween tween = animatable.transform.DOScale(Vector3.zero, animation.Settings.Duration)
            .SetEase(animation.Settings.Ease);
            yield return tween.WaitForCompletion();
        }

        
    }
    public class ShakableBase : AnimatableBase{
        

        public IEnumerator Animate(ShakeAnimation animation)
        {
            
            float duration = animation.Settings.Duration;
            int vibrato = animation.Settings.Vibrato; 
            float randomness = animation.Settings.Randomness; 
            bool snapping = animation.Settings.Snapping; 
            bool fadeOut = animation.Settings.FadeOut; 
            Ease ease = animation.Settings.Ease; 
            ShakeRandomnessMode shakeRandomnessMode = animation.Settings.ShakeRandomnessMode;
            Tween tween = animatable.transform.DOShakePosition(duration, animation.Settings.Strength, vibrato, randomness, snapping, fadeOut, shakeRandomnessMode)
            .SetEase(ease, amplitude: animation.Settings.Amplitude, period: animation.Settings.Period);
            yield return tween?.WaitForCompletion();
            
        
        }
    }

    public class SwappableBase : AnimatableBase{

        public IEnumerator Animate(SwapAnimation animation)
        {
        
            Tween tween = animatable.transform.DOMove(animation.Target, animation.Settings.Duration)
            .SetEase(animation.Settings.Ease);
            yield return tween.WaitForCompletion();
            
        
        } 
    }

    public class DroppableBase : AnimatableBase{

        public IEnumerator Animate(DropAnimation animation)
        {
        
            Tween tween = animatable.transform.DOMoveY(animation.Target, animation.Settings.Duration)
            .SetEase(animation.Settings.Ease,animation.Settings.Amplitude, animation.Settings.Period);
            yield return tween.WaitForCompletion();
            
        
        } 
    }


    public class RotatableBase : AnimatableBase{

        public IEnumerator Animate(RotateAnimation animation)
        {
        
            Tween tween = animatable.transform.DORotate(animation.Target, animation.Settings.Duration)
            .SetEase(animation.Settings.Ease);
            yield return tween.WaitForCompletion();
            
        
        } 
    }
}