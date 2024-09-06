using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeetleDotAnimation : DotsAnimationComponent{
    
    private BeetleDot Dot => (BeetleDot)DotsGameObject;
    private BeetleDotVisuals Visuals => DotsGameObject.VisualController.GetVisuals<BeetleDotVisuals>();

   
    public override IEnumerator PreviewClear()
    {
        Vector2 strength = new(0.02f, 0.02f);
        ShakeSettings settings = new(){
                Vibrato = 10,
                Duration = 2f,
                Ease = Ease.OutQuad,
                FadeOut = false
                     
        };
        yield return Shake(strength, settings);
        
    }

    public override IEnumerator PreviewHit()
    {
        var settings = new AnimationSettings{
            Duration = 0.26f
        };
        float startFlapAngle = 45f;
        float endFlapAngle = 0f;
        yield return FlapWings(settings,startFlapAngle, endFlapAngle);
    }

    public override IEnumerator Idle()
    {
        yield return new WaitForSeconds(Random.Range(4, PreviewableStateManager.Count(Dot) * 6));
        IEnumerator[] coroutines = {DoFlutterAnimation(), DoWiggleAnimation(), DoFlutterAndWiggleAnimation()};
        int rand = Random.Range(0, coroutines.Length);
        yield return coroutines[rand];
    }
    private IEnumerator FlapWings(AnimationSettings settings, float startAngle, float endAngle)
    {
        Vector3 leftWingStartAngle = new(0, 0, -startAngle);
        Vector3 rightWingStartAngle = new(0, 0, startAngle);
        Vector3 leftWingEndAngle = new(0, 0, -endAngle);
        Vector3 rightWingEndAngle = new(0, 0, endAngle);
        
        int loops = settings.Loops;
        Ease ease = settings.Ease;
        float duration = settings.Duration;
        int flapsCompleted = 0;
        bool isInfinite = loops < 0;

        while (isInfinite || flapsCompleted < loops)
        {
            // Flap up
            Tweens.Add(Dot.VisualController.GetVisuals<BeetleDotVisuals>().ActiveLeftWings.DOLocalRotate(leftWingStartAngle, duration / 2)
            .SetEase(ease));
            Tweens.Add(Dot.VisualController.GetVisuals<BeetleDotVisuals>().ActiveRightWings.DOLocalRotate(rightWingStartAngle, duration / 2)
            .SetEase(ease));

            yield return new WaitForSeconds(duration / 2);

            // Flap down
            Tweens.Add(Dot.VisualController.GetVisuals<BeetleDotVisuals>().ActiveLeftWings.DOLocalRotate(leftWingEndAngle, duration / 2)
            .SetEase(ease));
            Tweens.Add(Dot.VisualController.GetVisuals<BeetleDotVisuals>().ActiveRightWings.DOLocalRotate(rightWingEndAngle, duration / 2)
            .SetEase(ease));

            yield return new WaitForSeconds(duration / 2);

            flapsCompleted++;
        }
        Visuals.ActiveLeftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.ActiveRightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
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
        Tweens.Add(wiggleSequence.Play());
        yield return new WaitForSeconds(wiggleDuration);
    }
    
    public IEnumerator DoFlutterAnimation(){
        float startFlapAngle = Random.Range(10,45);
        float endFlapAngle = 0f;
        int flapCount = Random.Range(1,2);
        float flapDuration = Random.Range(0.5f,0.9f);
        var settings = new AnimationSettings{
            Loops = flapCount,
            Duration = flapDuration
        };

        yield return FlapWings(settings, startFlapAngle, endFlapAngle);
        
    }

    private IEnumerator DoFlutterAndWiggleAnimation(){
        CoroutineHandler.StartStaticCoroutine(DoFlutterAnimation());
        CoroutineHandler.StartStaticCoroutine(DoWiggleAnimation());

        yield return new WaitForSeconds(2);
    }
     
    


  
    public override IEnumerator Hit(AnimationSettings settings)
    {
        float duration = 1f;
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
    }

    public override IEnumerator Clear(AnimationSettings settings)
    {
        bool isExplosion = Dot.HitType.IsExplosion();
        float startFlapAngle = isExplosion ? 20f : 45f;
        float endFlapAngle = isExplosion ? 15f : 0f;
        float duration = settings.Duration;
        float elapsedTime = 0f;
        float amplitude = 1f;
        float frequency = 0.1f;
        float speed = 42f;
        Vector3 direction = new(Dot.DirectionX, Dot.DirectionY);

        Vector3 startPosition = transform.position;
        Vector3 unitDirection = direction.normalized;

        StartCoroutine(base.Clear(new AnimationSettings { Duration = duration * 2 }));

        StartCoroutine(FlapWings(new AnimationSettings{Duration = 0.4f, Loops = -1}, startFlapAngle, endFlapAngle));

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

}