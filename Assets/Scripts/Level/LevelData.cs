using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Type;

[Serializable]
public class LevelData
{
    public int levelNum;
    public int width;
    public int height;
    public int moves;
    public DotToSpawnData[] dotsToSpawn;
    public DotData[] dotsOnBoard;
    public TileData[] tilesOnBoard;
}


public class DotData
{
    public string type;
    public int col;
    public int row;

}

public class DirectionalTileData : TileData
{
    public Direction direction;
}

public class DirectionalDotData : DotData
{
    public Direction direction;

}

public class DirectionalDotToSpawnData : DotToSpawnData
{
    public Direction direction;

}

[Serializable]
public class Direction
{
    public int x;
    public int y;
}

[Serializable]
public class ColorDotData : DotData
{

    public string color;
}

public class NumberDotData : DotData
{

    public int number;
}



[Serializable]
public class DotToSpawnData
{
    public string type;
}


public class ClockDotData : NumberDotData
{

}




[Serializable]
public class ColorDotToSpawnData : DotToSpawnData
{
    public string color;
}

[Serializable]
public class NumberDotToSpawnData : DotToSpawnData
{
    public int number;
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

[Serializable]
public class TileData
{
    public string type;
    public int row;
    public int col;

}
