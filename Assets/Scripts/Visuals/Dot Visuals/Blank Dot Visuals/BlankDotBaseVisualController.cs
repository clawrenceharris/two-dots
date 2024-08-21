using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Linq;

public abstract class BlankDotBaseVisualController : ColorableDotVisualController
{

    public override void Init(DotsGameObject dotsGameObject)
    {
        base.Init(dotsGameObject);
        ConnectionManager.onDotConnected += OnDotConnected;
        ConnectionManager.onDotDisconnected += OnDotDisconnected;
    }


    public void SetInnerColor(Color color)
    {
        GetVisuals<BlankDotVisuals>().innerDot.color = color;
    }

    public override void SetInitialColor()
    {
        GetVisuals<Visuals>().spriteRenderer.color = ColorSchemeManager.CurrentColorScheme.blank;
    }

    public override IEnumerator AnimateSelectionEffect()
    {
        CoroutineHandler.StartStaticCoroutine(base.AnimateSelectionEffect());
        BlankDotVisuals visuals = GetVisuals<BlankDotVisuals>();


        Color color = ColorSchemeManager.FromDotColor(ConnectionManager.Connection.Color);

        visuals.innerDot.color = color;
        GetVisuals<DotVisuals>().outerDot.color = color;

        yield return visuals.innerDot.transform.DOScale(Vector2.one, BlankDotVisuals.innerDotScaleDuration);

    }

    public virtual IEnumerator AnimateDeselectionEffect()
    {
        BlankDotVisuals visuals = GetVisuals<BlankDotVisuals>();

        yield return visuals.innerDot.transform.DOScale(Vector2.zero, BlankDotVisuals.innerDotScaleDuration);

    }

    public void OnDotConnected(ConnectionArgs args){
        BlankDotBase dot = GetGameObject<BlankDotBase>();
        if(ConnectionManager.ConnectedDots.Contains(dot)){
            dot.UpdateColor();
        }
    }
     public void OnDotDisconnected(ConnectionArgs args){
        BlankDotBase dot = GetGameObject<BlankDotBase>();
        if(ConnectionManager.ConnectedDots.Contains(dot)){
            dot.UpdateColor();
        }
    }
    

   
}