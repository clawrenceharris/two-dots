using System.Collections;
using System.Collections.Generic;

public interface IColorable
{
    DotColor Color { get; set; }
    void Select();
}
