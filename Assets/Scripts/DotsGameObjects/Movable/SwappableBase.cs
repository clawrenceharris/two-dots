using System;
using System.Collections;
using UnityEngine;

public class SwappableBase : ISwappable{
     public int DirectionX { get; set; }
    public int DirectionY { get; set; }
    private ISwappable swappable;
    public DotsGameObject DotsGameObject => (DotsGameObject)swappable;

    // public DotsGameObject Target {get; set;}

    public T GetGameObject<T>() where T : DotsGameObject => (T)swappable;
    
    public void Init(ISwappable swappable){
        this.swappable = swappable;
    }
    /// <summary>
    /// Finds the best direction that the dot can turn to in which the
    /// dot it is directed towards can be moved to
    /// </summary>
    /// <param name="t"></param>
    /// <param name="board"></param>
    /// <returns></returns>
   
   public bool IsValidTarget(DotsGameObject target, Board board)
    {
        // Check if the target dot is null or if it's another swappable dot
        if (target == null || target is ISwappable)
        {
            return false;
        }

        // Get neighboring beetle dots around the target
        var neighbors = target.FindNeighbors<ISwappable>(board);

        // Check if any neighboring beetle dots have the same target dot
        foreach (ISwappable neighbor in neighbors)
        {
            if (neighbor == swappable)
            {
                continue;
            }

            // If a neighboring beetle dot shares the same target, movement is blocked
            if (swappable.GetTarget(board) == target)  
            {
                return false;
            }
        }

        // No conflict found, the beetle can move to the target
        return true;
    }

    public IEnumerator TrySwap(Board board, Action<bool> onComplete)
    {
        return null;
    }
    
    public DotsGameObject FindNextTarget(Board board)
    {
        
        return null;
    }

    public Dot GetTarget(Board board){
        return null;
    }

    public void ReplaceTarget(Board board)
    {
        return;
    }

    public void ReplaceCurrent(Board board)
    {
        return;
    }
}