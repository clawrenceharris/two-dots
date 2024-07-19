using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTypeExtensions
{

   


    public static bool IsLotusDot(this DotType type)
    {
        return type == DotType.LotusDot;
    }



    public static bool CanConnectToAnyDot(this DotType type)
    {
        return type == DotType.BlankDot || type == DotType.ClockDot || type == DotType.Magnet;
    }


    public static bool IsConnectableDot(this DotType type)
    {
        return type == DotType.NormalDot || type.CanConnectToAnyDot() || type == DotType.BeetleDot;
    }
    

    public static bool ShouldBeHitBySquare(this DotType type)
    {
        
        return type == DotType.MoonStoneDot || type == DotType.GeoidDot;   
    }

    
    public static bool IsBoardMechanicTile(this TileType type)
    {
        return type == TileType.Block || type == TileType.OneSidedBlock || type == TileType.EmptyTile;
    }
    public static bool IsExplosion(this HitType type)
    {
        return type == HitType.BombExplosion || type == HitType.GemExplosion;
    }

    public static bool IsColorable(this DotType type)
    {
        return type == DotType.NormalDot ||type == DotType.LotusDot || type == DotType.BeetleDot;
    }

    public static bool IsNumerable(this DotType type)
    {
        return type == DotType.ClockDot;
    }
    public static bool IsNumerable(this TileType type)
    {
        return type == TileType.Zapper;
    }

    public static bool IsDirectional(this TileType type)
    {
        return type == TileType.OneSidedBlock;
    }

    public static bool IsDirectional(this DotType type)
    {
        return type == DotType.BeetleDot || type == DotType.MonsterDot;
    }
    public static bool IsConnection(this HitType type)
    {
        return type == HitType.Square || type == HitType.Connection;
    }

    public static bool IsBasicDot(this DotType type)
    {
        return type == DotType.BlankDot || type == DotType.NormalDot;
    }
    public static bool IsClockDot(this DotType type)
    {
        return type == DotType.ClockDot;
    }

    public static bool IsMoonstoneDot(this DotType type)
    {
        return type == DotType.MoonStoneDot;
    }

    public static CommandType ToCommandType(this ExplosionType type)
    {
        return type switch
        {
            ExplosionType.BombExplosion => CommandType.BombExplode,
            ExplosionType.GemExplosion => CommandType.GemExplode,
            _ => CommandType.None,
        };
    }
}


