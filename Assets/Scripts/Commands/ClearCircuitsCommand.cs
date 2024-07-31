using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClearCircuitsCommand : Command
{
    public override CommandType CommandType => CommandType.ClearCircuits;


    public override IEnumerator Execute(Board board)
    {
        List<Circuit> visitedCircuits = new();
        List<Circuit> allCircuits = board.FindTilesOfType<Circuit>();
        foreach(Circuit circuit in allCircuits){
            if(visitedCircuits.Contains(circuit)){
                continue;
            }

            else{
                visitedCircuits.Add(circuit);
                List<Circuit> circuitGroup = new();
                circuitGroup = board.FindGroupOfType(circuit);
                
                //circuits should be cleared if all circuits in the group are active
                bool shouldClear = circuitGroup.All((circuit)=> circuit.IsActive);  
                Debug.Log("Count: " + circuitGroup.Count);
                foreach(Circuit circuitToClear in circuitGroup){
                    visitedCircuits.Add(circuit);
                    if(shouldClear)
                    {
                        Dot dotToRemove = board.GetDotAt(circuit.Column, circuit.Row);
                        CoroutineHandler.StartStaticCoroutine(dotToRemove.Clear(0));
                        CoroutineHandler.StartStaticCoroutine(circuitToClear.Clear());
                        
                        board.SpawnBomb(circuit.Column, circuit.Row);
                    }    
                
                }
            }
        }
        yield return base.Execute(board);
    }
}
