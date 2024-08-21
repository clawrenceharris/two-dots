using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using UnityEngine;

public class LotusDotVisualController : ColorableDotVisualController, IPreviewableVisualController
{
    private LotusDot dot;
    private LotusDotVisuals visuals;

    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (LotusDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<LotusDotVisuals>();

        base.Init(dotsGameObject);
    }

    public override void SetInitialColor()
    {
        Color initialColor = ColorSchemeManager.FromDotColor(dot.Color);
        foreach (Transform child in dot.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = initialColor;
        }

        visuals.Layer3.color = ColorUtils.LightenColor(initialColor, 0.5f);

    }

    public override void SetColor(Color color)
    {
        foreach (Transform child in dot.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sr.color = color;
        }

    }
    
    public IEnumerator DoIdleAnimation()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));
        int direction = (Random.Range(0, 2) * 2) - 1;
        IEnumerator[] coroutines = { DoRotate90Animation(direction), DoRotate360Animation(direction) };
        int rand = Random.Range(0, coroutines.Length);

        yield return coroutines[rand];
        yield return new WaitForSeconds(Random.Range(2, 4));
       

    }
   

    private IEnumerator DoRotate360Animation(int direction)
    {
        visuals.Layer1.transform.DOLocalRotate(new Vector3(0, 0, 360 * direction), 8f * 2, RotateMode.LocalAxisAdd)
    .SetEase(visuals.LotusBounceCurve);
        visuals.Layer2.transform.DOLocalRotate(new Vector3(0, 0, 360 * direction), 7f * 2, RotateMode.LocalAxisAdd)
    .SetEase(visuals.LotusBounceCurve); 
        visuals.Layer3.transform.DOLocalRotate(new Vector3(0, 0, 360 * direction), 6f *2, RotateMode.LocalAxisAdd)
    .SetEase(visuals.LotusBounceCurve); 

        yield return new WaitForSeconds(8 * 2);
    }
   
     private IEnumerator DoRotate90Animation(int direction)
    {
        visuals.Layer1.transform.DORotate(new Vector3(0,0, 90 * direction), 4f, RotateMode.LocalAxisAdd)
    .SetEase(visuals.LotusBounceCurve); 
        
        visuals.Layer2.transform.DORotate(new Vector3(0,0, -90 * direction), 4f, RotateMode.LocalAxisAdd)
    .SetEase(visuals.LotusBounceCurve);
        visuals.Layer3.transform.DORotate(new Vector3(0,0, 90 * direction), 4f, RotateMode.LocalAxisAdd)
    .SetEase(visuals.LotusBounceCurve);
        yield return new WaitForSeconds(4);
    }

    public IEnumerator DoHitPreviewAnimation()
    {
        yield break;
    }

    public IEnumerator DoClearPreviewAnimation()
    {
        yield break;
    }
}
