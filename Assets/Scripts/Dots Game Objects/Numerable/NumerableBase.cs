using System;
using UnityEngine;
using System.Collections;

public class NumerableBase : INumerable
{

    public int CurrentNumber { get; private set; }
    public int TempNumber { get; set; }
    public int InitialNumber { get; set; }
    private INumerable numerable;
    public DotsGameObject GetGameObject() => (DotsGameObject)numerable;
    public T GetGameObject<T>() where T : DotsGameObject => (T)numerable;

    public INumerableVisualController VisualController
    {
        get
        {
            DotsGameObject dotsObject = GetGameObject();
            return dotsObject.GetVisualController<INumerableVisualController>();
        }
    }

   

    public void Init(INumerable numerable)
    {
        this.numerable = numerable;
        UpdateCurrentNumber(InitialNumber);
    }

    public void UpdateCurrentNumber(int number)
    {
        CurrentNumber = number;
        VisualController.UpdateNumbers(number);
    }



    public void Hit(HitType hitType)
    {

        if (hitType.IsConnection())
        {
            UpdateCurrentNumber(TempNumber);

        }


        else if (hitType.IsExplosion())
        {
            //set current number to be one less than the current number
            TempNumber = Mathf.Clamp(CurrentNumber - 1, 0, int.MaxValue);
            UpdateCurrentNumber(TempNumber);

        };

    }
}