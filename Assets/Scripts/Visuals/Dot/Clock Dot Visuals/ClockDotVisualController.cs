using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using static Type;
using System;

public class ClockDotVisualController : BlankDotBaseVisualController, INumerableVisualController, IHittableVisualController
{
    public static Dictionary<Dot, GameObject> ClockDotPreviews { get; private set; } = new();
    private ClockDot dot;
    private  ClockDotVisuals visuals;
    private readonly NumerableVisualControllerBase numerableVisualController = new();
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (ClockDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<ClockDotVisuals>();
        numerableVisualController.Init(dot, visuals.numerableVisuals);
        base.Init(dotsGameObject);
    }


    protected override void SetUp()
    {
        UpdateNumbers(dot.CurrentNumber);
        base.SetUp();
    }

    public override void SetInitialColor()
    {
        visuals.top.color = ColorSchemeManager.CurrentColorScheme.clockDot;
        visuals.middle.color = new Color(255, 255, 255, 0.6f);
        visuals.shadow.color = new Color(255, 255, 255, 0.6f);
        base.SetInitialColor();
    }



    public override void SetColor(Color color)
    {

        base.SetColor(color);

        foreach (Transform child in dot.transform)
        {
            if (child.TryGetComponent(out SpriteRenderer sr))
            {
                //change to bomb color and keep the original alpha value
                sr.color = new Color(color.r, color.g, color.b, sr.color.a);
            }
        }

       
    }



    public void UpdateNumbers(int number)
    {
        numerableVisualController.UpdateNumbers(number);

    }

    public override IEnumerator DoHitAnimation(HitType hitType)
    {
        yield break;
    }



    public override IEnumerator PreviewHit(PreviewHitType hitType)
    {
        CoroutineHandler.StartStaticCoroutine(base.PreviewHit(hitType));

        if(dot.InitialNumber - dot.TempNumber == dot.HitsToClear)
            CoroutineHandler.StartStaticCoroutine(PreviewClear());

        List<ConnectableDot> connectedDots = ConnectionManager.Connection.ConnectedDots.ToList();

        if (ConnectionManager.ConnectedDots.Contains(dot))
        {
            numerableVisualController.UpdateNumbers(dot.TempNumber);

            visuals.clockDotPreview.SetActive(true);
            visuals.clockDotPreview.transform.SetParent(null);
            
        }
        int count = 0;
        ConnectableDot lastAvailableDot;
        
        for (int i = connectedDots.Count - 1; i >= 0; i--)
        {
            Dot dot = connectedDots[i];
            if (dot is ClockDot clockDot)
            {

                count++;

                lastAvailableDot = connectedDots[^count];
                if(lastAvailableDot.DotType.IsBasicDot() || lastAvailableDot.DotType.IsClockDot())
                    clockDot.VisualController.visuals.clockDotPreview.transform.DOMove(lastAvailableDot.transform.position, 0.5f);
                
                yield return null;
            }
        }
    }

   
    public IEnumerator PreviewClear()
    {
        while (dot.IsPreviewing)
        {

            float elapsedTime = 0f;
            Vector3 originalRotation = dot.transform.eulerAngles;
            // Adjust these variables to control the shaking animation
            float shakeDuration = 0.6f;
            float shakeIntensity = 15f;
            float shakeSpeed = 20f;
            while (elapsedTime < shakeDuration && dot.IsPreviewing)
            {
                // Calculate the amount to rotate by interpolating between -shakeIntensity and shakeIntensity
                float shakeAmount = Mathf.Sin(elapsedTime * shakeSpeed) * shakeIntensity;

                // Apply the rotation
                dot.transform.eulerAngles = originalRotation + new Vector3(0, 0, shakeAmount);

                // Increment the elapsed time
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            // Reset rotation to original position after the shaking animation is finished
            dot.transform.eulerAngles = Vector2.zero;

        }
    }
    public override IEnumerator AnimateDeselectionEffect()
    {
        if (!ConnectionManager.ConnectedDots.Contains(dot))

            yield return base.AnimateDeselectionEffect();
    }
    public IEnumerator DoMove(List<Vector2Int> path, Action onMoved)
    {
        float duration = ClockDotVisuals.moveDuration;
        float moveDuration = duration / path.Count;
        foreach (var pos in path)
        {
            dot.transform.DOMove(new Vector2(pos.x, pos.y) * Board.offset, moveDuration);
            yield return new WaitForSeconds(moveDuration - moveDuration /2);
            onMoved?.Invoke();
        }

        yield return base.AnimateDeselectionEffect();

    }

    public void StopPreview()
    {
        visuals.clockDotPreview.SetActive(false);
        visuals.clockDotPreview.transform.SetParent(dot.transform);
        visuals.clockDotPreview.transform.position = dot.transform.position;
    }
}