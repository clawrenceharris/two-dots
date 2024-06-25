using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
using System;

public class BeetleDotVisualController : ColorDotVisualController
{
    public new BeetleDotVisuals Visuals;
    protected new BeetleDot Dot;
    private List<GameObject> wingsLayer1;
    private List<GameObject> wingsLayer2;
    private List<GameObject> wingsLayer3;
    private List<GameObject> whiteWingsLayer1;
    private List<GameObject> whiteWingsLayer2;
    private List<GameObject> whiteWingsLayer3;
    private List<List<GameObject>> wingLayers;
    private List<List<GameObject>> whiteWingLayers;

    private int currentLayerIndex;
<<<<<<< Updated upstream
    private int wingsRemovedCount; //the amount of wings that were removed after set up
    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BeetleDotVisuals>();
        Dot = dot.GetComponent<BeetleDot>();
        base.Init(Dot);
=======

    public override T GetGameObject<T>()
    {
        return dot as T;
    }


    public override T GetVisuals<T>()
    {
        return visuals as T;
    }

    public override void Init(DotsGameObject dotsGameObject)
    {
        dot = (BeetleDot)dotsGameObject;
        visuals = dotsGameObject.GetComponent<BeetleDotVisuals>();
        spriteRenderer = dotsGameObject.GetComponent<SpriteRenderer>();
        SetUp();
>>>>>>> Stashed changes
    }

    protected override void SetUp()
    {
        wingsLayer1 = new() {
            Visuals.leftWingLayer1,
            Visuals.rightWingLayer1,
        };
        wingsLayer2 = new() {
            Visuals.leftWingLayer2,
            Visuals.rightWingLayer2,
        };
        wingsLayer3 = new() {
            Visuals.leftWingLayer3,
            Visuals.rightWingLayer3,
        };
        wingLayers = new()
        {
            wingsLayer1, wingsLayer2, wingsLayer3
        };
        whiteWingsLayer1 = new() {
            Visuals.leftWhiteWingLayer1,
            Visuals.rightWhiteWingLayer1,
        };
        whiteWingsLayer2 = new() {
            Visuals.leftWhiteWingLayer2,
            Visuals.rightWhiteWingLayer2,
        };
        whiteWingsLayer3 = new() {
            Visuals.leftWhiteWingLayer3,
            Visuals.rightWhiteWingLayer3,
        };
        wingLayers = new()
        {
            wingsLayer1, wingsLayer2, wingsLayer3
        };

        whiteWingLayers = new()
        {
            whiteWingsLayer1, whiteWingsLayer2, whiteWingsLayer3
        };
        currentLayerIndex = Mathf.Clamp(Dot.HitCount, 0, Dot.HitsToClear-1);

        Rotate();
<<<<<<< Updated upstream
        RemoveWings(0f);

=======
        RemoveWings();
>>>>>>> Stashed changes
        base.SetUp();
    }

