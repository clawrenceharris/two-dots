using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Visuals : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer[] Sprites;

    public class ShakeAnimationSettings{
        public float duration = 1;
        
        public Vector3 strength = new Vector3(0.1f, 0.1f, 0); 
        public int vibrato = 10; //number of shakes
        public float randomness = 20; 
        public bool fadeOut = true;
        public bool snapping = false;
        public ShakeRandomnessMode shakeRandomnessMode = ShakeRandomnessMode.Harmonic;
        public Ease ease= Ease.Linear;
    }
    public IEnumerator DoShakeAnimation(DotsGameObject dotsGameObject, ShakeAnimationSettings settings)
    {
       
        float duration = settings.duration;
        Vector3 strength = settings.strength; 
        int vibrato =settings.vibrato; //number of shakes
        float randomness = settings.randomness; 
        bool snapping = settings.snapping; 
        bool fadeOut = settings.fadeOut; 
        Ease ease = settings.ease; 
        ShakeRandomnessMode shakeRandomnessMode = settings.shakeRandomnessMode;
        dotsGameObject.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut, shakeRandomnessMode)
        .SetEase(ease);
        
        yield return new WaitForSeconds(duration);
       
    }
}
