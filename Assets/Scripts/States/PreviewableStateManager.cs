using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PreviewableStateManager : MonoBehaviour
{
    private IEnumerator currentCoroutine;
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

    private void Start()
    {
        StartCoroutine(UpdateState());    
    }

    private void OnDestroy(){
        StopAllCoroutines();
        
    }

    public void ChangeState(IState newState)
    {
       
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = null;

        currentState = newState;
    }
    

    private IEnumerator UpdateState(){
        while(true){
            EvaluateState();
            if (currentState != null && currentCoroutine == null){
                
                currentCoroutine = currentState.UpdateState(this);
                yield return currentCoroutine;
            }
        }
        
    }
    
   
    private void EvaluateState()
    {
        // Evaluate all states and select the highest priority one
        IState newState;
        
        if (Previewable.ShouldPreviewHit(Board))
        {
            newState = new PreviewHitState();
        }
        else if (Previewable.ShouldPreviewClear(Board))
        {
            newState = new PreviewClearState();
        }
        else 
        {
            newState = new IdleState();
        }

        
        ChangeState(newState);
        
    }
    

    
}

