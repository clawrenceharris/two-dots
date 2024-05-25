using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager
{   
    public static event OnMoveMade onMoveMade;
    public delegate void OnMoveMade();

    public static event OnNoMoves onNoMoves;
    public delegate void OnNoMoves();

    public int moves {get; private set;}
    
    public void SetMoves(int moves){
        this.moves = moves;
    }

    // Start is called before the first frame update
    public  MoveManager()
    {
        ConnectionManager.onConnectionEnded += OnConnectionEnded;
        
    }

    private void OnConnectionEnded(LinkedList<ConnectableDot> dots){
        moves -= 1;
        onMoveMade?.Invoke();

        if(moves == 0){
            onNoMoves?.Invoke();
        }

    }



 
}
