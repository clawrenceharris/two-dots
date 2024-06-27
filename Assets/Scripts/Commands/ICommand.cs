using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
using System;
public interface ICommand
{

    bool DidExecute { get; }
    CommandType CommandType { get; }
    IEnumerator Execute(Board board);



}
