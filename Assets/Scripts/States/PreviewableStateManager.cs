using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class PreviewableStateManager : MonoBehaviour
{
    private Coroutine currentCoroutine;
    public Board Board {get; set;}
    private IState currentState;
    public DotsGameObject DotsGameObject {get; private set;}

    public IPreviewable Previewable {
        
        get{
            if(DotsGameObject is IPreviewable previewable){
                return previewable;
            }
            else{
                throw new InvalidOperationException("The game object this script is attached to could not be converted to an IPreviewable."+
            " The game object must have a script that implmenets the IPreviewable interface.");
            }
        }
    }

    private void Awake(){
        DotsGameObject = GetComponent<DotsGameObject>();
        Board = FindObjectOfType<Board>();
        
    }

   
    private void OnDestroy(){
        StopAllCoroutines();
        
    }
     private List<IState> activeStates = new List<IState>();
private List<Coroutine> activeCoroutines = new List<Coroutine>();

private void Start()
{
    AddState(new IdleState());
   
}



private void EvaluateStates()
{
    // Clear out states that are no longer needed
    RemoveState<PreviewHitState>(() => !Previewable.ShouldPreviewHit(Board));
    RemoveState<PreviewClearState>(() => !Previewable.ShouldPreviewClear(Board));
    RemoveState<IdleState>(() => Previewable.ShouldPreviewHit(Board) || Previewable.ShouldPreviewClear(Board));
    
    // Add states based on current conditions
    if (Previewable.ShouldPreviewHit(Board) && !HasState<PreviewHitState>())
    {
        AddState(new PreviewHitState());
    }
    if (Previewable.ShouldPreviewClear(Board) && !HasState<PreviewClearState>())
    {
        AddState(new PreviewClearState());
    }
    if (!Previewable.ShouldPreviewHit(Board) && !Previewable.ShouldPreviewClear(Board) && !HasState<IdleState>())
    {
        AddState(new IdleState());
    }
}

private void AddState(IState newState)
{
    activeStates.Add(newState);
    Coroutine coroutine = StartCoroutine(newState.UpdateState(this));
    activeCoroutines.Add(coroutine);
}

private void RemoveState<T>(Func<bool> shouldRemove) where T : IState
{
   for (int i = activeStates.Count - 1; i >= 0; i--)
    {
        if (activeStates[i] is T && shouldRemove())
        {
            // Stop the associated coroutine
            StopCoroutine(activeCoroutines[i]);
            activeCoroutines.RemoveAt(i);
            
            // Remove the state
            activeStates.RemoveAt(i);
        }
    }
}

private bool HasState<T>() where T : IState
{
    return activeStates.Any(s => s is T);
}

private void Update()
{
    EvaluateStates();
   
}
    
    

    
}

