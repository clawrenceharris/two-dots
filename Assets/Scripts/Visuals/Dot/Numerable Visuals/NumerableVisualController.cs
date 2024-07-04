using System;
using System.Collections;
using Unity.VisualScripting;

public class NumerableVisualControllerBase : INumerableVisualController

{
    private INumerableVisuals visuals;
    public void Init(INumerableVisuals visuals)
    {
        this.visuals = visuals;
    }
    

    public void UpdateNumbers(int number)
    {
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