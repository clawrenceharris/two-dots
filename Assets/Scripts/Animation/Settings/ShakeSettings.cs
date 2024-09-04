using DG.Tweening;
using Newtonsoft.Json;

public class ShakeSettings : AnimationSettings
{
    public int Vibrato { get; set; } = 10;  // Number of shakes
    public float Randomness { get; set; } = 90f;
    public bool Snapping = false;
    public bool FadeOut {get; set;} = true;
    
    public ShakeRandomnessMode ShakeRandomnessMode {get; set;} =  ShakeRandomnessMode.Harmonic;
    
}