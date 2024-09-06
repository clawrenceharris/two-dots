using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


/// <summary>
/// Handles the animation of dots by managing different animation layers and running animations.
/// </summary>
public class DotsAnimator : MonoBehaviour
{
    /// <summary>
    /// The list of animation layers available for animation.
    /// </summary>
    [SerializeField] private List<AnimationLayer> layers;

    /// <summary>
    /// Indicates whether an animation is currently in progress.
    /// </summary>
    public bool IsAnimating { get; private set; }

    /// <summary>
    /// Represents a single animation layer and its associated animatable component.
    /// </summary>
    [Serializable]
    public class AnimationLayer
    {
        /// <summary>
        /// The animation layer being represented.
        /// </summary>
        public global::AnimationLayer Layer;
        
        /// <summary>
        /// The animation to be played
        /// </summary>
        public IEnumerator Animation;
        /// <summary>
        /// The component that can be animated within this layer.
        /// </summary>
        public DotsAnimationComponent Animatable;
    }

    /// <summary>
    /// Starts an animation on the specified layer.
    /// </summary>
    /// <param name="newAnimation">The animation to be played.</param>
    /// <param name="layerName">The name of the animation layer to use. Defaults to <see cref="global::AnimationLayer.BaseLayer"/>.</param>
    /// <returns>An IEnumerator that can be used to wait for the animation to complete.</returns>
    public IEnumerator Animate(IAnimation newAnimation, global::AnimationLayer layerName = global::AnimationLayer.BaseLayer)
    {
        if (layers.Count == 0)
        {
            yield break;
        }

        AnimationLayer layer = layers.Find(layer => layer.Layer == layerName);

        if (layer == null)
        {
            Debug.LogWarning($"No layer found for {layerName}");
            yield break;
        }
        if(layer.Animation != null){
            StopCoroutine(layer.Animation);
        }
        IsAnimating = true;
        layer.Animation = newAnimation.Animate(layer.Animatable);
        yield return layer.Animation;
        layer.Animation = null;
        IsAnimating = false;
    }

    /// <summary>
    /// Kills all active tweens and stops all ongoing coroutines 
    /// </summary>
    public void StopAnimations()
    {
        
        foreach(AnimationLayer layer in layers){
            foreach(Tween tween in layer.Animatable.Tweens){
                tween.Kill();
            }
        }
    }

    public T GetAnimationComponent<T>(global::AnimationLayer layer)
    where T : DotsAnimationComponent
    {
        return layers.Find(item => item.Layer == layer).Animatable as T;
    }
}

