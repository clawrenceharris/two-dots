﻿using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using static Type;
using System;

public class ClockDotVisualController : BlankDotBaseVisualController, INumerableVisualController, IPreviewable
{
    public static Dictionary<Dot, GameObject> ClockDotPreviews { get; private set; } = new();
    private ClockDot dot;
    public ClockDotVisuals Visuals { get; private set; }
    private readonly NumerableVisualControllerBase numerableVisualController = new();

    public override T GetGameObject<T>()
    {
        return dot as T;
    }

    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }
    public override void Init(DotsGameObject dotsGameObject)
    {
        base.Init(dotsGameObject);

        dot = (ClockDot)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<ClockDotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        numerableVisualController.Init(Visuals.numerableVisuals);
        SetUp();
    }


    protected override void SetUp()
    {
        base.SetUp();
        UpdateNumbers(dot.CurrentNumber);
    }

    protected override void SetColor()
    {
        Visuals.top.color = ColorSchemeManager.CurrentColorScheme.clockDot;
        Visuals.middle.color = new Color(255, 255, 255, 0.6f);
        Visuals.shadow.color = new Color(255, 255, 255, 0.6f);
        base.SetColor();
    }


    public override IEnumerator ClearAnimation()
    {
        yield return dot.transform.DOScale(Vector2.zero, 0f);
    }


    public override IEnumerator BombHit()
    {
        Color bombColor = ColorSchemeManager.CurrentColorScheme.bombLight;

        SetColor(bombColor);

        foreach (Transform child in dot.transform)
        {
            if (child.TryGetComponent(out SpriteRenderer sr))
            {
                //change to bomb color and keep the original alpha value
                sr.color = new Color(bombColor.r, bombColor.g, bombColor.b, sr.color.a);
            }
        }

        yield return new WaitForSeconds(HittableVisuals.hitDuration);

        //set back to initial color
        SetColor();

    }



    public void UpdateNumbers(int number)
    {
        numerableVisualController.UpdateNumbers(number);

    }


    public void Disconnect()
    {

        Visuals.clockDotPreview.SetActive(false);
        Visuals.clockDotPreview.transform.SetParent(dot.transform);
        Visuals.clockDotPreview.transform.position = dot.transform.position;

    }


    public override IEnumerator HitAnimation(HitType hitType)
    {
        yield break;
    }

    public IEnumerator PreviewHit(HitType hitType)
    {

        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();
        if (connectedDots.Count == 0)
        {
            yield break;
        }
        Visuals.clockDotPreview.SetActive(true);
        Visuals.clockDotPreview.transform.SetParent(null);
        Color color = Visuals.clockDotPreview.GetComponent<SpriteRenderer>().color;
        color.a = 0.6f;
        ClockDotPreviews.TryAdd(dot, Visuals.clockDotPreview);
        for (int i = connectedDots.Count - 1; i >= 0; i--)
        {
            Dot currentDot = connectedDots[i];
            if (currentDot is ClockDot clockDot)
            {
                Dot lastEmptyDot = clockDot;
                for (int k = i; k < connectedDots.Count; k++)
                {
                    Dot nextDot = connectedDots[k];

                    if (ClockDotPreviews.TryGetValue(nextDot, out var _))
                    {
                        continue;
                    }

                    if (nextDot is ClockDot)
                    {
                        continue;
                    }
                    lastEmptyDot = nextDot;
                }


                MoveClockDotPreview(clockDot, lastEmptyDot);
                ClockDotPreviews.Remove(currentDot);



            }

        }

        ClockDotPreviews.Clear();

        UpdateNumbers(dot.TempNumber);
        
    }

    private void MoveClockDotPreview( ClockDot clockDot, Dot destination)
    {
        ClockDotVisualController clockDotVisualController = clockDot.VisualController;
        GameObject clockDotPreview = clockDotVisualController.Visuals.clockDotPreview;
        Vector2 pos = new Vector2(destination.Column, destination.Row) * Board.offset;

        clockDotPreview.transform.DOMove(pos, 0.6f);
        ClockDotPreviews.TryAdd(destination, clockDotPreview);

    }

    public IEnumerator PreviewClear()
    {
        while (dot.HitCount >= dot.HitsToClear)
        {

            float elapsedTime = 0f;
            Vector3 originalRotation = dot.transform.eulerAngles;
            // Adjust these variables to control the shaking animation
            float shakeDuration = 0.6f;
            float shakeIntensity = 15f;
            float shakeSpeed = 20f;
            while (elapsedTime < shakeDuration)
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

    public IEnumerator DoMove(List<Vector2Int> path, Action onComplete)
    {
        float duration = 0.7f;
        float moveDuration = duration / path.Count;

        foreach (var pos in path)
        {
            dot.transform.DOMove(new Vector2(pos.x, pos.y) * Board.offset, moveDuration);
            yield return new WaitForSeconds(moveDuration - moveDuration / 2);
        }

        // Update the final position
        int endCol = path[^1].x;
        int endRow = path[^1].y;
        onComplete?.Invoke();
        
    }
}