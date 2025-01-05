
using System.Collections;
using System.Collections.Generic;
using Animations;
using DG.Tweening;
using UnityEngine;

public class BeetleDotAnimation : DotAnimation,
IIdlable, 
IClearPreviewable,
IHitPreviewable,
IRotatable,
IShakable,
Animations.IHittable

{
    
    private BeetleDot Dot => GetGameObject<BeetleDot>();
    private BeetleDotVisuals Visuals => GetVisuals<BeetleDotVisuals>();

    [SerializeField]private ShakeSettings shakeSettings;
    [SerializeField]private AnimationSettings rotateSettings;
    [SerializeField]private AnimationSettings hitSettings;

    AnimationSettings Animations.IHittable.Settings => hitSettings;
    AnimationSettings IRotatable.Settings => rotateSettings;

    ShakeSettings IShakable.Settings => shakeSettings;

   
    IEnumerator IClearPreviewable.Animate(PreviewClearAnimation animation)
    {
        
        yield return GetAnimatable<ShakableBase>().Animate(new ShakeAnimation{
            Settings = shakeSettings
        });
        
    }

    IEnumerator IHitPreviewable.Animate(PreviewHitAnimation a)
    {
        
        float startFlapAngle = 45f;
        float endFlapAngle = 0f;
        float duration = 0.26f;
       
        yield return FlapWings(duration,1, startFlapAngle, endFlapAngle);
        
    }

    IEnumerator IIdlable.Animate()
    {
        yield return new WaitForSeconds(Random.Range(4, PreviewableStateManager.Count<BeetleDot>() * 6));
        IEnumerator[] coroutines = {DoFlutterAnimation(), DoWiggleAnimation(), DoFlutterAndWiggleAnimation()};
        int rand = Random.Range(0, coroutines.Length);
        yield return coroutines[rand];
    }
    private IEnumerator FlapWings(float duration, float startAngle, float endAngle)
    {
        Vector3 leftWingStartAngle = new(0, 0, -startAngle);
        Vector3 rightWingStartAngle = new(0, 0, startAngle);
        Vector3 leftWingEndAngle = new(0, 0, -endAngle);
        Vector3 rightWingEndAngle = new(0, 0, endAngle);
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return StartCoroutine(FlapWings(0.26f,1, startAngle, endAngle));
            
            // Flap up
            // Visuals.ActiveLeftWings.DOLocalRotate(leftWingStartAngle, duration / 2)
            // .SetEase(ease);
            // Visuals.ActiveRightWings.DOLocalRotate(rightWingStartAngle, duration / 2)
            // .SetEase(ease);

            // yield return new WaitForSeconds(duration / 2);

            // // Flap down
            // Visuals.ActiveLeftWings.DOLocalRotate(leftWingEndAngle, duration / 2)
            // .SetEase(ease);
            // Visuals.ActiveRightWings.DOLocalRotate(rightWingEndAngle, duration / 2)
            // .SetEase(ease);

            // yield return new WaitForSeconds(duration / 2);

            // flapsCompleted++;
        }
        Visuals.ActiveLeftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.ActiveRightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
        
         
    private IEnumerator FlapWings(float duration, int numFlaps, float startAngle, float endAngle)
    {
        Vector3 leftWingStartAngle = new(0, 0, -startAngle);
        Vector3 rightWingStartAngle = new(0, 0, startAngle);
        Vector3 leftWingEndAngle = new(0, 0, -endAngle);
        Vector3 rightWingEndAngle = new(0, 0, endAngle);
        Ease ease = Ease.OutCirc;
        int flapsCompleted = 0;

        while (flapsCompleted < numFlaps)
        {
            // Flap up
            Visuals.ActiveLeftWings.DOLocalRotate(leftWingStartAngle, duration / 2)
            .SetEase(ease);
            Visuals.ActiveRightWings.DOLocalRotate(rightWingStartAngle, duration / 2)
            .SetEase(ease);

            yield return new WaitForSeconds(duration / 2);

            // Flap down
            Visuals.ActiveLeftWings.DOLocalRotate(leftWingEndAngle, duration / 2)
            .SetEase(ease);
            Visuals.ActiveRightWings.DOLocalRotate(rightWingEndAngle, duration / 2)
            .SetEase(ease);

            yield return new WaitForSeconds(duration / 2);

            flapsCompleted++;
        }
       
    }
        
    

    private IEnumerator DoWiggleAnimation(){
        int wiggleCount = Random.Range(2, 4);
        float[] angles = {2f, 5f, 10f};
        float wiggleIntensity = angles[Random.Range(0, angles.Length)];
        float[] durations = {0.2f, 0.5f, 0.7f};
        float[] speeds = {5f, 8f, 10f};
        float wiggleSpeed = speeds[Random.Range(0, speeds.Length)];
        float wiggleDuration = durations[Random.Range(0, durations.Length)];
        Vector3 originalRotation = transform.eulerAngles;


        // Create the wiggle sequence
        Sequence wiggleSequence = DOTween.Sequence()
            .Append(transform.DORotate(originalRotation + new Vector3(0, 0, wiggleIntensity), 1f / wiggleSpeed)
                .SetEase(Ease.InOutSine)) // Move in one direction
            .Append(transform.DORotate(originalRotation - new Vector3(0, 0, wiggleIntensity), 1f / wiggleSpeed)
                .SetEase(Ease.InOutSine)) // Move in the other direction
            .SetLoops(Mathf.FloorToInt(wiggleDuration * wiggleSpeed), LoopType.Yoyo) // Loop back and forth
            .OnComplete(() => transform.DORotate(originalRotation, 0.5f)); // Return to original rotation

        // Start the sequence
        wiggleSequence.Play();
        yield return new WaitForSeconds(wiggleDuration);
    }
    
    public IEnumerator DoFlutterAnimation(){
        float startFlapAngle = Random.Range(10,45);
        float endFlapAngle = 0f;
        int flapCount = Random.Range(1,2);
        float flapDuration = Random.Range(0.5f,0.9f);
       

        yield return FlapWings(flapDuration, flapCount, startFlapAngle, endFlapAngle);
        
    }

    private IEnumerator DoFlutterAndWiggleAnimation(){
        CoroutineHandler.StartStaticCoroutine(DoFlutterAnimation());
        CoroutineHandler.StartStaticCoroutine(DoWiggleAnimation());

        yield return new WaitForSeconds(2);
    }
     
    


  
    IEnumerator Animations.IHittable.Animate(HitAnimation animation)
    {
        Visuals.ActiveLeftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.ActiveRightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        animation.Settings = hitSettings;
        float duration = hitSettings.Duration;
        float rotationDuration = duration / 2;
        BeetleDotVisuals.WingLayer wingLayer = Visuals.WingLayers[^1];
     
        Vector3 leftWingAngle = new(0, 0, -90);
        Vector3 rightWingAngle = new(0, 0, 90);

        
        wingLayer.LeftWing.transform.DOLocalRotate(Dot.transform.rotation.eulerAngles + leftWingAngle, rotationDuration);
        wingLayer.RightWing.transform.DOLocalRotate(Dot.transform.rotation.eulerAngles + rightWingAngle, rotationDuration);
        
        wingLayer.RightWing.transform.DOMove(Dot.transform.position + new Vector3(Dot.DirectionY, -Dot.DirectionX) * 1.7f, duration);
        wingLayer.LeftWing.transform.DOMove(Dot.transform.position + new Vector3(-Dot.DirectionY, Dot.DirectionX)  * 1.7f, duration);

        wingLayer.RightWing.transform.DOScale(Vector2.zero, duration);
        wingLayer.LeftWing.transform.DOScale(Vector2.zero, duration);


        wingLayer.RightWing.DOFade(0, duration);
        wingLayer.LeftWing.DOFade(0, duration);
        yield return new WaitForSeconds(duration);
        wingLayer.RightWing.transform.rotation = Quaternion.Euler(Vector3.zero);
        wingLayer.LeftWing.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    protected override IEnumerator Clear(ClearAnimation animation)
    {
        IClearable clearable = this;
        bool isExplosion = Dot.HitType.IsExplosion();
        float startFlapAngle = isExplosion ? 20f : 45f;
        float endFlapAngle = isExplosion ? 15f : 0f;
        float elapsedTime = 0f;
        float amplitude = 1f;
        float duration = clearable.Settings.Duration;
        float frequency = 0.1f;
        float speed = 42f;
        Vector3 direction = new(Dot.DirectionX, Dot.DirectionY);

        Vector3 startPosition = transform.position;
        Vector3 unitDirection = direction.normalized;
        
        Dot.StartCoroutine(GetAnimatable<ClearableBase>().Animate(new ClearAnimation{
            Settings = clearable.Settings
        }));

        Dot.StartCoroutine(FlapWings(duration, startFlapAngle, endFlapAngle));

        while (elapsedTime < duration * speed)
        {
            elapsedTime += Time.deltaTime * speed;

            // Calculate the progress based on elapsed time and duration
            float progress = elapsedTime / duration;

            // Calculate the linear displacement along the direction vector with respect to speed and progress
            Vector3 linearDisplacement = progress * unitDirection;

            // Calculate the perpendicular displacement using the sine function
            Vector3 perpendicularDisplacement = amplitude * Mathf.Sin(progress * frequency * Mathf.PI * 2) * Vector3.Cross(unitDirection, Vector3.forward).normalized;

            Vector3 newPosition = startPosition + linearDisplacement + perpendicularDisplacement;

            // Calculate the direction of the movement
            Vector3 movementDirection = (newPosition - transform.position).normalized;

            // Update the rotation of the dot to face the movement direction
            if (movementDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            }

            transform.position = newPosition;

            yield return null;
        }
    }

    public IEnumerator TryRotate(Vector3 targetRotation)
    {
        return null;
    }

    IEnumerator IShakable.Animate(ShakeAnimation animation)
    {
        animation.Settings = shakeSettings;
        yield return GetAnimatable<ShakableBase>().Animate(animation);
    }
   
    IEnumerator IRotatable.Animate(RotateAnimation animation)
    {
        animation.Settings = rotateSettings;
        yield return GetAnimatable<RotatableBase>().Animate(animation);
    }
}