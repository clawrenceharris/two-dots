using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class BombCommand : Command
{
    public override CommandType CommandType =>CommandType.Bomb;

    public override IEnumerator Execute(Board board)
    {
        List<Dot> bombDots = board.GetBombDots();
        List<Coroutine> clearCoroutines = new List<Coroutine>();

        // Start clear coroutines for all bomb dots simultaneously
        foreach (Dot dot in bombDots)
        {
            if (dot && dot.IsBomb)
            {
                Coroutine coroutine = CoroutineHandler.StartStaticCoroutine(dot.Clear());
                clearCoroutines.Add(coroutine);
                DidExecute = true;
            }
        }

        // Wait for all clear coroutines to finish
        foreach (Coroutine coroutine in clearCoroutines)
        {
            yield return coroutine;
        }

        // Create bombs after all dots have finished clearing
        foreach (Dot dot in bombDots)
        {
            if (dot && dot.IsBomb)
            {
                board.CreateBomb(dot.Column, dot.Row);
            }
        }


        yield return base.Execute(board);

    }
}
