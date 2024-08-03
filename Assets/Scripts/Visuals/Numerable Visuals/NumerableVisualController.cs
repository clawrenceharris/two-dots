using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NumerableVisualController : INumerableVisualController

{
    private INumerableVisuals visuals;
    

    public IEnumerator DoIdleAnimation()
    {
        throw new NotImplementedException();
    }

    public void Init(INumerable numerable, INumerableVisuals visuals)
    {
        this.visuals = visuals;
    }
    

    public void UpdateNumbers(int number)
    {
       number = Mathf.Clamp(number, 0, 99);
        string numberStr = number.ToString();
        if (numberStr.Length == 1)
        {
            visuals.Digit1.sprite = GameAssets.Instance.Numbers[0];
            visuals.Digit2.sprite = GameAssets.Instance.Numbers[int.Parse(numberStr)];
        }
        else
        {

            visuals.Digit1.sprite = GameAssets.Instance.Numbers[int.Parse(numberStr[0].ToString())];
            visuals.Digit2.sprite = GameAssets.Instance.Numbers[int.Parse(numberStr[1].ToString())];

        }
    }
}