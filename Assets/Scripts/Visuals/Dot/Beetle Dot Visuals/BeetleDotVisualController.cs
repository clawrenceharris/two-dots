using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
using System;

public class BeetleDotVisualController : ColorableDotVisualController, IPreviewable, IDirectionalVisualController
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

        Rotate();
        RemoveWings();
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

    public override IEnumerator AnimateSelectionEffect()
    {
        return base.AnimateSelectionEffect();
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
    private void RemoveWings(float duration = 0f)
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

    public override IEnumerator DoClearAnimation()
    {
        bool isBombHit = dot.HitType == HitType.BombExplosion;
        float startFlapAngle = isBombHit ? 20f : 45f;
        float endFlapAngle = isBombHit ? 15f : 0;

        float duration = visuals.hittableVisuals.ClearDuration;
        float elapsedTime = 0f;
        float amplitude = 1f;
        float frequency = 0.1f;
        float speed = 40f;
        Vector3 direction = new(dot.DirectionX, dot.DirectionY);

        Vector3 startPosition = dot.transform.position;
        Vector3 unitDirection = direction.normalized;

        CoroutineHandler.StartStaticCoroutine(base.DoClearAnimation());

        while (elapsedTime < duration * speed)
        {
            
            elapsedTime += Time.deltaTime * speed;

            // Calculate the progress based on elapsed time and duration
            float progress = elapsedTime / duration;

            // Calculate the linear displacement along the direction vector with respect to speed and progress
            Vector3 linearDisplacement = progress * unitDirection;

            // Calculate the perpendicular displacement using the sine function
            Vector3 perpendicularDisplacement = amplitude * Mathf.Sin(progress * frequency * Mathf.PI * 2) * Vector3.Cross(unitDirection, Vector3.forward).normalized;

            Vector3 newPosition = startPosition + linearDisplacement + perpendicularDisplacement;

            // Calculate the direction of the movement
            Vector3 movementDirection = (newPosition - dot.transform.position).normalized;

            // Update the rotation of the dot to face the movement direction
            if (movementDirection != Vector3.zero)
            {
                dot.transform.rotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            }

            dot.transform.position = newPosition;


            CoroutineHandler.StartStaticCoroutine(FlapWings(startFlapAngle, endFlapAngle));
            yield return null;
        }
    }



    private IEnumerator FlapWings(float startFlapAngle, float endFlapAngle)
    {
        float flapDuration = 0.16f;
        
        Vector3 leftWingStartAngle = new(0, 0, -startFlapAngle);
        Vector3 rightWingStartAngle = new(0, 0, startFlapAngle);

        Vector3 leftWingEndAngle = new(0, 0, -endFlapAngle);
        Vector3 rightWingEndAngle = new(0, 0, endFlapAngle);
        
        // Flap up
        visuals.leftWings.DOLocalRotate(leftWingStartAngle, flapDuration);
        visuals.rightWings.DOLocalRotate(rightWingStartAngle, flapDuration);

        yield return new WaitForSeconds(flapDuration);

        // Flap down
        visuals.leftWings.DOLocalRotate(leftWingEndAngle, flapDuration);
        visuals.rightWings.DOLocalRotate(rightWingEndAngle, flapDuration);
        yield return new WaitForSeconds(flapDuration);  
    }


    public IEnumerator PreviewHit(PreviewHitType hitType)
    {
        float startFlapAngle = 45f;
        float endFlapAngle = 0f;
        while (DotTouchIO.IsInputActive && ConnectionManager.ToHit.Contains(dot) || dot.HitCount >= dot.HitsToClear) {

            yield return FlapWings(startFlapAngle, endFlapAngle);

        }
        visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

    }

    public IEnumerator PreviewClear()
    {
        yield break;
    }


    
    public override IEnumerator DoHitAnimation(HitType hitType)
    {
        float hitDuration = HittableVisuals.hitDuration;
        currentLayerIndex = Mathf.Clamp(currentLayerIndex + 1, 0, dot.HitsToClear - 1);

        visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

        if(dot.HitCount == 3)
        {
            yield break;
        }

        RemoveWings(hitDuration);
        yield return new WaitForSeconds(hitDuration);
    }

    public IEnumerator DoRotateAnimation()
    {

        Vector3 rotation = GetRotation();
        yield return dot.transform.DOLocalRotate(rotation, visuals.rotationSpeed)
                    .SetEase(visuals.rotationEase);

    }

    private Vector3 GetRotation()
    {
        Vector3 rotation = Vector3.zero;
        if (dot.DirectionY < 0)
        {
            rotation = new Vector3(0, 0, 180);
        }

        if (dot.DirectionX < 0)
        {
            rotation = new Vector3(0, 0, 90);

        }
        if (dot.DirectionX > 0)
        {
            rotation = new Vector3(0, 0, -90);

        }
        return rotation;
    }

    private void Rotate()
    {
        Vector3 rotation = GetRotation();
        dot.transform.localRotation = Quaternion.Euler(rotation);
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

        for (int i = 0; i < visuals.sprites.Length; i++)
        {
            visuals.sprites[i].sortingOrder += 100;

        }

        dotToSwap.transform.DOLocalMove(new Vector2(beetleDotCol, beetleDotRow) * Board.offset, moveDuration);
        dot.transform.DOLocalMove(new Vector2(dotToSwapCol, dotToSwapRow) * Board.offset, moveDuration);
        

        yield return new WaitForSeconds(moveDuration);

        for (int i = 0; i < visuals.sprites.Length; i++)
        {
            visuals.sprites[i].sortingOrder -= 100;

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
}
