using System.Collections;
using UnityEngine;
using static Type;
using System;
using System.Collections.Generic;

public class ClockDot : BlankDotBase, INumerable, IPreviewable
{
    public override DotType DotType => DotType.ClockDot;
    private int tempNumber;
    private NumerableBase numerable;
    public int InitialNumber { get => numerable.InitialNumber; set => numerable.InitialNumber = value; }
    public int CurrentNumber { get => numerable.CurrentNumber;}

    public override Dictionary<HitType, IHitRule> HitRules
    {
        get
        {
            return new()
            {
                {

                    HitType.ClockDot, new HitByConnectionRule()
                },
                
            };
        }
    }

    public new ClockDotVisualController VisualController => GetVisualController<ClockDotVisualController>();

    public override int HitsToClear => numerable.InitialNumber;

    public override void Init(int column, int row)
    {
        numerable.UpdateCurrentNumber(InitialNumber);
        tempNumber = CurrentNumber;

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

    

    public void UpdateCurrentNumber(int number)
    {
        
        numerable.UpdateCurrentNumber(number);
    }
    
    public override void Disconnect()
    {
        base.Disconnect();
        numerable.UpdateNumberVisuals(CurrentNumber);
        HitCount = CurrentNumber;
        VisualController.Disconnect();
    }


    public override IEnumerator Hit(HitType hitType)
    {
        if (hitType == HitType.Connection)
        {
            int connectionCount = ConnectionManager.ToHit.Count;

            //allow the current number to decrease by number of connected dots
            tempNumber = Mathf.Clamp(CurrentNumber - connectionCount, 0, int.MaxValue);

            UpdateNumberVisuals(tempNumber);
            StartCoroutine(VisualController.Hit(hitType));
            HitCount = InitialNumber - tempNumber;

        }

        //this happens when the connection has concluded  
        else if (hitType == HitType.ClockDot)
        {
            UpdateCurrentNumber(tempNumber);
            HitCount = InitialNumber - tempNumber;

        }
        else if (hitType == HitType.BombExplosion)
        {
            //set current number to be one less than the current number
            numerable.UpdateCurrentNumber(Mathf.Clamp(CurrentNumber - 1, 0, int.MaxValue));
            HitCount++;

        }
        yield return VisualController.Hit(hitType);


    }


    public IEnumerator PreviewHit(HitType hitType)
    {

        yield return VisualController.PreviewHit(hitType);
        

    }

    public IEnumerator PreviewClear()
    {
        yield return VisualController.PreviewClear();
    }
}
