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
            return new(base.HitRules)
            {
                {
                    HitType.ClockDot, new HitByConnectionRule()
                },
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
    
    public void UpdateNumberVisuals(int number)
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
        UpdateNumberVisuals(number);
    }
    
    public override IEnumerator Clear()
    {
        NotifyDotCleared();

        DotController.DoBombDot(this);

        yield return null;
    }

    public override void Disconnect()
    {
        base.Disconnect();
        UpdateNumberVisuals(currentNumber);
        HitCount = currentNumber;
        VisualController.Disconnect();
    }

    public override void Select()
    {

        base.Select();
    }
    

    public override IEnumerator Hit(HitType hitType)
    {
        if (hitType == HitType.Connection)
        {
            int connectionCount = ConnectionManager.ToHit.Count;

            //allow the current number to decrease by number of connected dots
            tempNumber = Mathf.Clamp(currentNumber - connectionCount, 0, int.MaxValue);

            UpdateNumberVisuals(tempNumber);
        }

        //this happens when the connection has concluded  
        else if (hitType == HitType.ClockDot)
        {
            SetCurrentNumber(tempNumber);

        }
        else if (hitType == HitType.BombExplosion)
        {
            //set current number to be one less than the current number
            SetCurrentNumber(Mathf.Clamp(currentNumber - 1, 0, int.MaxValue)); 

        }

        HitCount = initialNumber - tempNumber;

        yield return base.Hit(hitType);

    }
    
    
  

    public IEnumerator PreviewHit()
    {
        
        ClockDotAnimation animation = new();
        yield return StartCoroutine(animation.HitAnimation(this));
        

    }

}
