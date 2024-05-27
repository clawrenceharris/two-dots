using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Type;
using static IDotAnimation;

public class DotAnimationManager : MonoBehaviour
{
    public static Dictionary<Dot, Coroutine> DotAnimations { get; private set; } = new();

    public void StartDotAnimation(Dot dot, IEnumerator coroutine)
    {
        // Check if the dot is already animating
        if (!DotAnimations.ContainsKey(dot))
        {
            Coroutine animationCoroutine = StartCoroutine(coroutine);
            DotAnimations.Add(dot, animationCoroutine);
        }


    }

    public void StopDotAnimation(Dot dot)
    {
        if (DotAnimations.ContainsKey(dot))
        {
            StopCoroutine(DotAnimations[dot]);
            DotAnimations.Remove(dot);
        }

    }

}
