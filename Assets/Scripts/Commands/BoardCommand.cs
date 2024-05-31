using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;


public class BoardCommand : Command
{
    public override CommandType CommandType => CommandType.Board;

    
   
    

    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(BoardCommand));


        bool dotsDropped;
        do
        {
            dotsDropped = board.CollapseColumn();


            DidExecute = dotsDropped || DidExecute;

        } while (dotsDropped);
        dotsDropped = board.FillBoard();
        DidExecute = dotsDropped || DidExecute;
        yield return new WaitForSeconds(DidExecute ? Board.DotDropSpeed : 0f);




        yield return base.Execute(board);


    }
}