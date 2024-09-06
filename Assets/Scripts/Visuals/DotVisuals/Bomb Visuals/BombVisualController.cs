using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using DG.Tweening;

public class BombVisualController : DotVisualController
{
    private Bomb dot;
    private BombVisuals visuals;
    public override T GetGameObject<T>() => dot as T;

    public override T GetVisuals<T>() => visuals as T;
    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (Bomb)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BombVisuals>();
        base.Init(dotsGameObject);

    }
   
    public override void SetInitialColor()
    {
        for (int i = 0; i  < visuals.BombSprites.Length; i++)
        {
            if(i % 2 == 0)
                visuals.BombSprites[i].color = ColorSchemeManager.CurrentColorScheme.bombLight;
            else
                visuals.BombSprites[i].color = ColorSchemeManager.CurrentColorScheme.bombDark;

        }
    }

    public override void SetColor(Color color)
    {
        for(int i = 0; i < visuals.BombSprites.Length; i++)
            visuals.BombSprites[i].color = color;
    }


    public IEnumerator DoLineAnimation(IHittable hittable, Action callback = null)
    {

        float duration = visuals.ExplodeDuration;
        Vector3 startPos = dot.transform.position;
        DotsGameObject dotsGameObject = (DotsGameObject)hittable;
        Vector3 targetPosition = new Vector3(dotsGameObject.Column, dotsGameObject.Row) * Board.offset;

        float angle = Vector2.SignedAngle(Vector2.right, targetPosition - startPos);
        float distance = Vector2.Distance(startPos, targetPosition);
        distance -= dot.transform.localScale.x / 2 + dotsGameObject.transform.localScale.x / 2;

        ConnectorLine line = Object.Instantiate(GameAssets.Instance.Line);

        line.enabled = false;
        line.SpriteRenderer.color = ColorSchemeManager.CurrentColorScheme.bombLight;
        line.SpriteRenderer.sortingLayerName = "Bomb";
        line.SpriteRenderer.sortingOrder = 100;
        line.transform.SetPositionAndRotation(startPos, Quaternion.Euler(0, 0, angle));

        line.transform.localScale = new Vector3(0, 0.2f);
        line.transform.parent = dot.transform;

        line.transform.DOScale(new Vector3(distance, 0.2f), duration / 2);
        yield return new WaitForSeconds(duration / 2);

        callback?.Invoke();
        Vector3 position = targetPosition - (targetPosition - startPos).normalized * distance;

        line.transform.DOMove(position, duration);


        line.transform.DOScale(new Vector3(distance, 0.1f), duration / 2);


        yield return new WaitForSeconds(duration / 2);
        line.transform.DOMove(targetPosition, duration/2);

        line.transform.DOScale(new Vector3(0, 0.1f), duration / 2);
        yield return new WaitForSeconds(duration / 2);
        Object.Destroy(line.gameObject);
    }





}
