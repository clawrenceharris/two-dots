
using static Type;
using Color = UnityEngine.Color;
using System.Collections;
using System.Collections.Generic;

public class NormalDot : ConnectableDot, IColorable
{
    private DotColor color;
    public DotColor Color { get => color; set => color = value; }
    public override DotType DotType => DotType.NormalDot;

    public override int HitsToClear => 1;

    public override void Connect()
    {
        if (visualController is ConnectableDotVisualController cVisualController)

            cVisualController.AnimateSelectionEffect();

    }
    
    public override void Disconnect()
    {
        //do nothing
    }

   public override void InitDisplayController()
    {
        visualController = new ColorDotVisualController();
        visualController.Init(this);
    }


    

   
    public override void Select()
    {
        if (visualController is ConnectableDotVisualController cVisualController)

            cVisualController.AnimateSelectionEffect();

    }



    public override IEnumerator Hit(HitType hitType)
    {
        HitCount++;

        yield return base.Hit(hitType);
    }

    
}
