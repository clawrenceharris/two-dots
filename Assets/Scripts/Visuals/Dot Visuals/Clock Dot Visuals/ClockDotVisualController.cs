using System.Collections;
using UnityEngine;
using DG.Tweening;
using static Type;
using System.Linq;
using System.Collections.Generic;
using static UnityEditor.PlayerSettings;

public class ClockDotVisualController : BlankDotVisualController
{
    private new ClockDotVisuals Visuals;
    private new ClockDot Dot;
    private static Coroutine currentAnimationCoroutine;

    public override void Init(Dot dot)
    {
        Dot = (ClockDot)dot;
        Visuals = dot.GetComponent<ClockDotVisuals>();

        base.Init(Dot);




    }

    public override void SetUp()
    {
        base.SetUp();
        UpdateNumbers(Dot.CurrentNumber);
    }

    public override void SetColor()
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

        yield return new WaitForSeconds(Visuals.clearTime);

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


    }

    private static Dictionary<Dot, GameObject> clockDotPreviews = new();

    public void Connect(ClockDot dot)
    {

        List<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots.ToList();



        
        Visuals.clockDotPreview.SetActive(true);
        Visuals.clockDotPreview.transform.SetParent(null);
        
        clockDotPreviews.TryAdd(dot, Visuals.clockDotPreview);
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