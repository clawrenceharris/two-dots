using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusDotVisuals : DotVisuals
{
    public SpriteRenderer Layer1;
    public SpriteRenderer Layer2;
    public SpriteRenderer Layer3;
    public SpriteRenderer Layer4;
    
    public readonly AnimationCurve LotusBounceCurve = new(
    
        new Keyframe(0f, 0f, 0f, 2.5f),        // Start point
        new Keyframe(0.2f, 1.08f, 0f, 2f),     // First peak bounce (higher and faster)
        new Keyframe(0.35f, 0.95f, 0f, 1.8f),  // Second smaller bounce, quickly back down
        new Keyframe(0.5f, 1.03f, 0f, 1.2f),   // Third quick bounce
        new Keyframe(0.65f, 0.98f, 0f, 1f),    // Fourth very small bounce
        new Keyframe(0.75f, 1.01f, 0f, 0.8f),  // Tiny bounce
        new Keyframe(1f, 1f)            
    
    );
    

}
