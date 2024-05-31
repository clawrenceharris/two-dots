using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;

public class BombCommand : Command
{
    public override CommandType CommandType =>CommandType.Bomb;

    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(BombCommand));

        List<Dot> bombDots = board.GetBombDots();

        // Start clear coroutines for all bomb dots simultaneously
        foreach (Dot dot in bombDots)
        {
            if (dot && dot.IsBomb)
            {
                CoroutineHandler.StartStaticCoroutine(dot.Clear());
                board.RemoveDot(dot);
                board.CreateBomb(dot.Column, dot.Row);
                DidExecute = true;
            }
        }

        


        yield return base.Execute(board);

    }
}
