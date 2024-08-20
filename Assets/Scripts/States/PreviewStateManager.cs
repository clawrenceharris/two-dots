using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



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

    private void Start()
    {
        ChangeState(new IdleState());
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
    public void Update(){
        if (currentState != null && currentCoroutine == null){
            
            currentCoroutine = StartCoroutine(currentState.UpdateState(this));
        }
    }
    
   
    
    

    
}

