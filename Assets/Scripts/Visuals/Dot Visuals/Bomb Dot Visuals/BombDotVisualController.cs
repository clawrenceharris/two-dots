using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using DG.Tweening;
public class BombDotVisualController : DotVisualController
{
    private BombDot dot;
    public BombDotVisuals Visuals { get; private set; }


    public override T GetGameObject<T>()
    {
        return dot as T;
    }

    public override T GetVisuals<T>()
    {
        return Visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        base.Init(dotsGameObject);

        dot = (BombDot)dotsGameObject;
        Visuals = dotsGameObject.GetComponent<BombDotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();

    }

    protected override void SetColor()
    {
        for (int i = 0; i  < Visuals.bombSprites.Length; i++)
        {
            if(i % 2 == 0)
                Visuals.bombSprites[i].color = ColorSchemeManager.CurrentColorScheme.bombLight;
            else
                Visuals.bombSprites[i].color = ColorSchemeManager.CurrentColorScheme.bombDark;

        }
    }


    public IEnumerator AnimateLine(IHittable hittable)
    {

        float elapsedTime = 0f;
        float duration = 0.3f;

        Vector2 startPos = dot.transform.position;
        Vector2 endPos = new Vector2(hittable.Column, hittable.Row) * Board.offset;
        Vector2 startScale = new(1f, 0.2f);
        Vector2 endScale = new(0.7f, 0.03f);

        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);

        ConnectorLine line = Object.Instantiate(GameAssets.Instance.Line);

        line.transform.parent = dot.transform;
        line.transform.localScale = startScale;
        line.sprite.color = BombDot.Hits.Contains(hittable) || hittable is BombDot ? Color.clear : ColorSchemeManager.CurrentColorScheme.bombLight;
        line.transform.rotation = Quaternion.Euler(1f, 0, angle);
        line.disabled = true;

        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; 

            Vector3 newPos = Vector3.Lerp(startPos, endPos, t);
            Vector3 newScale = Vector3.Lerp(startScale, endScale, t);

            // Update line position
            line.transform.position = newPos;
            line.transform.localScale = newScale;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return line.transform.DOScale(new Vector2(0, line.transform.localScale.y), 0.4f);
        Object.Destroy(line.gameObject);


        
       
    }

    public override IEnumerator DoBombHit()
    {
        yield break;
    }



}
