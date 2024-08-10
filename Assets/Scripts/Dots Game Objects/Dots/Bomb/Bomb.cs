using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class Bomb : Dot, IExplodable
{
    
    
    public override DotType DotType => DotType.Bomb;
   

    public IExplosionRule ExplosionRule => new BombExplosionRule();
    public override int HitsToClear => 1;
    public static BombExplosionManager bombExplosionManager = new();
    public override IHitRule HitRule => null;

    public static List<IHittable> Hits { get; } = new();// the list of hittables all Bomb objects have hit

    public ExplosionType ExplosionType => ExplosionType.BombExplosion;

    public new BombDotVisualController VisualController => GetVisualController<BombDotVisualController>();

    public int HitsToExplode => 0;


    public override void InitDisplayController()
    {
        visualController = new BombDotVisualController();
        visualController.Init(this);
    }


    private void OnEnable()
    {
        BombExplosionManager.bombs.Add(this);
    }


    private void OnDisable()
    {
        BombExplosionManager.bombs.Remove(this);
    }


    public IEnumerator ChangeHittablesColor(IHittable hittable)
    {
        float duration = 0.2f;
        DotsGameObject dotsGameObject = (DotsGameObject)hittable;

        //set the color to the bomb color
        dotsGameObject.VisualController.SetColor(ColorSchemeManager.CurrentColorScheme.bombLight);
        yield return new WaitForSeconds(duration);

        //undo previous color change    
        dotsGameObject.VisualController.SetInitialColor();

        
    }


    
    public IEnumerator Explode(List<IHittable> toHit, Action<IHittable> onComplete)
    {
        List<IHittable> hittables = new(toHit.Where(hittable => hittable is not Bomb));

        bombExplosionManager.AssignHittablesToBombs(hittables);
        
        int counter = 0;


        foreach (var bomb in BombExplosionManager.bombToDotsMap.Keys)
        {
            foreach (IHittable hittable in BombExplosionManager.bombToDotsMap[bomb])
            {
    
                if (hittable != null && !Hits.Contains(hittable))
                {
                    Hits.Add(hittable);
                    CoroutineHandler.StartStaticCoroutine(bomb.VisualController.DoLineAnimation(hittable, () =>
                    {
                        CoroutineHandler.StartStaticCoroutine(ChangeHittablesColor(hittable),() => counter++);

                    }));

                    yield return new WaitForSeconds(0.02f);

                }
                else
                {
                    counter++;

                }
                if (hittable == hittables.Last())
                {
                    yield return new WaitForSeconds(0.1f);
                }


            }

        }


        yield return new WaitUntil(() => counter == hittables.Count);
        foreach (IHittable hittable in toHit)
        {
            onComplete?.Invoke(hittable);

        }

        Hits.Clear();
        BombExplosionManager.bombs.Clear();
    }

    

    public override void Hit(HitType hitType)
    {
        HitCount++;
    }
    

}
   

