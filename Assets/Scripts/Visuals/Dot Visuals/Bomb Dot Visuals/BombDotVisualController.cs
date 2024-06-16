using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class BombDotVisualController : DotVisualController
{

    public new BombDotVisuals Visuals;

    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BombDotVisuals>();
        base.Init(dot);
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
    public IEnumerator AnimateLine(IHittable hittable, Action onHittableReached)
    {
        float elapsedTime = 0f;
        float duration = 0.3f;

        Vector2 startPos = Dot.transform.position;
        Vector2 endPos = new Vector2(hittable.Column, hittable.Row) * Board.offset;

        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);

        ConnectorLine line = Object.Instantiate(GameAssets.Instance.Line);

        line.transform.parent = Dot.transform;
        line.transform.localScale = new Vector2(2f, 0.1f);
        line.sprite.color = Bomb.Hits.Contains(hittable) ? Color.clear : ColorSchemeManager.CurrentColorScheme.blank;
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
        onHittableReached?.Invoke();




    }

    public override IEnumerator BombHit()
    {
        yield break;
    }

}
