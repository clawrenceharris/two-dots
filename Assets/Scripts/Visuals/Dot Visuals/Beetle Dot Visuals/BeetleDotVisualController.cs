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
    private List<List<GameObject>> wingLayers;
    private int currentLayerIndex;
    private int wingsRemovedCount; //the amount of wings that were removed after set up
    public override void Init(Dot dot)
    {
        Visuals = dot.GetComponent<BeetleDotVisuals>();
        Dot = dot.GetComponent<BeetleDot>();
        base.Init(Dot);
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
        currentLayerIndex = Mathf.Clamp(Dot.HitCount, 0, Dot.HitsToClear-1);

        Rotate();
        RemoveWings();

        base.SetUp();
    }

    protected override void SetColor()
    {
        for(int i =0; i < Dot.Colors.Length; i++)
        {
            Color color = ColorSchemeManager.FromDotColor(Dot.Colors[i]);
            SetColor(i, color);
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
    private void RemoveWings()
    {
        float removeDuration = 0.8f;
        for(int i = 0; i < currentLayerIndex; i++)
        {
            
            CoroutineHandler.StartStaticCoroutine(RemoveWingsCo(wingLayers[i][0], wingLayers[i][1], removeDuration));
            if (i + 1 < wingLayers.Count)
            {
                wingLayers[i + 1][1].transform.parent = Visuals.rightWings;
                wingLayers[i + 1][0].transform.parent = Visuals.leftWings;
            }
        }



    }

    public override IEnumerator Clear()
    {
        float duration = Visuals.clearDuration;
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


            yield return null;
        }
    }

    public override IEnumerator PreviewHit(HitType hitType)
    {
        bool isBombHit = hitType == HitType.BombExplosion;
        float flapDuration = 0.15f;
        float startFlapAngle = isBombHit ? 15f : 45f;
        float endFlapAngle = isBombHit ? 10f : 0;
        Vector3 leftWingStartAngle = new(0, 0, -startFlapAngle);
        Vector3 rightWingStartAngle = new(0, 0, startFlapAngle);

        Vector3 leftWingEndAngle = new(0, 0,-endFlapAngle);
        Vector3 rightWingEndAngle = new(0, 0, endFlapAngle);

        while (DotTouchIO.IsInputActive && ConnectionManager.ToHit.Contains(Dot) || Dot.HitCount >= Dot.HitsToClear)
        {
            // Flap up
            Visuals.leftWings.DOLocalRotate(leftWingStartAngle, flapDuration);
            Visuals.rightWings.DOLocalRotate(rightWingStartAngle, flapDuration);

            yield return new WaitForSeconds(flapDuration);

            // Flap down
            Visuals.leftWings.DOLocalRotate(leftWingEndAngle, flapDuration);
            Visuals.rightWings.DOLocalRotate(rightWingEndAngle, flapDuration);
            yield return new WaitForSeconds(flapDuration);

        }
        Visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

        yield return base.PreviewHit(hitType);
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



    private IEnumerator RemoveWingsCo(GameObject leftWing, GameObject rightWing, float duration)
    {
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
        yield return new WaitForSeconds(DotVisuals.defaultClearDuration);
        SetColor(currentLayerIndex, ColorSchemeManager.FromDotColor(Dot.Color));
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
