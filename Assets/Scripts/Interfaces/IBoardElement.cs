using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardElement
{
    public int Column { get; set; }
    public int Row { get; set; }
    public void Debug(Color color);
    public void Debug();


}
