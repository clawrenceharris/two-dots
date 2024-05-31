using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosionRule
{
    public List<IHittable> Validate(IExplodable explodable, Board board);
}
