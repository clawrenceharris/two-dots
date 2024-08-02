using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PreviewableStateManager : MonoBehaviour
{
    private Coroutine currentCoroutine;
    private Board board;
    private IState currentState;
    public DotsGameObject DotsGameObject {get; private set;}

    private PreviewHitState previewHitState;
    private PreviewClearState previewClearState;
    private IdleState idleState;

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
        board = GetComponentInParent<Board>();
    }

    private void Start()
    {
        previewClearState = GetComponent<PreviewClearState>();
        previewHitState = GetComponent<PreviewHitState>();
        idleState = GetComponent<IdleState>();
        ChangeState(idleState);
    }

    

    public void ChangeState(IState newState)
    {
        if(newState == null){
            return;
        }
        if(currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = null;
        currentState?.Exit(this);
        currentState = newState;
        currentState?.Enter(this);
    }

    private void OnConnectionChanged(){
        //change and execute previewing hit state on every connection made
     
        
    }
    private void Update(){
        if(Previewable.HitRule.Validate(Previewable, board)){
        
            ChangeState(new PreviewHitState());
        }
        else
            ChangeState(new PreviewClearState());
        
        
        if (currentState != null && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(currentState.Execute(this));
        }
        
    }
    
    

    
}

