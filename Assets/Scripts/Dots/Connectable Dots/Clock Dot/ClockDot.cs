using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using System.Linq;
using Unity.VisualScripting;



public class ClockDot : BlankDotBase, INumerable
{
    public override DotType DotType => DotType.ClockDot;
    private int initialNumber;
    private int tempNumber;
    private int currentNumber;

    public int InitialNumber { set => initialNumber = value; }
    public int TempNumber { get => tempNumber; }
    public int CurrentNumber { get => currentNumber; }

    public override int HitsToClear => initialNumber;


    public override void Init(int column, int row)
    {
        currentNumber = initialNumber;
        tempNumber = currentNumber;

        base.Init(column, row);
    }
    
    public void UpdateNumber(int number)
    {
        if(visualController is ClockDotVisualController clockDotVisualController)
        {

            clockDotVisualController.UpdateNumbers(number);
        }
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

    public override IEnumerator Hit(HitType hitType)
    {
        if (hitType == HitType.ClockDot)
        {
            int connectionCount = ConnectionManager.ToHit.Count;

            //allow the current number to decrease by number of connected dots
            tempNumber = Mathf.Clamp(currentNumber - connectionCount, 0, int.MaxValue);

            UpdateNumber(tempNumber);
        }

        //this happens when the connection has concluded  
        else if(hitType == HitType.Connection)
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




}
