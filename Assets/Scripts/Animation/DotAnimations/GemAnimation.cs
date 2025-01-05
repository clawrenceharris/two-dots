using System.Collections;
using DG.Tweening;
using UnityEngine;
using Animations;
public class GemAnimation : DotAnimation,
IClearPreviewable,
IShakable

{

    [SerializeField]private ShakeSettings shakeSettings;
    ShakeSettings IShakable.Settings => shakeSettings;

    IEnumerator IClearPreviewable.Animate(PreviewClearAnimation animation)
    {
        IShakable shakable= this;
        yield return shakable.Animate(new ShakeAnimation{
            Settings = shakeSettings
        });
        
        // ShakeSettings settings = new(){
        //         Vibrato = 10,
        //         Duration = 2f,
        //         Ease = Ease.OutQuad,
        //         FadeOut = false
                     
        // };
        
    }
    
    IEnumerator IShakable.Animate(ShakeAnimation animation)
    {
        animation.Settings = shakeSettings;
        yield return GetAnimatable<ShakableBase>().Animate(animation);
    }
}