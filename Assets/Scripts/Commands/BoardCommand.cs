using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;


public class BoardCommand : Command
{
    public override CommandType CommandType => CommandType.Board;

    
   
    

    public override IEnumerator Execute(Board board)
    {


        bool dotsDropped;
        do
        {
            dotsDropped = board.CollapseColumn();


            DidExecute = dotsDropped || DidExecute;

        } while (dotsDropped);
        dotsDropped = board.FillBoard();
        DidExecute = dotsDropped || DidExecute;
        yield return new WaitForSeconds(DidExecute ? Board.DotDropSpeed : 0f);


        if (DidExecute)
        {
            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(BoardCommand));

        }
        
        yield return base.Execute(board);


    }
}