using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class PreviewableStateManager : MonoBehaviour
{

    public static Board Board {get; private set;}
    public DotsGameObject DotsGameObject {get; private set;}
     private IPreviewable previewable;
    public IPreviewable Previewable {
        
        get{
            previewable ??= GetComponent<IPreviewable>();
            return previewable;
        }
    }
    
    private void Awake(){
        DotsGameObject = GetComponent<DotsGameObject>();
        Board = FindObjectOfType<Board>();
        
    }

    public static int Count<T>(T previewable) 
    where T : class, IPreviewable
    {
        return Board.FindDotsOfType<T>().Count;
    }
    private void OnDestroy(){
        StopAllCoroutines();
        
    }
    private readonly List<IState> states = new () {
        new IdleState(), 
        new PreviewHitState(),
        new PreviewClearState()
        };

    private void Start(){
       StartAllStates();

    }
    private bool CheckState(IState targetState)
    {
        
       bool shouldExecute = false;

        if (targetState is PreviewHitState && Previewable.ShouldPreviewHit(Board))
        {
            shouldExecute = true;
        }
        if (targetState is PreviewClearState && Previewable.ShouldPreviewClear(Board))
        {
            shouldExecute = true;
        }
        if (targetState is IdleState && !Previewable.ShouldPreviewHit(Board))
        {
            shouldExecute = true;
        }

        return shouldExecute;
    }

    
    private void StartAllStates()
    {
        foreach (IState state in states)
        {
            StartCoroutine(RunState(state));
        }
    }

    private IEnumerator RunState(IState state)
    {
        
        while (true)
        {
            if (CheckState(state))
            {
                yield return state.UpdateState(this);

            }
            else
            {
                yield return null;
            }
        }
    }


    
}

