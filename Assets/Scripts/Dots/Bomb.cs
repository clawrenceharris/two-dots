using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static Type;

public class Bomb : Dot, IExplodable
{


    public override DotType DotType => DotType.Bomb;

    public Dictionary<HitType, IExplosionRule> ExplosionRules => new() { { HitType.BombExplosion, new BombExplosionRule() } };

    public override int HitsToClear => 1;

    public override Dictionary<HitType, IHitRule> HitRules => new();

    private static List<IHittable> Hits { get; } = new();
    public static event Action<IHittable> onBombExploded;

    public override void InitDisplayController()
    {
        visualController = new DotVisualController();
        visualController.Init(this);
    }


    private IEnumerator AnimateLine(IHittable hittable)
    {
        float elapsedTime = 0f;
        float duration = 0.3f;

        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(hittable.Column, hittable.Row) * Board.offset;

        float angle = Vector2.SignedAngle(Vector2.right, endPos - startPos);

        ConnectorLine line = Instantiate(GameAssets.Instance.Line);

        line.transform.parent = transform;
        line.transform.localScale = new Vector2(1f, 0.1f);
        line.sprite.color = Hits.Contains(hittable) ?  Color.clear : ColorSchemeManager.CurrentColorScheme.blank;
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
        Destroy(line.gameObject);

        yield return hittable.Hit(HitType.BombExplosion);
        
        

    }

    private void OnDisable()
    {
        Hits.Clear();
    }


    public IEnumerator Explode(List<IHittable> hittables)
    {

        List<Coroutine> lineCoroutines = new(); // Store coroutines for each line

        foreach (IHittable hittable in hittables)
        {
            if (hittable is Bomb)
            {
                continue;
            }
            
            Coroutine lineCoroutine = StartCoroutine(AnimateLine(hittable));
            lineCoroutines.Add(lineCoroutine);
            Hits.Add(hittable);


        }

        // Wait for all line animations to finish
        foreach (Coroutine coroutine in lineCoroutines)
        {
            yield return coroutine;
        }

        


    }
    
    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;
        yield return null;
    }

   
}
   

