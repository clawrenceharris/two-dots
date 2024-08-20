using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MonsterDotVisualController : ColorableDotVisualController, INumerableVisualController, IDirectionalVisualController, IPreviewableVisualController
{
    private MonsterDot dot;
    private MonsterDotVisuals visuals;
    private readonly DirectionalVisualController directionalVisualController = new();
    private readonly NumerableVisualController numerableVisualController = new();
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (MonsterDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<MonsterDotVisuals>();
        directionalVisualController.Init(dot, visuals.DirectionalVisuals);
        numerableVisualController.Init(dot, visuals.NumerableVisuals);
        base.Init(dotsGameObject);
        ConnectionManager.onDotConnected += OnConnectionChanged;
        ConnectionManager.onDotDisconnected += OnConnectionChanged;
    }

    

    public void UpdateNumbers(int amount)
    {
         numerableVisualController.UpdateNumbers(amount);
    }

    
    private void OnConnectionChanged(ConnectableDot _){
       
       // numerableVisualController.UpdateNumberByConnectionCount(dot);
    }

    public override IEnumerator Hit(HitType hitType)
    {
        numerableVisualController.Hit(hitType);
        yield return base.Hit(hitType);
    }

    public IEnumerator DoMove(int col, int row)
    {
        for(int i = 0; i < visuals.AllSprites.Length; i++)
        {
            visuals.AllSprites[i].sortingOrder += 100;

        }
        yield return dot.transform.DOMove(new Vector2(col, row) * Board.offset, MonsterDotVisuals.MoveDuration);

        yield return new WaitForSeconds(0.8f);
        
    }

    
    public IEnumerator DoRotateAnimation()
    {
        //do nothing; no rotation animation needed
        yield break;
    }

    public override void SetInitialColor()
    {   
        Color color = ColorSchemeManager.FromDotColor(dot.Color);
        visuals.spriteRenderer.color =  color;
        visuals.EyeLids.color = color;

    }

    public IEnumerator DoClearPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoHitPreviewAnimation()
    {
        yield break;
    }

    private IEnumerator DoBlinkingAnimation(){
        float[] durations = {1f, 3f};
        float intervalDuration = durations[Random.Range(0, durations.Length)];
        Sprite[] frames = visuals.BlinkAnimationFrames;
        for(int i = 0; i < 2; i++){
            foreach(Sprite frame in frames){
                visuals.EyeLids.sprite = frame;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(intervalDuration);
        }
        
    }

    private IEnumerator DoRightEyeAnimation(){
        yield return new WaitForSeconds(0.8f);
        Sprite[] rightEyeFrames = visuals.RightEyeAnimationFrames;
        foreach(Sprite frame in rightEyeFrames){
            visuals.RightEye.sprite = frame;
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    private IEnumerator DoLeftEyeAnimation(){
        yield return new WaitForSeconds(0.8f);
        Sprite[] leftEyeFrames = visuals.LeftEyeAnimationFrames;
        foreach(Sprite frame in leftEyeFrames){
            visuals.LeftEye.sprite = frame;
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    public IEnumerator DoIdleAnimation()
    {
        IEnumerator[] coroutines = {DoEyeAnimation(), DoBlinkingAnimation() };
        int rand = Random.Range(0, coroutines.Length);

        yield return coroutines[rand];
        yield return new WaitForSeconds(6);


    }

    private IEnumerator DoEyeAnimation()
    {
        CoroutineHandler.StartStaticCoroutine(DoLeftEyeAnimation());
        yield return CoroutineHandler.StartStaticCoroutine(DoRightEyeAnimation());
    }

    public void UpdateRotation()
    {
        directionalVisualController.UpdateRotation();
    }

    public IEnumerator ScaleNumbers()
    {
       yield return numerableVisualController.ScaleNumbers();
    }
}
