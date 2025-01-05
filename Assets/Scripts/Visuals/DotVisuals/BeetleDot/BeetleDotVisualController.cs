using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Animations;
using UnityEngine.UIElements;

public class BeetleDotVisualController : ColorableDotVisualController, IPreviewableVisualController
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
    private readonly PreviewableVisualController previewableVisualController = new();
    private readonly DirectionalVisualController directionalVisualController = new();
    
    public override T GetGameObject<T>() => dot as T;
    
    public override T GetVisuals<T>() => visuals as T;
    

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BeetleDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BeetleDotVisuals>();
        spriteManager = dotsGameObject.GetComponent<SpriteManager>();
        directionalVisualController.Init(this);
        previewableVisualController.Init(this);
        base.Init(dotsGameObject);

    }

    protected override void SetUp()
    {

        currentLayerIndex = Mathf.Clamp(dot.HitCount, 0, dot.HitsToClear-1);
        
        for(int i = 0; i < currentLayerIndex; i++)
            RemoveCurrentLayer();
        CoroutineHandler.StartStaticCoroutine(Rotate(dot.ToRotation(dot.DirectionX, dot.DirectionY)));
        base.SetUp();
    }

    
    public void RemoveCurrentLayer()
    {
        if(CurrentLayer == null){
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

        for (int i = currentLayerIndex; i < visuals.WingLayers.Count; i++)
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
        if(dot.HitCount >= 3){
            yield break;
        }
        currentLayerIndex = Mathf.Clamp(currentLayerIndex + 1, 0, dot.HitsToClear - 1);
        yield return base.Hit();
        RemoveCurrentLayer();
    }
   
   
    public IEnumerator DoSwap(DotsGameObject dotToSwap)
    {
        float moveDuration = BeetleDotVisuals.moveDuration;
        int dotToSwapCol = dotToSwap.Column;
        int dotToSwapRow = dotToSwap.Row;
        int beetleDotCol = dot.Column;
        int beetleDotRow = dot.Row;


       
        spriteManager.BringSpritesToTop();
        dotToSwap.StartCoroutine(dotToSwap.GetVisualController<IAnimatableVisualController>().Swap(new Vector2(beetleDotCol, beetleDotRow) * Board.offset));
        dot.StartCoroutine(Swap(new Vector2(dotToSwapCol, dotToSwapRow) * Board.offset));
        
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

    public IEnumerator PreviewClear()
    {
        yield return previewableVisualController.PreviewClear();
    }

    public IEnumerator PreviewHit()
    {
        yield return previewableVisualController.PreviewHit();
    }

    public IEnumerator Idle()
    {
        yield return previewableVisualController.Idle();
    }
}
