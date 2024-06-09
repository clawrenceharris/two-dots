using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Collections.Generic;
using static Type;

public class ClockDotVisualController : BlankDotVisualController
{
    private new ClockDotVisuals Visuals;
    private new ClockDot Dot;
    public static Dictionary<Dot, GameObject> clockDotPreviews { get; private set; } = new();

    public override void Init(Dot dot)
    {
        Dot = (ClockDot)dot;
        Visuals = dot.GetComponent<ClockDotVisuals>();

        base.Init(Dot);




    }

    protected override void SetUp()
    {
        base.SetUp();
        UpdateNumbers(Dot.CurrentNumber);
    }

    protected override void SetColor()
    {
        Visuals.top.color = ColorSchemeManager.CurrentColorScheme.clockDot;
        Visuals.middle.color = new Color(255, 255, 255, 0.6f);
        Visuals.shadow.color = new Color(255, 255, 255, 0.6f);

    }





    public override IEnumerator BombHit()
    {
        SpriteRenderer.color = Color.white;

        foreach (Transform child in Dot.transform)
        {
            if (child.TryGetComponent(out SpriteRenderer sr))
            {
                sr.color = Color.white;
            }
        }

        yield return new WaitForSeconds(DotVisuals.defaultClearTime);

        SetColor();

    }



    public void UpdateNumbers(int number)
    {
        string numberStr = number.ToString();
        if (numberStr.Length == 1)
        {
            Visuals.digit1.sprite = Visuals.numbers[0];
            Visuals.digit2.sprite = Visuals.numbers[int.Parse(numberStr)];
        }
        else
        {

            Visuals.digit1.sprite = Visuals.numbers[int.Parse(numberStr[0].ToString())];
            Visuals.digit2.sprite = Visuals.numbers[int.Parse(numberStr[1].ToString())];

        }

    }


    public void Disconnect()
    {

        Visuals.clockDotPreview.SetActive(false);
        Visuals.clockDotPreview.transform.SetParent(Dot.transform);
        Visuals.clockDotPreview.transform.position = Dot.transform.position;

    }


    public override IEnumerator Hit(HitType hitType)
    {

        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();
        if(connectedDots.Count == 0)
        {
            yield break;
        }
        Visuals.clockDotPreview.SetActive(true);
        Visuals.clockDotPreview.transform.SetParent(null);
        Color color = Visuals.clockDotPreview.GetComponent<SpriteRenderer>().color;
        color.a = 0.6f;
        clockDotPreviews.TryAdd(Dot, Visuals.clockDotPreview);
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

    public override IEnumerator PreviewHit(HitType hitType)
    {
        while (Dot.HitCount >= Dot.HitsToClear)
        {

            float elapsedTime = 0f;
            Vector3 originalRotation = Dot.transform.eulerAngles;
            // Adjust these variables to control the shaking animation
            float shakeDuration = 0.6f;
            float shakeIntensity = 15f;
            float shakeSpeed = 20f;
            while (elapsedTime < shakeDuration)
            {
                // Calculate the amount to rotate by interpolating between -shakeIntensity and shakeIntensity
                float shakeAmount = Mathf.Sin(elapsedTime * shakeSpeed) * shakeIntensity;

                // Apply the rotation
                Dot.transform.eulerAngles = originalRotation + new Vector3(0, 0, shakeAmount);

                // Increment the elapsed time
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            // Reset rotation to original position after the shaking animation is finished
            Dot.transform.eulerAngles = Vector2.zero;

        }
        yield return base.PreviewHit(hitType);
    }

    private void MoveClockDotPreview( ClockDot clockDot, Dot destination)
    {
        ClockDotVisualController clockDotVisualController = (ClockDotVisualController)clockDot.visualController;
        GameObject clockDotPreview = clockDotVisualController.Visuals.clockDotPreview;
        Vector2 pos = new Vector2(destination.Column, destination.Row) * Board.offset;

        clockDotPreview.transform.DOMove(pos, 0.6f);
        clockDotPreviews.TryAdd(destination, clockDotPreview);

    }



}