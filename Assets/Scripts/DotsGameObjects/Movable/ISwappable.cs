using System;
using System.Collections;

public interface ISwappable{

    // Dot Target {get;}
    IEnumerator TrySwap(Board board, Action<bool> onComplete);
    Dot GetTarget(Board board);

   
    bool IsValidTarget(DotsGameObject target, Board board);
}