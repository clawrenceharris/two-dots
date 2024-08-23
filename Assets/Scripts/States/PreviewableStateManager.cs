using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class PreviewableStateManager : MonoBehaviour
{
    private Coroutine currentCoroutine;

    public Board Board {get; set;}
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

    

    

    private bool HasState<T>() where T : IState
    {
        return states.Any(s => s is T);
    }

   
    private void AddState(IState newState)
    {
        // Ensure that only one coroutine runs at a time
        if (currentCoroutine == null)
        {
            // Optionally, stop the current coroutine if you want to interrupt the previous state
            StopCoroutine(currentCoroutine);
        }

        states.Add(newState);

        // Start the coroutine and track it
        
    }

    

    private void StartAllStates()
{
    foreach (IState state in states)
    {
        // Start a separate coroutine for each state
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

