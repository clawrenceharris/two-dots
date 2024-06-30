
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CoroutineHandler : MonoBehaviour
{
    private static CoroutineHandler instance;

   
    public static IEnumerator WaitForAll(List<IEnumerator> coroutines)
    {
        List<Coroutine> runningCoroutines = new List<Coroutine>();
        foreach (IEnumerator coroutine in coroutines)
        {
            runningCoroutines.Add(instance.StartCoroutine(coroutine));
        }

        foreach (Coroutine coroutine in runningCoroutines)
        {
            yield return coroutine;
        }

        coroutines.Clear();
    }

    private void Awake()
    {
        // Ensure only one instance of CoroutineHandler exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

 
    public static Coroutine StartStaticCoroutine(IEnumerator coroutine, Action callback = null)
    {
        return instance.StartCoroutine(RunCoroutine(coroutine, callback));
    }

    // Wrapper coroutine that runs the original coroutine and then invokes the callback
    private static IEnumerator RunCoroutine(IEnumerator coroutine, Action callback)
    {
        yield return coroutine;
        callback?.Invoke();
    }

    public static void StopStaticCoroutine(Coroutine coroutine)
    {
        instance.StopCoroutine(coroutine);
    }


}
