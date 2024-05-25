using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;


public class BoardCommand : Command
{
    public override CommandType CommandType => CommandType.Board;

    
   
    private IEnumerator DropDots(Board board)
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

    }

    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(BoardCommand));

        yield return new BombCommand().Execute(board);
        yield return DropDots(board);



        CommandInvoker.Instance.Enqueue(new HitDotsCommand());
        CommandInvoker.Instance.Enqueue(new HitTilesCommand());

        CommandInvoker.Instance.Enqueue(new ExplosionCommand());





        yield return base.Execute(board);


    }
}