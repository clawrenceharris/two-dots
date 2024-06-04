using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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



public class DotToSpawnData
{
    public string type;
}

public class DotData : DotToSpawnData
{
    public int col;
    public int row;

}

public interface IColorableData
{
    public string Color { get; set; }
}


public class ColorableDotData : DotData, IColorableData
{

    public string Color { get; set; }
}


public class ColorableDotToSpawnData : DotToSpawnData, IColorableData
{
    public string Color { get; set; }
}

public interface INumerableData 
{

    public int Number { get; set; }
}

public class NumerableDotData : DotData, INumerableData
{
    public int Number { get; set; }
}


public class NumberDotToSpawnData : DotToSpawnData, INumerableData
{
    public int Number { get; set; }
}

public interface IDirectionalData
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }

}


public class DirectionalDotData : DotData, IDirectionalData
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }

}

public class DirectionalDotToSpawnData : DotToSpawnData, IDirectionalData
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }

}

public class DirectionalColorDotData : DotData, IDirectionalData, IColorableData
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }
    public string Color { get; set; }

}

public class DirectionalColorDotToSpawnData : DotToSpawnData, IDirectionalData, IColorableData
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }
    public string Color { get; set; }

}



public class TileData
{
    public string type;
    public int row;
    public int col;

}

public class DirectionalTileData : TileData, IDirectionalData
{
    public int DirectionX { get; set; }
    public int DirectionY { get; set; }
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

