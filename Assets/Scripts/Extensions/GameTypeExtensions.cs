using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class DotsExtensions{
    public static List<Dot> FindDotNeighbors(this DotsGameObject dot, Board board){
        return board.GetDotNeighbors(dot.Column, dot.Row);  
    }
    public static List<Tile> FindTileNeighbors(this DotsGameObject dot, Board board){
        return board.GetTileNeighbors(dot.Column, dot.Row);  
    }
    public static List<T> FindTileNeighbors<T>(this DotsGameObject dot, Board board)
     where T :class{
        return board.GetTileNeighbors<T>(dot.Column, dot.Row);  
    }
    public static List<T> FindDotNeighbors<T>(this DotsGameObject dot, Board board)
     where T :class{
        return board.GetDotNeighbors<T>(dot.Column, dot.Row);
    }
    public static List<DotsGameObject> FindNeighbors(this DotsGameObject dot, Board board)
    {
        return board.GetNeighbors(dot.Column, dot.Row);
    }

    public static List<T> FindNeighbors<T>(this DotsGameObject dot, Board board)
    where T : class
    {
        return board.GetNeighbors<T>(dot.Column, dot.Row);
    }
}

public static class GameTypeExtensions
{

    
    public static bool IsBlockable(this TileType type){
        return type == TileType.Block || type == TileType.OneSidedBlock;

    }

    public static bool IsLotusDot(this DotType type)
    {
        return type == DotType.LotusDot;
    }

    public static bool IsWater(this TileType type)
    {
        return type == TileType.Water;
    }

    public static bool IsBlank(this DotType type)
    {
        return type == DotType.BlankDot || type == DotType.ClockDot || type == DotType.Magnet;
    }

    public static bool IsBlank(this DotColor type)
    {
        return type == DotColor.Blank;
    }
    public static bool IsConnectableDot(this DotType type)
    {
        return type == DotType.NormalDot || type.IsBlank() || type == DotType.BeetleDot;
    }
    

    public static bool ShouldBeHitBySquare(this DotType type)
    {
        
        return type == DotType.MoonStoneDot;   
    }
    public static bool ShouldBeHitBySquare(this TileType type)
    {
        
        return false;   
    }

    
    public static bool IsBoardMechanicTile(this TileType type)
    {
        return type == TileType.Block || type == TileType.OneSidedBlock || type == TileType.EmptyTile;
    }
    public static bool IsExplosion(this HitType type)
    {
        return type == HitType.BombExplosion || type == HitType.GemExplosion || type == HitType.Explosion;
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
    public static HitType ToHitType(this CommandType type)
    {
        return type switch
        {
            CommandType.BombExplode => HitType.BombExplosion,
            CommandType.GemExplode => HitType.GemExplosion,
            _ => HitType.None,
        };
    }
}


