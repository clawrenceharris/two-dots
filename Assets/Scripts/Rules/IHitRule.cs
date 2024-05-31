using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Type;
public interface IHitRule
{
    public bool Validate(IHittable hittable, Board board);
}
