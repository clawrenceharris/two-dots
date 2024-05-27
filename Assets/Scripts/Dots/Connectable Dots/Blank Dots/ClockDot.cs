using System.Collections;
using UnityEngine;
using static Type;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ClockDot : BlankDotBase, INumerable, IPreviewable
{
    public override DotType DotType => DotType.ClockDot;
    private int initialNumber;
    private int tempNumber;
    private int currentNumber;

    public int InitialNumber { set => initialNumber = value; }
    public int TempNumber { get => tempNumber; }
    public int CurrentNumber { get => currentNumber; }

    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new()
            {


                {
                    HitType.ClockDot, new HitByConnectionRule()
                },
                {
                    HitType.Square, new HitBySquareRule()

                }

            };
        }
    }

    private ClockDotVisualController VisualController
    {
        get
        {
            if (visualController is ClockDotVisualController clockDotVisualController)
            {
                return clockDotVisualController; 
            }
            throw new InvalidCastException("Unable to cast base visualController to ClockDotVisualController");

        }
    }

    public override int HitsToClear => initialNumber;


    public override void Init(int column, int row)
    {
        currentNumber = initialNumber;
        tempNumber = currentNumber;

        base.Init(column, row);
    }
    
    public void UpdateNumber(int number)
    {
       
        VisualController.UpdateNumbers(number);
        
    }

    public override void InitDisplayController()
    {
        visualController = new ClockDotVisualController();
        visualController.Init(this);
    }

    

    public void SetCurrentNumber(int number)
    {
        currentNumber = number;
        UpdateNumber(number);
    }
    
    public override IEnumerator Clear()
    {
        if (IsBomb)
            yield return base.Clear();
        else
            IsBomb = true;
    }

    public override void Disconnect()
    {
        base.Disconnect();
        UpdateNumber(currentNumber);
        VisualController.Disconnect();
    }

    public override void Select()
    {

        base.Select();
    }
    public override void Connect()
    {
        base.Connect();
    }

    public override IEnumerator Hit(HitType hitType)
    {
        if (hitType == HitType.Connection)
        {
            int connectionCount = ConnectionManager.ToHit.Count;

            //allow the current number to decrease by number of connected dots
            tempNumber = Mathf.Clamp(currentNumber - connectionCount, 0, int.MaxValue);

            VisualController.Connect();

            UpdateNumber(tempNumber);
        }

        //this happens when the connection has concluded  
        else if (hitType == HitType.ClockDot)
        {
            SetCurrentNumber(tempNumber);

        }
        else if (hitType == HitType.BombExplosion)
        {
            //set temp number to be one less than the current number
            tempNumber = Mathf.Clamp(currentNumber - 1, 0, int.MaxValue);
            currentNumber = tempNumber;
        }

        HitCount = initialNumber - tempNumber;

        yield return base.Hit(hitType);

    }
    
    
    public override void BombHit()
    {
        UpdateNumber(currentNumber);

        base.BombHit();

    }


    public void PreviewHit()
    {
        
        ClockDotAnimation animation = new();
        StartCoroutine(animation.HitAnimation(this));
        

    }

}
