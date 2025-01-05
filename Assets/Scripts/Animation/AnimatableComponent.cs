

using Animations;
using UnityEngine;

public class AnimatableComponent : MonoBehaviour, IAnimatable
{
   
    protected T GetVisuals<T>() where T : class => DotsGameObject.VisualController.GetVisuals<T>();
    
    protected T GetGameObject<T>() where T : DotsGameObject => (T)DotsGameObject;
    
    protected T GetAnimatable<T>() where T : AnimatableBase, new(){
        T animatable = new();
        
        animatable.Init(this);

        
        return animatable;
    }
    
    private DotsGameObject dotsGameObject;
    public DotsGameObject DotsGameObject{
        get{
            if(dotsGameObject == null){
                if(TryGetComponent(out DotsGameObject dotsGameObject)){
                    this.dotsGameObject = dotsGameObject;
                }
                else{
                    this.dotsGameObject = GetComponentInParent<DotsGameObject>();
                }
            }
            return dotsGameObject;
        }
    }


    private void OnDestroy(){
        StopAllCoroutines();
    }
   

}