using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using static Type;

public class ClockDotVisualController : BlankDotVisualController, IPreviewable
{
    public static Dictionary<Dot, GameObject> clockDotPreviews { get; private set; } = new();
    private ClockDot dot;
    private ClockDotVisuals visuals;
    public override T GetVisuals<T>()
    {
        return visuals as T;
    }
    public override T GetGameObject<T>()
    {
        return dot as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        base.Init(dotsGameObject);
        dot = (ClockDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<ClockDotVisuals>();
        SetUp();
    }


    protected override void SetUp()
    {
        base.SetUp();
        UpdateNumbers(dot.CurrentNumber);
    }

    protected override void SetColor()
    {
        visuals.top.color = ColorSchemeManager.CurrentColorScheme.clockDot;
        visuals.middle.color = new Color(255, 255, 255, 0.6f);
        visuals.shadow.color = new Color(255, 255, 255, 0.6f);

    }





    public override IEnumerator BombHit()
    {
        spriteRenderer.color = Color.white;

        foreach (Transform child in dot.transform)
        {
            if (child.TryGetComponent(out SpriteRenderer sr))
            {
                sr.color = Color.white;
            }
        }

        yield return new WaitForSeconds(HittableVisuals.defaultClearDuration);

        SetColor();

    }



    public void UpdateNumbers(int number)
    {
        string numberStr = number.ToString();
        if (numberStr.Length == 1)
        {
            visuals.digit1.sprite = visuals.numbers[0];
            visuals.digit2.sprite = visuals.numbers[int.Parse(numberStr)];
        }
        else
        {

            visuals.digit1.sprite = visuals.numbers[int.Parse(numberStr[0].ToString())];
            visuals.digit2.sprite = visuals.numbers[int.Parse(numberStr[1].ToString())];

        }

    }


    public void Disconnect()
    {

        visuals.clockDotPreview.SetActive(false);
        visuals.clockDotPreview.transform.SetParent(dot.transform);
        visuals.clockDotPreview.transform.position = dot.transform.position;

    }


    public override IEnumerator Hit(HitType hitType)
    {

        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();
        if(connectedDots.Count == 0)
        {
            yield break;
        }
        visuals.clockDotPreview.SetActive(true);
        visuals.clockDotPreview.transform.SetParent(null);
        Color color = visuals.clockDotPreview.GetComponent<SpriteRenderer>().color;
        color.a = 0.6f;
        clockDotPreviews.TryAdd(dot, visuals.clockDotPreview);
        for(int i = connectedDots.Count -1 ; i >= 0 ; i--)
        {
            if (connectedDots[i] is ClockDot clockDot)
            {
                Dot lastEmptyDot = clockDot;
                for(int k = i; k < connectedDots.Count; k++)
                {
                    if(clockDotPreviews.TryGetValue(connectedDots[k], out var _))
                    {
                        continue;
                    }
                    
                    if (connectedDots[k] is ClockDot)
                    {
                        continue;
                    }
                    lastEmptyDot = connectedDots[k];
                }

                    
                    MoveClockDotPreview(clockDot, lastEmptyDot);
                    clockDotPreviews.Remove(connectedDots[i]);
                

                
            }
           
        }
       
        clockDotPreviews.Clear();

        yield return base.Hit(hitType);
    }

    public IEnumerator PreviewHit(HitType hitType)
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

    private void MoveClockDotPreview( ClockDot clockDot, Dot destination)
    {
        ClockDotVisualController clockDotVisualController = clockDot.VisualController;
        GameObject clockDotPreview = clockDotVisualController.visuals.clockDotPreview;
        Vector2 pos = new Vector2(destination.Column, destination.Row) * Board.offset;

        clockDotPreview.transform.DOMove(pos, 0.6f);
        clockDotPreviews.TryAdd(destination, clockDotPreview);

    }



}