using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MoveMonsterDotsCommand : MoveCommand
{
    public override CommandType CommandType => CommandType.MoveMonsterDots;

    //Dictionary that maps each movable monster dot to a dot to move to 
    private readonly Dictionary<MonsterDot, Dot> dotsToMoveTo = new();
    private int ongoingCoroutines;
    private readonly List<NormalDot> replacementDots = new();
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

    private void MoveMonsterDots(Board board){
        List<MonsterDot> monsterDots = board.FindElementsOfType<MonsterDot>();

        int monsterDotCount = 0;

    

        foreach (MonsterDot monsterDot in monsterDots)
        {
            Vector2Int targetDirection = GetRandomDirection(monsterDot, board);
            monsterDot.DirectionX = targetDirection.x;
            monsterDot.DirectionY = targetDirection.y;
            Dot dotToMoveTo = board.GetDotAt(
                monsterDot.Column + monsterDot.DirectionX,
                monsterDot.Row + monsterDot.DirectionY);

            if (monsterDot.WasHit == true)
            {
                monsterDot.WasHit = false;
                continue;

            }

            //if the monster dot can move add it to the dictionary
            else if (CanMove(dotToMoveTo))
            {
                DidExecute = true;

                
                dotsToMoveTo.TryAdd(monsterDot, dotToMoveTo);
            }

            
        }

        foreach (MonsterDot monsterDot in dotsToMoveTo.Keys)
        {
            if (dotsToMoveTo.TryGetValue(monsterDot, out var dotToMoveTo))
            {
                ongoingCoroutines++;
                CoroutineHandler.StartStaticCoroutine(monsterDot.DoMove(),() =>
                {
                    onCommandExecuting?.Invoke(this);
                    monsterDotCount++;

                    int startCol = monsterDot.Column;
                    int startRow = monsterDot.Row;
                    int endCol = dotToMoveTo.Column;
                    int endRow = dotToMoveTo.Row;

                    Dot dot = board.GetDotAt(endCol, endRow); //get the dot to move to



                    //Set dot data that will be used to spawn a dot at the monster's start position
                    DotsGameObjectData data = new(JSONLevelLoader.ToJsonDotType(DotType.NormalDot))
                    {
                        col = startCol,
                        row = startRow
                    };
                    data.SetProperty("Color", JSONLevelLoader.ToJsonColor(monsterDot.Color));

                    NormalDot replacementDot = board.InitDotsGameObject<NormalDot>(data);

                    replacementDots.Add(replacementDot);
                    board.DestroyDotsGameObject(dot);
                    board.Put(replacementDot, startCol, startRow);
                    board.Put(monsterDot, endCol, endRow);
                    monsterDot.Column = endCol;
                    monsterDot.Row = endRow;
                    ongoingCoroutines--;

                });
            }
        }
    }
    public override IEnumerator Execute(Board board)
    {

        if(!board.HasAny<MonsterDot>()){
            yield break;
        }   
        Debug.Log(CommandInvoker.commandCount + " Executing " + nameof(MoveMonsterDotsCommand));
        
        MoveMonsterDots(board);
        yield return new WaitUntil(() => ongoingCoroutines == 0);

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
