using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IHitRule
{
    public bool Validate(IHittable hittable, Board board);
}
