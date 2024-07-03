using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Type;
public class MoveMonsterDotsCommand : MoveCommand
{
    public override CommandType CommandType => CommandType.MoveMonsterDots;


    public override bool CanMove(Dot dot)
    {
        return dot != null && dot is not MonsterDot;
    }
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveMonsterDotsCommand));

        List<MonsterDot> monsterDots = board.GetElements<MonsterDot>();
        int monsterDotCount = 0;

        monsterDots = monsterDots.Where((dot) => !dot.WasHit).ToList();

        foreach (MonsterDot monsterDot in monsterDots)
        {
            Vector2Int targetDirection = GetRandomDirection(monsterDot, board);
            monsterDot.DirectionX = targetDirection.x;
            monsterDot.DirectionY = targetDirection.y;
            CoroutineHandler.StartStaticCoroutine(monsterDot.DoMove(() =>
            {
                DidExecute = true;
                monsterDotCount++;

                int startCol = monsterDot.Column;
                int startRow = monsterDot.Row;
                int endCol = targetDirection.x + monsterDot.Column;
                int endRow = targetDirection.y + monsterDot.Row;

               

                Dot dot = board.Get<Dot>(endCol, endRow);

                board.Put(monsterDot, endCol, endRow);
                board.DestroyDotsGameObject(dot);

                board.Remove(monsterDot, startCol, startRow);
                NormalDot repacementDot = board.InitDotsGameObject<NormalDot>(monsterDot.ReplacementDot);
                repacementDot.Select();
                monsterDot.Column = endCol;
                monsterDot.Row = endRow;
            }));
        }
        yield return new WaitUntil(() => monsterDotCount == monsterDots.Count);

        if (DidExecute)
        {
            onCommandExecuting?.Invoke(this);

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveMonsterDotsCommand));

        }


        yield return base.Execute(board);
    }
}
