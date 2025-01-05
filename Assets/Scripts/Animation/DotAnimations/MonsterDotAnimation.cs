using System.Collections;
using UnityEngine;
using Animations;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
public class MonsterDotAnimation : DotAnimation,
IIdlable,  
IMovable,
Animations.IHittable

{
    
    [SerializeField]private Sprite[] LeftEyeAnimation;
    [SerializeField]private Sprite[] RightEyeAnimation;
    [SerializeField]private Sprite[] BlinkAnimation;

    [SerializeField]private AnimationSettings moveSettings;


    [SerializeField]private AnimationSettings hitSettings;

    AnimationSettings Animations.IHittable.Settings => hitSettings;

    public AnimationSettings Settings => throw new System.NotImplementedException();

    IEnumerator Animations.IHittable.Animate(HitAnimation animation)
    {
        float scaleDuration = 0.2f;

        GetVisuals<MonsterDotVisuals>().Digit1.transform.DOScale(Vector2.one * 1.8f, scaleDuration);
        GetVisuals<MonsterDotVisuals>().Digit2.transform.DOScale(Vector2.one * 1.8f, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);
        GetVisuals<MonsterDotVisuals>().Digit1.transform.DOScale(Vector2.one, scaleDuration);
        GetVisuals<MonsterDotVisuals>().Digit2.transform.DOScale(Vector2.one, scaleDuration);
    }
    
    
    IEnumerator IIdlable.Animate()
    {
        IEnumerator[] coroutines = {DoEyeAnimation(), DoBlinkingAnimation() };
        int rand = Random.Range(0, coroutines.Length);

        yield return coroutines[rand];
        yield return new WaitForSeconds(6);

    }

    private IEnumerator DoBlinkingAnimation(){
        float[] durations = {1f, 3f};
        float intervalDuration = durations[Random.Range(0, durations.Length)];
        Sprite[] frames = BlinkAnimation;
        for(int i = 0; i < 2; i++){
            foreach(Sprite frame in frames){
                GetVisuals<MonsterDotVisuals>().EyeLids.sprite = frame;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(intervalDuration);
        }
        
    }

    private IEnumerator DoRightEyeAnimation(){
        yield return new WaitForSeconds(0.8f);
        Sprite[] rightEyeFrames = RightEyeAnimation;
        foreach(Sprite frame in rightEyeFrames){
            GetVisuals<MonsterDotVisuals>().RightEye.sprite = frame;
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    private IEnumerator DoLeftEyeAnimation(){
        yield return new WaitForSeconds(0.8f);
        Sprite[] leftEyeFrames = LeftEyeAnimation;
        foreach(Sprite frame in leftEyeFrames){
            GetVisuals<MonsterDotVisuals>().LeftEye.sprite = frame;
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    private IEnumerator DoEyeAnimation()
    {
        CoroutineHandler.StartStaticCoroutine(DoLeftEyeAnimation());
        yield return CoroutineHandler.StartStaticCoroutine(DoRightEyeAnimation());
    }

    public IEnumerator Animate(MoveAnimation animation)
    {
        throw new System.NotImplementedException();
    }
}