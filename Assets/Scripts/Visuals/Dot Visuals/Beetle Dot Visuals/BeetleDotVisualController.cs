using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Type;
using System;
using Object = UnityEngine.Object;

public class BeetleDotVisualController : ColorDotVisualController
{
    public new BeetleDotVisuals Visuals;
    protected new BeetleDot Dot;
    private List<GameObject> wingsLayer1;
    private List<GameObject> wingsLayer2;
    private List<GameObject> wingsLayer3;
    private List<GameObject>[] wingLayers;
    private int currentLayerIndex;
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
        wingLayers = new[] { wingsLayer1, wingsLayer2, wingsLayer3 };
        currentLayerIndex = 0;

        Rotate();
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
        for (int i =0; i < wingLayers.Length; i++)
        {
            for(int j = 0; j < wingLayers[i].Count; j++)
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
        for (int i = 0; i < wingLayers.Length; i++)
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

    private IEnumerator DoHitAnimation()
    {
        Visuals.leftWings.DOLocalRotate(Vector3.zero, 0.1f);
        Visuals.rightWings.DOLocalRotate(Vector3.zero, 0.1f);
        if (Dot.HitCount == 1)
        {
            yield return RemoveWings(Visuals.rightWingLayer1, Visuals.leftWingLayer1);
            Visuals.rightWingLayer2.transform.parent = Visuals.rightWings;
            Visuals.leftWingLayer2.transform.parent = Visuals.leftWings;

        }
        if (Dot.HitCount == 2)
        {
            yield return RemoveWings(Visuals.rightWingLayer2, Visuals.leftWingLayer2);
            Visuals.rightWingLayer3.transform.parent = Visuals.rightWings;
            Visuals.leftWingLayer3.transform.parent = Visuals.leftWings;

        }
        

    }


    private IEnumerator DoCurvedTranslate(GameObject target, Vector3 direction, float duration)
    {
        float elapsedTime = 0f;
        float amplitude = Visuals.clearAmplitude;
        float frequency = Visuals.clearFrequeuncy;
        float speed = 20f;  // Adjust this as needed for your speed

        Vector3 startPosition = target.transform.position;
        Vector3 unitDirection = direction.normalized;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the progress based on elapsed time and duration
            float progress = elapsedTime / duration;

            // Calculate the linear displacement along the direction vector with respect to speed and progress
            Vector3 linearDisplacement = progress * speed * unitDirection;

            // Calculate the perpendicular displacement using the sine function
            Vector3 perpendicularDisplacement = amplitude * Mathf.Sin(progress * frequency * Mathf.PI * 2) * Vector3.Cross(unitDirection, Vector3.forward).normalized;

            // Update the position of the GameObject
            Vector3 newPosition = startPosition + linearDisplacement + perpendicularDisplacement;

            // Calculate the direction of the movement
            Vector3 movementDirection = (newPosition - target.transform.position).normalized;

            // Update the rotation of the GameObject to face the movement direction
            if (movementDirection != Vector3.zero)
            {
                target.transform.rotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            }

            target.transform.position = newPosition;


            yield return null;
        }
    }


    public override IEnumerator Clear()
    {
        Vector3 direction = new(Dot.DirectionX, Dot.DirectionY);

        yield return DoCurvedTranslate(Dot.gameObject, direction, 6f);
    }

    public override IEnumerator PreviewHit(PreviewHitType hitType)
    {
        float flapDuration = 0.15f;
        float flapAngle = 45f;

        Vector3 leftWingAngle = new(0, 0, -flapAngle);
        Vector3 rightWingAngle = new(0, 0, flapAngle);


        while (ConnectionManager.ToHit.Contains(Dot) || Dot.HitCount >= Dot.HitsToClear)
        {
            // Flap up
            Visuals.leftWings.DOLocalRotate(leftWingAngle, flapDuration);
            Visuals.rightWings.DOLocalRotate(rightWingAngle, flapDuration);

            yield return new WaitForSeconds(flapDuration);

            // Flap down
            Visuals.leftWings.DOLocalRotate(Vector3.zero, flapDuration);
            Visuals.rightWings.DOLocalRotate(Vector3.zero, flapDuration);
            yield return new WaitForSeconds(flapDuration);

        }
        Visuals.leftWings.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Visuals.rightWings.transform.localRotation = Quaternion.Euler(Vector3.zero);

        yield return base.PreviewHit(hitType);
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

    public override IEnumerator Hit(HitType hitType)
    {
        yield return DoHitAnimation();
        yield return base.Hit(hitType);
    }


    private IEnumerator RemoveWings(GameObject leftWing, GameObject rightWing)
    {
        Vector3 leftWingAngle = new(0, 0, 90);
        Vector3 rightWingAngle = new(0, 0, -90);

        rightWing.transform.SetParent(null);
        leftWing.transform.SetParent(null);
        leftWing.transform.DOLocalRotate(Dot.transform.rotation.eulerAngles + leftWingAngle, 0.8f);
        rightWing.transform.DOLocalRotate(Dot.transform.rotation.eulerAngles + rightWingAngle, 0.8f);
        

        rightWing.transform.DOMove(Dot.transform.position + new Vector3(-Dot.DirectionY, Dot.DirectionX) * 2, 0.8f);
        leftWing.transform.DOMove(Dot.transform.position + new Vector3(Dot.DirectionY, -Dot.DirectionX) * 2, 0.8f);

        rightWing.GetComponent<SpriteRenderer>().DOFade(0, 0.8f);
        leftWing.GetComponent<SpriteRenderer>().DOFade(0, 0.8f);
        yield return new WaitForSeconds(0.7f);
        Object.Destroy(leftWing);
        Object.Destroy(rightWing);
        wingLayers[currentLayerIndex].Remove(leftWing);
        wingLayers[currentLayerIndex].Remove(rightWing);




    }

    public override IEnumerator BombHit()
    {
        SetColor(currentLayerIndex, Color.white);
        yield return new WaitForSeconds(Visuals.clearTime);
        SetColor(currentLayerIndex, Color);
    }

    public IEnumerator DoSwap(Dot dotToSwap)
    {
        float moveSpeed = Visuals.moveSpeed;
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
