using System;
using UnityEngine;
using System.Collections.Generic;

public class NumerableBase : MonoBehaviour, INumerable
{

    public int CurrentNumber { get; private set; }
    public int TempNumber { get; set; } = -1;
    public int InitialNumber { get; set; }
    private INumerable numerable;
    private DotsGameObject dotsGameObject;

    public bool TryGetGameObject<T>(out T gameObject) where T : class{ 
        gameObject = default;
        if(dotsGameObject is T t){
            gameObject = t;
            return true;
        }

        
        return false;
    }
    public INumerableVisualController VisualController => dotsGameObject.GetVisualController<INumerableVisualController>();
     
   

    public void Init(INumerable numerable)
    {
        this.numerable = numerable;
        dotsGameObject = GetComponent<DotsGameObject>();

        UpdateCurrentNumber(InitialNumber);
        ConnectionManager.onDotConnected += OnConnectionChanged;
        ConnectionManager.onDotDisconnected += OnConnectionChanged;
    }

    public void OnDestroy(){
        ConnectionManager.onDotConnected -= OnConnectionChanged;
        ConnectionManager.onDotDisconnected -= OnConnectionChanged;
    }

    public void UpdateNumberVisuals(int number)
    {
        
        VisualController.UpdateNumbers(number);
        
    }

    public void UpdateCurrentNumber(int number)
    {
        CurrentNumber = number;
        VisualController.UpdateNumbers(number);
    }



    public void OnConnectionChanged(ConnectableDot dot){
        if(TryGetGameObject<ConnectableDot>(out var connectableDot)){
            //if the dot is not in connection
            if(!ConnectionManager.ToHit.Contains(connectableDot)){

                //go back to original number 
                UpdateNumberVisuals(numerable.CurrentNumber);
                return;
            }
            // if dot is in connection and it is the only one in the connection 
            if(ConnectionManager.ConnectedDots.Count == 1){
                
                //go back to to the original number
                UpdateNumberVisuals(numerable.CurrentNumber);
                return;
            }
            List<IHittable> toHit = ConnectionManager.ToHit;
            
            numerable.TempNumber = Mathf.Clamp(numerable.CurrentNumber - toHit.Count, 0, int.MaxValue);
            
            UpdateNumberVisuals(numerable.TempNumber);
            CoroutineHandler.StartStaticCoroutine(VisualController.ScaleNumbers());
        }

        
        
    }


    public void Hit(HitType hitType)
    {

        if (hitType.IsExplosion())
        {
            //set current number to be one less than the current number
            TempNumber = Mathf.Clamp(CurrentNumber - 1, 0, 99);
            UpdateCurrentNumber(TempNumber);
            return;
        };
        UpdateCurrentNumber(TempNumber);

    }

}