using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionRule : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream
        
    }
=======
        Connection connection = ConnectionManager.Connection;
        MatchingColorRule colorRule = new();
        AdjacentPositionRule adjacentRule = new();
        
        if ( b is IColorable d )
        {
            if (!colorRule.Validate(connection.Color, d.Color, board))
            {
                return false;
            }
        }
        
        
>>>>>>> Stashed changes

    // Update is called once per frame
    void Update()
    {
        
    }
}