    protected override void SetColor()
    {
<<<<<<< Updated upstream
        for(int i =0; i < Dot.Colors.Length; i++)
        {
            Color color = ColorSchemeManager.FromDotColor(Dot.Colors[i]);
            SetColor(i, color);
=======
        for (int i = 1; i < dot.Colors.Length; i++)
        {     
            SetColor(i, ColorSchemeManager.FromDotColor(dot.Colors[i]));    
>>>>>>> Stashed changes
        }
        Color color = ColorSchemeManager.FromDotColor(dot.Color);
        SetColor(currentLayerIndex, color);


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

        base.EnableSprites();
    }


    /// <summary>
    /// Based on the current layer, removes previous layers of wings off of the beetle dot
    /// </summary>
    /// <param name="duration">The duration of the animation</param>
    private void RemoveWings(float duration = 0.8f)
    {
        float removeDuration = 0.8f;
        for(int i = 0; i < currentLayerIndex; i++)
        {
            
            CoroutineHandler.StartStaticCoroutine(RemoveWingsCo(i, duration));
            if (i + 1 < wingLayers.Count)
            {
                wingLayers[i + 1][1].transform.parent = Visuals.rightWings;
                wingLayers[i + 1][0].transform.parent = Visuals.leftWings;
            }
        }
    }

    public override IEnumerator Clear()
    {
<<<<<<< Updated upstream
        float duration = Visuals.clearDuration;
=======
        bool isBombHit = dot.HitType == HitType.BombExplosion;
        float startFlapAngle = isBombHit ? 20f : 45f;
        float endFlapAngle = isBombHit ? 15f : 0;

        float duration = visuals.clearDuration;
>>>>>>> Stashed changes
        float elapsedTime = 0f;
        float amplitude = 1f;
        float frequency = 0.1f;
        float speed = 37f;
        Vector3 direction = new(Dot.DirectionX, Dot.DirectionY);

        Vector3 startPosition = Dot.transform.position;
        Vector3 unitDirection = direction.normalized;

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
            Vector3 movementDirection = (newPosition - Dot.transform.position).normalized;

            // Update the rotation of the dot to face the movement direction
            if (movementDirection != Vector3.zero)
            {
                Dot.transform.rotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            }

            Dot.transform.position = newPosition;


            CoroutineHandler.StartStaticCoroutine(FlapWings(startFlapAngle, endFlapAngle));
            yield return null;
        }
    }

<<<<<<< Updated upstream
    public override IEnumerator PreviewHit(HitType hitType)
    {
        bool isBombHit = hitType == HitType.BombExplosion;
        float flapDuration = 0.15f;
        float startFlapAngle = isBombHit ? 15f : 45f;
        float endFlapAngle = isBombHit ? 10f : 0;
=======

    private IEnumerator FlapWings(float startFlapAngle, float endFlapAngle)
    {
        float flapDuration = 0.16f;
        
>>>>>>> Stashed changes
        Vector3 leftWingStartAngle = new(0, 0, -startFlapAngle);
        Vector3 rightWingStartAngle = new(0, 0, startFlapAngle);

        Vector3 leftWingEndAngle = new(0, 0, -endFlapAngle);
        Vector3 rightWingEndAngle = new(0, 0, endFlapAngle);
<<<<<<< Updated upstream
        Debug.Log(ConnectionManager.ToHit.Contains(Dot));
        while (DotTouchIO.IsInputActive && ConnectionManager.ToHit.Contains(Dot) || Dot.HitCount >= Dot.HitsToClear)
        {
            // Flap up
            Visuals.leftWings.DOLocalRotate(leftWingStartAngle, flapDuration);
            Visuals.rightWings.DOLocalRotate(rightWingStartAngle, flapDuration);
=======
        
        // Flap up
        visuals.leftWings.DOLocalRotate(leftWingStartAngle, flapDuration);
        visuals.rightWings.DOLocalRotate(rightWingStartAngle, flapDuration);
>>>>>>> Stashed changes

        yield return new WaitForSeconds(flapDuration);

<<<<<<< Updated upstream
            // Flap down
            Visuals.leftWings.DOLocalRotate(leftWingEndAngle, flapDuration);
            Visuals.rightWings.DOLocalRotate(rightWingEndAngle, flapDuration);
            yield return new WaitForSeconds(flapDuration);
=======
        // Flap down
        visuals.leftWings.DOLocalRotate(leftWingEndAngle, flapDuration);
        visuals.rightWings.DOLocalRotate(rightWingEndAngle, flapDuration);
        yield return new WaitForSeconds(flapDuration);

        
       
    }


    public IEnumerator PreviewHit(HitType hitType)
    {
        float startFlapAngle = 45f;
        float endFlapAngle = 0f;
        while (DotTouchIO.IsInputActive && ConnectionManager.ToHit.Contains(dot) || dot.HitCount >= dot.HitsToClear) {

            yield return FlapWings(startFlapAngle, endFlapAngle);
>>>>>>> Stashed changes

        }
        Visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

        yield return base.PreviewHit(hitType);
    }

    public IEnumerator PreviewClear()
    {
        yield break;
    }


    
    public override IEnumerator Hit(HitType hitType)
    {
        yield return base.Hit(hitType);

        currentLayerIndex = Mathf.Clamp(currentLayerIndex + 1, 0, Dot.HitsToClear - 1);

        Visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

        RemoveWings();
    }

    public IEnumerator RotateCo()
    {

        Vector3 rotation = GetRotation();
        yield return Dot.transform.DOLocalRotate(rotation, Visuals.rotationSpeed)
                    .SetEase(Visuals.rotationEase);

    }

    private Vector3 GetRotation()
    {
        Vector3 rotation = Vector3.zero;
        if (Dot.DirectionY < 0)
        {
            rotation = new Vector3(0, 0, 180);
        }

        if (Dot.DirectionX < 0)
        {
            rotation = new Vector3(0, 0, 90);

        }
        if (Dot.DirectionX > 0)
        {
            rotation = new Vector3(0, 0, -90);

        }
        return rotation;
    }

    private void Rotate()
    {
        Vector3 rotation = GetRotation();
        Dot.transform.localRotation = Quaternion.Euler(rotation);
    }



    private IEnumerator RemoveWingsCo(int layer, float duration)
    {
        GameObject leftWing = wingLayers[layer][0];
        GameObject rightWing = wingLayers[layer][1];
        GameObject leftWhiteWing = whiteWingLayers[layer][0];
        GameObject rightWhiteWing = whiteWingLayers[layer][1];
        rightWhiteWing.SetActive(false);
        leftWhiteWing.SetActive(false);

        Vector3 leftWingAngle = new(0, 0, -90);
        Vector3 rightWingAngle = new(0, 0, 90);

        rightWing.transform.SetParent(null);
        leftWing.transform.SetParent(null);
        leftWing.transform.DOLocalRotate(Dot.transform.rotation.eulerAngles + leftWingAngle, duration);
        rightWing.transform.DOLocalRotate(Dot.transform.rotation.eulerAngles + rightWingAngle, duration);
        
        rightWing.transform.DOMove(Dot.transform.position + new Vector3(Dot.DirectionY, -Dot.DirectionX) * 2, duration);
        leftWing.transform.DOMove(Dot.transform.position + new Vector3(-Dot.DirectionY, Dot.DirectionX) * 2, duration);
        
        rightWing.GetComponent<SpriteRenderer>().DOFade(0, duration);
        leftWing.GetComponent<SpriteRenderer>().DOFade(0, duration);

        yield return new WaitForSeconds(duration);

        leftWing.SetActive(false);
        rightWing.SetActive(false);

    }

    public override IEnumerator BombHit()
    {
        SetColor(currentLayerIndex, ColorSchemeManager.CurrentColorScheme.bombLight);
<<<<<<< Updated upstream
        yield return new WaitForSeconds(DotVisuals.defaultClearDuration);
        SetColor(currentLayerIndex, ColorSchemeManager.FromDotColor(Dot.Color));
=======
        yield return new WaitForSeconds(HittableVisuals.defaultClearDuration);
        SetColor(currentLayerIndex, ColorSchemeManager.FromDotColor(dot.Color));
>>>>>>> Stashed changes
    }

    public IEnumerator DoSwap(Dot dotToSwap)
    {
        float moveSpeed = 0.4f;
        int dotToSwapCol = dotToSwap.Column;
        int dotToSwapRow = dotToSwap.Row;
        int beetleDotCol = Dot.Column;
        int beetleDotRow = Dot.Row;
        dotToSwap.transform.DOLocalMove(new Vector2(beetleDotCol, beetleDotRow) * Board.offset, moveSpeed);



         Dot.transform.DOLocalMove(new Vector2(dotToSwapCol, dotToSwapRow) * Board.offset, moveSpeed);
        

        yield return new WaitForSeconds(moveSpeed);

    }

    public IEnumerator TrySwap(Action callback)
    {
        float offset = 0.3f;

        //Set the small movement based on the beetle dot's current direction
        Vector2 currentDirection = new(Dot.DirectionX, Dot.DirectionY);
        Vector2 smallMovement = currentDirection * offset;

        //set the new local position
        Vector2 originalPosition = new Vector2(Dot.Column, Dot.Row) * Board.offset;
        Vector2 newPosition = originalPosition + smallMovement;

        Dot.transform.DOLocalMove(newPosition, 0.2f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                Dot.transform.DOLocalMove(originalPosition, 0.2f)
            .SetEase(Ease.OutCubic);
            });
        yield return new WaitForSeconds(0.5f);
        callback?.Invoke();


    }
}
