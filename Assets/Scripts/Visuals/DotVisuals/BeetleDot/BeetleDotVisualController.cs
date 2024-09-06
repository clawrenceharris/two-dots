using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeetleDotVisualController : ColorableDotVisualController, IDirectionalVisualController
{
    private List<GameObject> wingsLayer1;
    private List<GameObject> wingsLayer2;
    private List<GameObject> wingsLayer3;
    private BeetleDot dot;
    private SpriteManager spriteManager;
    private BeetleDotVisuals visuals;
    private BeetleDotVisuals.WingLayer CurrentLayer {
        get{
            if(visuals.WingLayers.Count > 0){
                return visuals.WingLayers[^1];
            }
            return null;
        }
    }
    private int currentLayerIndex;
    private readonly DirectionalVisualController directionalVisualController = new();
    
    public override T GetGameObject<T>() => dot as T;
    
    public override T GetVisuals<T>() => visuals as T;
    

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BeetleDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BeetleDotVisuals>();
        spriteManager = dotsGameObject.GetComponent<SpriteManager>();
        directionalVisualController.Init(dot, visuals.directionalVisuals);
        base.Init(dotsGameObject);

    }

    protected override void SetUp()
    {

        currentLayerIndex = Mathf.Clamp(dot.HitCount, 0, dot.HitsToClear-1);

        UpdateRotation();
        
        for(int i = 0; i < currentLayerIndex; i++)
            RemoveCurrentLayer();
        
        base.SetUp();
    }
    public void RemoveCurrentLayer()
    {
        if(visuals.WingLayers.Count <= 1){
            return;
        }
        //destroy both wings
        Object.Destroy(CurrentLayer.LeftWing.gameObject);
        Object.Destroy(CurrentLayer.RightWing.gameObject);

        //remove the current layer (last layer in layers list)
        visuals.WingLayers.Remove(CurrentLayer);

        //Reset the transform parent of newly changed the current layer to the active transform
        CurrentLayer.LeftWing.transform.SetParent(visuals.ActiveLeftWings);
        CurrentLayer.RightWing.transform.SetParent(visuals.ActiveRightWings);
    }

    public override void SetInitialColor()
    {
        Color currentColor = ColorSchemeManager.FromDotColor(dot.Color);

        SetColor(currentColor);

        for (int i = currentLayerIndex + 1; i < visuals.WingLayers.Count; i++)
        {     
            visuals.WingLayers[i].LeftWing.color = ColorSchemeManager.FromDotColor(dot.Colors[i]);    
            visuals.WingLayers[i].RightWing.color = ColorSchemeManager.FromDotColor(dot.Colors[i]);    
        }


    }


    public override void SetColor(Color color)
    {
        if(CurrentLayer != null){
            CurrentLayer.LeftWing.color = color;
            CurrentLayer.RightWing.color = color;
        }
    }


    public override void DisableSprites()
    {
        for (int i = 0; i < visuals.WingLayers.Count; i++)
        {
            
            visuals.WingLayers[i].LeftWing.enabled = false;
            visuals.WingLayers[i].RightWing.enabled = false;    

        }
        base.DisableSprites();
    }

    public override void EnableSprites()
    {
        for (int i = 0; i < visuals.WingLayers.Count; i++)
        {
            
            visuals.WingLayers[i].LeftWing.enabled = true;
            visuals.WingLayers[i].RightWing.enabled = true;    

        }
        base.EnableSprites();
    }


   

    public override IEnumerator Hit()
    {
        yield return base.Hit();
        currentLayerIndex = Mathf.Clamp(currentLayerIndex + 1, 0, dot.HitsToClear - 1);
        RemoveCurrentLayer();

    }

    public IEnumerator DoRotateAnimation()
    {

        Vector3 rotation = dot.GetRotation();
        yield return dot.transform.DOLocalRotate(rotation, visuals.rotationSpeed)
                    .SetEase(visuals.rotationEase);

    }

    
    


      
   
   
    public IEnumerator DoSwap(Dot dotToSwap)
    {
        dot.Debug(Color.red);
        float moveDuration = BeetleDotVisuals.moveDuration;
        int dotToSwapCol = dotToSwap.Column;
        int dotToSwapRow = dotToSwap.Row;
        int beetleDotCol = dot.Column;
        int beetleDotRow = dot.Row;
        var settings = new AnimationSettings{
            Duration = moveDuration
        };



       
        spriteManager.BringSpritesToTop();
        StartCoroutine(dotToSwap.VisualController.Animate(new SwapAnimation{
            Target = new Vector2(beetleDotCol, beetleDotRow) * Board.offset,
        }));
        StartCoroutine(Animate(new SwapAnimation{
            Target = new Vector2(dotToSwapCol, dotToSwapRow) * Board.offset
        }));
        yield return new WaitForSeconds(moveDuration);
        spriteManager.BringSpritesBack();


    }

    public IEnumerator TrySwap()
    {
        float offset = 0.3f;
        //Set the small movement based on the beetle dot's current direction
        Vector2 currentDirection = new(dot.DirectionX, dot.DirectionY);
        Vector2 smallMovement = currentDirection * offset;

        //set the new local position
        Vector2 originalPosition = new Vector2(dot.Column, dot.Row) * Board.offset;
        Vector2 newPosition = originalPosition + smallMovement;

        dot.transform.DOLocalMove(newPosition, 0.2f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                dot.transform.DOLocalMove(originalPosition, 0.2f)
            .SetEase(Ease.OutCubic);
            });
        yield return new WaitForSeconds(0.5f);


    }

    public void UpdateRotation()
    {
        directionalVisualController.UpdateRotation();
    }
}
