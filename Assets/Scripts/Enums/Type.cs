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
    public enum BoardMechanicType
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
        Glacier



    }


    public enum TileType
    {
        None,
        OneSidedBlock,
        EmptyTile,
        Ramp,
        NeonLight,
        BeeHive,
        PartyPopper,
        Scroll,
        Snow,
        BlockTile,
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
    }

    public enum CommandType
    {
        Hit,
        Clear,
        MoveClockDots,
        Board,
        Explosion,
        HitClockDots,
        MoveBeetleDots,
        BlockTile,
        NestingDot,
        MonsterDot,
        BeetleDot,
        Bomb,
        ClockDot,
        BlankDot,
        NormalDot,
        AnchorDot,
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


    public static bool ShouldBeHitBySquare(IHittable hittable)
    {
        if(hittable is Dot dot)
        {
            return dot.DotType == DotType.MoonStoneDot || dot.DotType == DotType.GeoidDot;

        }
        return false;
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

    
}


