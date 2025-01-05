using System.Collections.Generic;
using System;

[Serializable]
public class LevelData
{
    public int levelNum;
    public int width;
    public int height;
    public int moves;
    public string[] colors;

    public DotObject[] initDotsToSpawn;

    public DotObject[] dotsToSpawn;
    public DotObject[] dotsOnBoard;
    public DotObject[] tilesOnBoard;
}



[Serializable]
public class DotObject
{
    public int col;
    public int row;
    public int hitCount;
    public string type;
    
    public enum Property{
        Color,
        Directions,
        Type,
        Number,

        Active,
        Position
    }
    // Dictionary to hold dynamic properties
    private readonly Dictionary<Property, object> properties = new();

    public T GetProperty<T>(Property key)
    {
        if (properties.ContainsKey(key))
        {
            return (T)properties[key];
        }
        return default;
    }

    public void SetProperty<T>(Property key, T value)
    {
        if (properties.ContainsKey(key))
        {
            properties[key] = value;
        }
        else
        {
            properties.Add(key, value);
        }
    }
}


[Serializable]
public class GoalData
{
    public string type;
    public int amount;
}

[Serializable]
public class TutorialData
{
    public string topText;
    public string bottomText;
    public PositionData[] connection;
}

[Serializable]
public class PositionData
{
    public int row;
    public int column;
}

