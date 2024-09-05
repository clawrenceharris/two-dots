using System.Collections;
using UnityEngine;

public class MonsterDotAnimation : DotsAnimationComponent{
    
    [SerializeField]private Sprite[] LeftEyeAnimation;
    [SerializeField]private Sprite[] RightEyeAnimation;
    [SerializeField]private Sprite[] BlinkAnimation;

    private MonsterDotVisuals Visuals => DotsGameObject.VisualController.GetVisuals<MonsterDotVisuals>();
    public override IEnumerator Idle()
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
                Visuals.EyeLids.sprite = frame;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(intervalDuration);
        }
        
    }

    private IEnumerator DoRightEyeAnimation(){
        yield return new WaitForSeconds(0.8f);
        Sprite[] rightEyeFrames = RightEyeAnimation;
        foreach(Sprite frame in rightEyeFrames){
            Visuals.RightEye.sprite = frame;
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    private IEnumerator DoLeftEyeAnimation(){
        yield return new WaitForSeconds(0.8f);
        Sprite[] leftEyeFrames = LeftEyeAnimation;
        foreach(Sprite frame in leftEyeFrames){
            Visuals.LeftEye.sprite = frame;
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    private IEnumerator DoEyeAnimation()
    {
        CoroutineHandler.StartStaticCoroutine(DoLeftEyeAnimation());
        yield return CoroutineHandler.StartStaticCoroutine(DoRightEyeAnimation());
    }

}