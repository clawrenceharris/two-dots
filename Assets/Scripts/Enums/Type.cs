using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Type
{

    public enum DotType
    {
        None,

        BlankDot,
        ClockDot,
        LotusDot,
        NestingDot,
        Bomb,
        MoonStoneDot,
        AnchorDot,
        BeetleDot,
        NormalDot,
        EnchantedDot,
        Magnet,
        Lantern,
        Gem,
        SunDot,
        SeedDot,
        GeoidDot,
        MonsterDot,
    }
    public enum SquareType
    {
        BlankSqaure,
        NoramlSqaure,
        BigSqaure,

    }
    public enum TileType
    {
        None,
        Ice,
        Water,
        Vine,
        Train,
        Slime,
        CeramicTile,
        SunGate,
        Circut,
        Glacier,
        OneSidedBlock,
        EmptyTile,
        Ramp,
        NeonLight,
        BeeHive,
        PartyPopper,
        Scroll,
        Snow,
        Block,


    }



    public enum HitType
    {
        None,
        Connection,
        BombExplosion,
        AnchorDot,
        BlockTile,
        Explosion,
        GemExplosion,
        Square,
        ClockDot,
        OneSidedBlock,
        NestingDot,
        BeetleDot,
        MonsterDot,
        LotusDot,
    }

    public enum PreviewHitType
    {
        None,
        Connection,
        BombExplosion,
        AnchorDot,
        BlockTile,
        Explosion,
        GemExplosion,
        Square,
        ClockDot,
        OneSidedBlock,
        NestingDot,
        BeetleDot,
        Disconnection,
    }

    public enum CommandType
    {
        SinkAnchorDots,
        ClearLotusDots,

        Hit,
        MoveClockDots,

        Clear,

        GemExplode,
        Board,

        Explosion,

        BombExplode,
        MoveBeetleDots,
        MoveMonsterDots,
    }

    public enum ExplosionType
    {
        BombExplosion,
        GemExplosion,


    }
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
    public static bool IsColorDot(this DotType type)
    {
        return type == DotType.NormalDot || type == DotType.LotusDot || type == DotType.BeetleDot;
    }

    public static bool HasOrientation(this DotType type)
    {
        return type == DotType.BeetleDot || type == DotType.EnchantedDot;
    }


    public static bool ShouldBeHitBySquare(this DotType type)
    {
        
        return type == DotType.MoonStoneDot || type == DotType.GeoidDot;   
    }

    
    public static bool IsBoardMechanicTile(this TileType type)
    {
        return type == TileType.Block || type == TileType.OneSidedBlock;
    }
    public static bool IsExplosion(this HitType type)
    {
        return type == HitType.BombExplosion || type == HitType.GemExplosion;
    }

    public static bool HasColor(this DotType type)
    {
        return type == DotType.NormalDot ||type == DotType.LotusDot || type == DotType.BeetleDot;
    }

    public static bool HasNumber(this DotType type)
    {
        return type == DotType.ClockDot;
    }

    public static bool HasDirection(this TileType type)
    {
        return type == TileType.OneSidedBlock;
    }

    public static bool HasDirection(this DotType type)
    {
        return type == DotType.BeetleDot;
    }
    public static bool IsConnection(this HitType type)
    {
        return type == HitType.Square || type == HitType.Connection;
    }

    public static CommandType ToCommandType(this ExplosionType type)
    {
        switch (type)
        {
            case ExplosionType.BombExplosion:
                return CommandType.BombExplode;
            case ExplosionType.GemExplosion:
                return CommandType.GemExplode;
            default: throw new ArgumentException();
            
        }
    }

    public static bool IsBasicDot(this DotType type)
    {
        return type == DotType.BlankDot || type == DotType.NormalDot;
    }
    public static bool IsClockDot(this DotType type)
    {
        return type == DotType.ClockDot;
    }
}


