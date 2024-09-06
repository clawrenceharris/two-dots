using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LotusDotAnimation : DotsAnimationComponent{
    

    private LotusDot Dot => (LotusDot)DotsGameObject;

    private LotusDotVisuals Visuals => Dot.VisualController.GetVisuals<LotusDotVisuals>();
    private readonly AnimationCurve lotusCurve = new(
    
        new Keyframe(0f, 0f, 0f, 2.5f),        // Start point
        new Keyframe(0.2f, 1.08f, 0f, 2f),     // First peak bounce (higher and faster)
        new Keyframe(0.35f, 0.95f, 0f, 1.8f),  // Second smaller bounce, quickly back down
        new Keyframe(0.5f, 1.03f, 0f, 1.2f),   // Third quick bounce
        new Keyframe(0.65f, 0.98f, 0f, 1f),    // Fourth very small bounce
        new Keyframe(0.75f, 1.01f, 0f, 0.8f),  // Tiny bounce
        new Keyframe(1f, 1f)            
    
    );
    
    public override IEnumerator Idle()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));
        int direction = (Random.Range(0, 2) * 2) - 1;
        IEnumerator[] coroutines = { DoRotate90Animation(direction), DoRotate360Animation(direction) };
        int rand = Random.Range(0, coroutines.Length);

        yield return coroutines[rand];
        yield return new WaitForSeconds(Random.Range(2, 4));
    }

    

    private IEnumerator DoRotate360Animation(int direction)
    {
        Visuals.Layer1.transform.DOLocalRotate(new Vector3(0, 0, 180 * direction), 8f * 2, RotateMode.LocalAxisAdd)
    .SetEase(lotusCurve);
        Visuals.Layer2.transform.DOLocalRotate(new Vector3(0, 0, 180 * direction), 7f * 2, RotateMode.LocalAxisAdd)
    .SetEase(lotusCurve); 
        Visuals.Layer3.transform.DOLocalRotate(new Vector3(0, 0, 180 * direction), 6f *2, RotateMode.LocalAxisAdd)
    .SetEase(lotusCurve); 

        yield return new WaitForSeconds(8 * 2);
    }
   
     private IEnumerator DoRotate90Animation(int direction)
    {
        Visuals.Layer1.transform.DORotate(new Vector3(0,0, 90 * direction), 4f, RotateMode.LocalAxisAdd)
    .SetEase(lotusCurve); 
        
        Visuals.Layer2.transform.DORotate(new Vector3(0,0, -90 * direction), 4f, RotateMode.LocalAxisAdd)
    .SetEase(lotusCurve);
        Visuals.Layer3.transform.DORotate(new Vector3(0,0, 90 * direction), 4f, RotateMode.LocalAxisAdd)
    .SetEase(lotusCurve);
        yield return new WaitForSeconds(4);
    }

}