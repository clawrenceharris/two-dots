using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using DG.Tweening;
public class BombDotVisualController : DotVisualController
{
    private BombDot dot;
    private BombDotVisuals visuals;

    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BombDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BombDotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();

    }

    protected override void SetColor()
    {
        for (int i = 0; i  < visuals.bombSprites.Length; i++)
        {
            if(i % 2 == 0)
                visuals.bombSprites[i].color = ColorSchemeManager.CurrentColorScheme.bombLight;
            else
                visuals.bombSprites[i].color = ColorSchemeManager.CurrentColorScheme.bombDark;

        }
    }


    public IEnumerator DoLineAnimation(IHittable hittable)
    {

        float elapsedTime = 0f;
        float duration = 0.25f;

        Vector2 startPos = dot.transform.position;
        Vector2 endPos = new Vector2(hittable.Column, hittable.Row) * Board.offset;
        Vector2 startScale = new(1f, 0.2f);
        Vector2 endScale = new(0.7f, 0.03f);

        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);

        ConnectorLine line = Object.Instantiate(GameAssets.Instance.Line);

        line.transform.parent = dot.transform;
        line.transform.localScale = startScale;
        line.sprite.enabled = !BombDot.Hits.Contains(hittable) && hittable is not BombDot;
        line.transform.rotation = Quaternion.Euler(1f, 0, angle);
        line.disabled = true;

        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; 

            Vector3 newPos = Vector2.Lerp(startPos, endPos, t);
            Vector3 newScale = Vector2.Lerp(startScale, endScale, t);

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
