using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeetleDotVisualController : ColorableDotVisualController, IDirectionalVisualController
{
    private List<GameObject> wingsLayer1;
    private List<GameObject> wingsLayer2;
    private List<GameObject> wingsLayer3;
    private List<List<GameObject>> wingLayers;
    private BeetleDot dot;
    private BeetleDotVisuals visuals;

    private int currentLayerIndex;
    private readonly DirectionalVisualController directionalVisualController = new();
    
    public override T GetGameObject<T>() => dot as T;
    
    public override T GetVisuals<T>() => visuals as T;
    

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BeetleDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BeetleDotVisuals>();
        directionalVisualController.Init(dot, visuals.directionalVisuals);
        base.Init(dotsGameObject);

    }

    protected override void SetUp()
    {
        wingsLayer1 = new() {
            visuals.leftWingLayer1,
            visuals.rightWingLayer1,
        };
        wingsLayer2 = new() {
            visuals.leftWingLayer2,
            visuals.rightWingLayer2,
        };
        wingsLayer3 = new() {
            visuals.leftWingLayer3,
            visuals.rightWingLayer3,
        };
        wingLayers = new()
        {
            wingsLayer1, wingsLayer2, wingsLayer3
        };
        

        currentLayerIndex = Mathf.Clamp(dot.HitCount, 0, dot.HitsToClear-1);

        UpdateRotation();
        RemoveLayers();
        base.SetUp();
    }

    public override void SetInitialColor()
    {
        Color currentColor = ColorSchemeManager.FromDotColor(dot.Color);

        SetColor(currentLayerIndex, currentColor);

        for (int i = currentLayerIndex + 1; i < wingLayers.Count; i++)
        {     
            SetColor(i, ColorSchemeManager.FromDotColor(dot.Colors[i]));    
        }


    }


    public override void SetColor(Color color)
    {
        SetColor(currentLayerIndex, color);
    }


    private void SetColor(int layer, Color color)
    {
        foreach (GameObject wing in wingLayers[layer])
        {
            if (wing.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.color = color;
            }
        }
    }

    public override void DisableSprites()
    {
        for (int i = 0; i < wingLayers.Count; i++)
        {
            for (int j = 0; j < wingLayers[i].Count; j++)
            {
                if (wingLayers[i][j].TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                {
                    spriteRenderer.enabled = false;
                }
            }

        }

        foreach(Transform child in visuals.rightWings)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = false;
            }
        }

        base.DisableSprites();
    }

    public override void EnableSprites()
    {
        for (int i = 0; i < wingLayers.Count; i++)
        {
            for (int j = 0; j < wingLayers[i].Count; j++)
            {
                if (wingLayers[i][j].TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                {
                    spriteRenderer.enabled = true;
                }
            }

        }

        foreach (Transform child in visuals.rightWings)
        {
            if (child.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.enabled = true;
            }
        }
        base.EnableSprites();
    }


    /// <summary>
    /// Based on the current layer, removes previous layers of wings off of the beetle dot
    /// </summary>
    /// <param name="duration">The duration of the animation</param>
    private void RemoveLayers(float duration = 0f)
    {
        for(int i = 0; i < currentLayerIndex; i++)
        {
            
            CoroutineHandler.StartStaticCoroutine(RemoveWingsCo(i, duration));
            if (i + 1 < wingLayers.Count)
            {
                wingLayers[i + 1][1].transform.parent = visuals.rightWings;
                wingLayers[i + 1][0].transform.parent = visuals.leftWings;
            }
        }
    }
    

    public override IEnumerator Hit(HitType hitType)
    {
        float hitDuration = visuals.hittableVisuals.HitDuration;
        currentLayerIndex = Mathf.Clamp(currentLayerIndex + 1, 0, dot.HitsToClear - 1);

        visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if(dot.HitCount == 3)
        {
            yield break;
        }

        RemoveLayers(hitDuration);
        yield return new WaitForSeconds(hitDuration);
    }

    public IEnumerator DoRotateAnimation()
    {

        Vector3 rotation = dot.GetRotation();
        yield return dot.transform.DOLocalRotate(rotation, visuals.rotationSpeed)
                    .SetEase(visuals.rotationEase);

    }

    
    


    private IEnumerator RemoveWingsCo(int layer, float duration)
    {
        float rotationDuration = duration / 2;
        GameObject leftWing = wingLayers[layer][0];
        GameObject rightWing = wingLayers[layer][1];
     
        Vector3 leftWingAngle = new(0, 0, -90);
        Vector3 rightWingAngle = new(0, 0, 90);

        rightWing.transform.SetParent(null);
        leftWing.transform.SetParent(null);
        leftWing.transform.DOLocalRotate(dot.transform.rotation.eulerAngles + leftWingAngle, rotationDuration);
        rightWing.transform.DOLocalRotate(dot.transform.rotation.eulerAngles + rightWingAngle, rotationDuration);
        
        rightWing.transform.DOMove(dot.transform.position + new Vector3(dot.DirectionY, -dot.DirectionX) * 1.7f, duration);
        leftWing.transform.DOMove(dot.transform.position + new Vector3(-dot.DirectionY, dot.DirectionX)  * 1.7f, duration);

        rightWing.transform.DOScale(Vector2.zero, duration);
        leftWing.transform.DOScale(Vector2.zero, duration);


        rightWing.GetComponent<SpriteRenderer>().DOFade(0, duration);
        leftWing.GetComponent<SpriteRenderer>().DOFade(0, duration);
        foreach(Transform child in rightWing.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in leftWing.transform)
        {
            child.gameObject.SetActive(false);
        }


        yield return new WaitForSeconds(duration);

        leftWing.SetActive(false);
        rightWing.SetActive(false);

    }
    
   
   
    public IEnumerator DoSwap(Dot dotToSwap)
    {
        float moveDuration = BeetleDotVisuals.moveDuration;
        int dotToSwapCol = dotToSwap.Column;
        int dotToSwapRow = dotToSwap.Row;
        int beetleDotCol = dot.Column;
        int beetleDotRow = dot.Row;
        var settings = new AnimationSettings{
            Duration = moveDuration
        };



       
        for (int i = 0; i < visuals.Sprites.Length; i++)
        {
            visuals.Sprites[i].sortingOrder += 100;

        }
        StartCoroutine(dotToSwap.VisualController.Animate(new SwapAnimation{
            Target = new Vector2(beetleDotCol, beetleDotRow) * Board.offset,
        }));
        StartCoroutine(Animate(new SwapAnimation{
            Target = new Vector2(dotToSwapCol, dotToSwapRow) * Board.offset
        }));
        yield return new WaitForSeconds(moveDuration);
        for (int i = 0; i < visuals.Sprites.Length; i++)
        {
            visuals.Sprites[i].sortingOrder -= 100;

        }

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
