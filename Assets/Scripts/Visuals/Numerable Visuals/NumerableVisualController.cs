using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class NumerableVisualController : INumerableVisualController

{
    private INumerableVisuals visuals;
    private INumerable numerable;

    
    public void Init(INumerable numerable, INumerableVisuals visuals)
    {
        this.visuals = visuals;
        this.numerable = numerable;
    }
    
    public void UpdateNumberByConnectionCount(ConnectableDot dot){
        
        if(!ConnectionManager.ConnectedDots.Contains(dot)){
            return;
        }
       
        if(ConnectionManager.ConnectedDots.Count == 1){
            numerable.TempNumber = numerable.CurrentNumber;
            UpdateNumbers(numerable.TempNumber);
            return;
        }
        List<IHittable> toHit = ConnectionManager.ToHit;
        numerable.TempNumber = Mathf.Clamp(numerable.CurrentNumber - toHit.Count, 0, int.MaxValue);
        
        UpdateNumbers(numerable.TempNumber);
        

        CoroutineHandler.StartStaticCoroutine(ScaleNumbers());
    }

    public IEnumerator ScaleNumbers()
    {
        float scaleDuration = 0.2f;

        visuals.Digit1.transform.DOScale(Vector2.one * 1.8f, scaleDuration);
        visuals.Digit2.transform.DOScale(Vector2.one * 1.8f, scaleDuration);

        yield return new WaitForSeconds(scaleDuration);
        visuals.Digit1.transform.DOScale(Vector2.one, scaleDuration);
        visuals.Digit2.transform.DOScale(Vector2.one, scaleDuration);
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