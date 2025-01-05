
using System.Collections;

/// <summary>
/// Represents a visual controller that controls the visuals of a hittable dots game object
/// </summary>
public interface IHittableVisualController{
    /// <summary>
    /// Performs visual hit updates to the object including starting the hit animation.
    /// </summary>
    IEnumerator Hit();
    
    /// <summary>
    /// Performs visual clear updates to the object including starting the clear animation.
    /// Uses an updated instance of predefined animation settings where the duration is set to the give duration value 
    /// </summary>
    /// <param name="duration">The duration of the animation</param>
    IEnumerator Clear(float duration);
  

}