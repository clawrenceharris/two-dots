using System.Collections;
using UnityEngine;
public class ClockDotVisualController : BlankDotVisualController
{
    private new ClockDotVisuals Visuals;
    private new ClockDot Dot;
     
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
        Visuals.middle.color = new Color(255,255,255, 0.6f);
        Visuals.shadow.color = new Color(255, 255, 255, 0.6f);

    }



    public IEnumerator ReadyToHit()
    {
        yield return null;
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
        if(numberStr.Length == 1)
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
}