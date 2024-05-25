using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectByColorRule : IConnectionRule
{
    public bool Validate(ConnectableDot dot)
    {
        Connection connection = ConnectionManager.Connection;
        if (connection.Color == DotColor.Blank ||
            dot is IBlankDot)
        {
            return true;
        }
        else if (dot is IColorable colorDot)
        {
            return connection.Color == colorDot.Color;

        }
        return false;
    }
}
