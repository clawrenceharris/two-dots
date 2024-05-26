using System.Collections;
using UnityEngine;
using DG.Tweening;
using static Type;
using System.Linq;
using System.Collections.Generic;

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

    public void PreviewHit()
    {
        

    }
    public void Disconnect(Dot dot)
    {
        if (dot == Dot)
        {
            Visuals.clockDotPreview.SetActive(false);
            Visuals.clockDotPreview.transform.SetParent(Dot.transform);
        }
        //else
        //{
        //    LinkedList<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots;
        //    if (connectedDots.Last.Value is ClockDot)
        //    {
        //        return;
        //    }
        //    ConnectableDot nextDot = connectedDots.Find(Dot).Previous.Value;

        //    Vector2 pos = new Vector2(nextDot.Column, nextDot.Row) * Board.offset;
        //    Visuals.clockDotPreview.transform.DOMove(pos, 0.6f);
        //    Visuals.clockDotPreview.transform.SetParent(null);
        //    Visuals.clockDotPreview.SetActive(true);

        //}
    }

    

    public void Connect()
    {
        
        LinkedList<ConnectableDot> connectedDots = ConnectionManager.ConnectedDots;
        if (connectedDots.Count == 1)
        {
            Visuals.clockDotPreview.SetActive(false);
            Visuals.clockDotPreview.transform.SetParent(Dot.transform);
        }

        if (connectedDots.Last.Value is ClockDot)
        {
            return;
        }
        
        Dot nextDot = connectedDots.Last.Value;

        Vector2 pos = new Vector2(nextDot.Column, nextDot.Row) * Board.offset;
        Visuals.clockDotPreview.transform.DOMove(pos, 0.6f);
        Visuals.clockDotPreview.transform.SetParent(null);

        Visuals.clockDotPreview.SetActive(true);

    }

    public void StartConnection()
    {
    }
}