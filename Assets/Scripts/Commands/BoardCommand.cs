using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardCommand : Command
{
    public override CommandType CommandType => CommandType.Board;
    public override IEnumerator Execute(Board board)
    {


       
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(BoardCommand));
        onCommandExecuting?.Invoke(this);

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
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(BoardCommand));

            yield return new WaitForSeconds(0.85f);

        }

        yield return base.Execute(board);


    }

    
}