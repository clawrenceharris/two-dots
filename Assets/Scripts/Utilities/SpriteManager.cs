using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField]private int baseSortingOrder = 0;
    private DotsGameObject dotsGameObject;
    private SpriteRenderer sprite;
    
    private Visuals Visuals => dotsGameObject.VisualController.GetVisuals<Visuals>();
    public void Awake(){
        dotsGameObject = GetComponent<DotsGameObject>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        SetSortingOrder();
    }
    private void SetSortingOrder()
    {
        foreach (var sprite in Visuals.Sprites)
        {
            if(sprite)
                sprite.sortingOrder += baseSortingOrder;
        }
    }

   

    public void BringSpriteToTop()
    {
        sprite.sortingOrder += 100;
    }

    public void BringSpriteBack(){
        sprite.sortingOrder -= 100;
    }
    public void BringSpritesToTop()
    {
        foreach (var sprite in Visuals.Sprites)
        {
            if(sprite)
                sprite.sortingOrder += 100;
        }
    }
    public void BringSpritesBack()
    {
        foreach (var sprite in Visuals.Sprites)
        {
            if(sprite)
                sprite.sortingOrder -= 100;
        }
    }
}
