using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class BombDotVisualController : DotVisualController
{
    private BombDot dot;
    private BombDotVisuals visuals;

    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    public override T GetGameObject<T>()
    {
        return dot as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BombDot)dotsGameObject;
        visuals = dot.GetComponent<BombDotVisuals>();
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


    public IEnumerator AnimateLine(IHittable hittable)
    {

        float elapsedTime = 0f;
        float duration = 0.3f;

        Vector2 startPos = dot.transform.position;
        Vector2 endPos = new Vector2(hittable.Column, hittable.Row) * Board.offset;

        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);

        ConnectorLine line = Object.Instantiate(GameAssets.Instance.Line);

        line.transform.parent = dot.transform;
        line.transform.localScale = new Vector2(1f, 0.1f);
        line.sprite.color = BombDot.AllHits.Contains(hittable) ? Color.clear : ColorSchemeManager.CurrentColorScheme.bombLight;
        line.transform.rotation = Quaternion.Euler(0, 0, angle);
        line.disabled = true;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Normalized time

            // Perform linear interpolation between start and end positions
            Vector3 newPos = Vector3.Lerp(startPos, endPos, t);

            // Update line position
            line.transform.position = newPos;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Object.Destroy(line.gameObject);


    }

    public override IEnumerator BombHit()
    {
        yield break;
    }

   
}
