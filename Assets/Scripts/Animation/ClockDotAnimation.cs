using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using System;

public class ClockDotAnimation
{
    public delegate void AnimationCallback();
    public IEnumerator HitAnimation(Dot dot)
    {
        while (dot.HitType == HitType.Connection && ConnectionManager.ConnectedDots.Count > 1)
        {

            float elapsedTime = 0f;
            Vector3 originalRotation = dot.transform.eulerAngles;
            // Adjust these variables to control the shaking animation
            float shakeDuration = 0.6f;
            float shakeIntensity = 15f;
            float shakeSpeed = 20f;
            while (elapsedTime < shakeDuration)
            {
                // Calculate the amount to rotate by interpolating between -shakeIntensity and shakeIntensity
                float shakeAmount = Mathf.Sin(elapsedTime * shakeSpeed) * shakeIntensity;

                // Apply the rotation
                dot.transform.eulerAngles = originalRotation + new Vector3(0, 0, shakeAmount);

                // Increment the elapsed time
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            // Reset rotation to original position after the shaking animation is finished
            dot.transform.eulerAngles =Vector2.zero;

        }

        
    }
    

}
