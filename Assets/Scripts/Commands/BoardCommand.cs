using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;


public class BoardCommand : Command
{
    public override CommandType CommandType => CommandType.Board;
    public override IEnumerator Execute(Board board)
    {

        onCommandExecuting?.Invoke(this);

        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(BoardCommand));

        bool dotsDropped;
        do
        {
            dotsDropped = board.CollapseColumn();


            DidExecute = dotsDropped || DidExecute;

        } while (dotsDropped);
        dotsDropped = board.FillBoard();
        DidExecute = dotsDropped || DidExecute;

        if (DidExecute)
        {
            yield return new WaitForSeconds(Board.DotDropSpeed);
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(BoardCommand));

            CommandInvoker.Instance.Enqueue(new HitCommand());

        }

        yield return base.Execute(board);


    }
}