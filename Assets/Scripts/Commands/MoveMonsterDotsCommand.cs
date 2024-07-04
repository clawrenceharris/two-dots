using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Type;
public class MoveMonsterDotsCommand : MoveCommand
{
    public override CommandType CommandType => CommandType.MoveMonsterDots;
    private readonly Dictionary<MonsterDot, Dot> dotsToMoveTo = new();


    public override bool CanMove(Dot targetDot)
    {
        //if the dot is null or it is a monster dot 
        if (targetDot == null || targetDot is MonsterDot)
        {
            //then it is not avalaible to be moved to
            return false;
        }
        //if another monster dot is already moving to this dot
        if (dotsToMoveTo.ContainsValue(targetDot))
        {
            //then it is not avalaible to be moved to
            return false;
        }
        return true;
    }
    public override IEnumerator Execute(Board board)
    {
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveMonsterDotsCommand));

        List<MonsterDot> monsterDots = board.GetElements<MonsterDot>();
        List<NormalDot> replacementDots = new();
        int monsterDotCount = 0;

        if(monsterDots.Where((dot)=> !dot.WasHit).Count() > 0)
        {
            onCommandExecuting?.Invoke(this);

        }

        foreach (MonsterDot monsterDot in monsterDots)
        {
            Vector2Int targetDirection = GetRandomDirection(monsterDot, board);
            monsterDot.DirectionX = targetDirection.x;
            monsterDot.DirectionY = targetDirection.y;
            Dot dotToMoveTo = board.Get<Dot>(
                monsterDot.Column + monsterDot.DirectionX,
                monsterDot.Row + monsterDot.DirectionY);

            if (monsterDot.WasHit)
            {
                monsterDot.WasHit = false;
                continue;
            }

            //if the monster dot can move
            if (CanMove(dotToMoveTo))
            {
                DidExecute = true;

                //then add it to the dictionary
                dotsToMoveTo.TryAdd(monsterDot, dotToMoveTo);
            }
            
        }

        foreach (MonsterDot monsterDot in dotsToMoveTo.Keys)
        {
            if (dotsToMoveTo.TryGetValue(monsterDot, out var dotToMoveTo))
            {

                CoroutineHandler.StartStaticCoroutine(monsterDot.DoMove(() =>
                {
                    monsterDotCount++;

                    int startCol = monsterDot.Column;
                    int startRow = monsterDot.Row;
                    int endCol = dotToMoveTo.Column;
                    int endRow = dotToMoveTo.Row;

                    Dot dot = board.Get<Dot>(endCol, endRow); //get the dot to move to

                    board.Put(monsterDot, endCol, endRow);

                    board.Remove(monsterDot, startCol, startRow);

                    //Set dot data that will be used to spawn a dot at the monster's start position
                    DotsGameObjectData data = new(JSONLevelLoader.ToJsonDotType(DotType.NormalDot))
                    {
                        col = startCol,
                        row = startRow
                    };
                    data.SetProperty("Color", JSONLevelLoader.ToJsonColor(monsterDot.Color));

                    NormalDot repacementDot = board.InitDotsGameObject<NormalDot>(data);

                    replacementDots.Add(repacementDot);
                    board.DestroyDotsGameObject(dot);

                    monsterDot.Column = endCol;
                    monsterDot.Row = endRow;
                }));
            }
        }
        yield return new WaitUntil(() => monsterDotCount == dotsToMoveTo.Count);

        foreach(NormalDot dot in replacementDots)
        {
            dot.Select();
        }


        if (DidExecute)
        {

            Debug.Log(CommandInvoker.commandCount + " Executed " + nameof(MoveMonsterDotsCommand));

        }


        yield return base.Execute(board);
    }
}
