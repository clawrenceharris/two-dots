using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConnectionRule
{
    public bool Validate(ConnectableDot dot);
}
