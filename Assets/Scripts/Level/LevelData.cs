using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
[Serializable]
public class LevelData
{
    public int levelNum;
    public int width;
    public int height;
    public int moves;
    public DotsGameObjectData[] initialDotsToSpawn;

    public DotsGameObjectData[] dotsToSpawn;
    public DotsGameObjectData[] dotsOnBoard;
    public DotsGameObjectData[] tilesOnBoard;
}



[Serializable]
public class DotsGameObjectData
{
    public int col;
    public int row;
    public int hitCount;
    public string type;
    public DotsGameObjectData(string type)
    {
        this.type = type;
    }

    // Dictionary to hold dynamic properties
    private readonly Dictionary<string, object> properties = new();

    public T GetProperty<T>(string key)
    {
        if (properties.ContainsKey(key))
        {
            return (T)properties[key];
        }
        return default;
    }

    public void SetProperty<T>(string key, T value)
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

